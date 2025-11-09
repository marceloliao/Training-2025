using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;


namespace CSF_Reorganize_TMs
{

    public class Program
    {
        public const string filePath = @"E:\SDL Archive\104 CSF Inc\2025-02-05 TM Test";
        private const string header = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<tmx version=\"1.4\">\r\n<header creationtool=\"SDL Language Platform\" creationtoolversion=\"8.1\" o-tmf=\"SDL TM8 Format\" datatype=\"xml\" segtype=\"sentence\" adminlang=\"en-US\" srclang=\"en-US\" creationdate=\"20241107T103809Z\" creationid=\"SDLPRODUCTS\\maliao\">\r\n<prop type=\"x-sourceFilename:SingleString\"/>\r\n<prop type=\"x-targetFilename:SingleString\"/>\r\n</header>\r\n<body>";
        private const string footer = "</body>\r\n</tmx>";

        private static void Main(string[] args)
        {
            // Create service collection and configure our services
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            // Get service and call method
            var app = serviceProvider.GetService<Execution>();
            app.Run();
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            // Configure NLog
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                loggingBuilder.AddNLog("NLog.config");
            });

            // Register the application entry point
            services.AddTransient<Execution>();

            return services;
        }

       internal class Execution
        {
            private readonly ILogger<Execution> _logger;

            public Execution(ILogger<Execution> logger)
            {
                _logger = logger;
            }

            public void Run()
            {
                Console.WriteLine(filePath);
                
                ProcessFiles(filePath);
            }

            private void ProcessFiles(string path)
            {
                // Get all files in the current directory
                string[] files = Directory.GetFiles(path);

                foreach (string file in files)
                {
                    if (!Path.GetFileNameWithoutExtension(file).Contains("_modified"))
                    {
                        _logger.LogInformation($"Processing the file {file}");

                        XDocument xFile = XDocument.Load(file);

                        var translationUnits = from c in xFile.Descendants()
                                               where (c.Name == "tu" || c.Name == "TU")
                                               select c;
                        int counter = 0;

                        XElement? sourceProp = null;
                        XElement? targetProp = null;

                        // Write to a new TMX file
                        string newTMX = $"{Path.GetDirectoryName(file)}\\{Path.GetFileNameWithoutExtension(file)}_modified.tmx";

                        if (File.Exists(newTMX))
                            File.Delete(newTMX);

                        // Insert the header
                        using (StreamWriter sw = new StreamWriter(newTMX, true, Encoding.UTF8))
                        {
                            sw.WriteLine(header);
                            sw.Close();
                        }

                        // Append the TU to the file
                        using (StreamWriter sw = new StreamWriter(newTMX, true, Encoding.UTF8))
                        {
                            foreach (var translationUnit in translationUnits)
                            {
                                var languages = from c in translationUnit.Descendants()
                                                where (c.Name == "tuv" || c.Name == "TUV")
                                                select c;

                                if (languages.Count() == 2)
                                {
                                    //string sourceLanguage = languages.First().Attribute("xml:lang").Value;
                                    //string targetLanguage = languages.Last().Attribute("xml:lang").Value;

                                    var sourceSegContent = from c in languages.First().Descendants()
                                                           where (c.Name == "seg" || c.Name == "SEG")
                                                           select c;

                                    var targetSegContent = from c in languages.Last().Descendants()
                                                           where (c.Name == "seg" || c.Name == "SEG")
                                                           select c;

                                    if ((sourceSegContent.First().Value.ToLower().Contains("_en.") && targetSegContent.First().Value.ToLower().Contains("_fr.")) || (sourceSegContent.First().Value.ToLower().Contains("-en.") || targetSegContent.First().Value.ToLower().Contains("-fr.")) || (sourceSegContent.First().Value.ToLower().Contains("language=en") && targetSegContent.First().Value.ToLower().Contains("language=fr")))
                                    {
                                        // Found the file path and need to add them as properties
                                        sourceProp = new XElement("prop", sourceSegContent.First().Value);
                                        sourceProp.SetAttributeValue("type", "x-sourceFilename:SingleString");
                                        targetProp = new XElement("prop", targetSegContent.First().Value);
                                        targetProp.SetAttributeValue("type", "x-targetFilename:SingleString");
                                    }
                                    else
                                    {
                                        languages.First().AddFirst(sourceProp);
                                        languages.Last().AddFirst(targetProp);

                                        // Define a regular expression pattern to match a file path
                                        string pattern = "(<prop type=\"x-(source|target)Filename:SingleString\">)(.*)\\r\\n(.*?</prop>)";

                                        // Create an instance of the Regex class
                                        Regex reg = new Regex(pattern);

                                        String translationUnitString = translationUnit.ToString();

                                        MatchCollection matches = reg.Matches(translationUnitString);

                                        if (matches.Count != 0)
                                            translationUnitString = reg.Replace(translationUnitString, "$1$3$4");

                                        // Define another regular expression pattern to match a file path
                                        pattern = "(<prop type=\"x-(source|target)Filename:SingleString\">).*\\\\(.*)(</prop>)";

                                        reg = new Regex(pattern);

                                        matches = reg.Matches(translationUnitString);

                                        if (matches.Count != 0)
                                            translationUnitString = reg.Replace(translationUnitString, "$1$3$4");

                                        // Define another regular expression pattern to match line break before </seg> 
                                        pattern = "\\r\\n</seg>";

                                        reg = new Regex(pattern);

                                        matches = reg.Matches(translationUnitString);

                                        if (matches.Count != 0)
                                            translationUnitString = reg.Replace(translationUnitString, "</seg>");

                                        sw.WriteLine(translationUnitString);
                                    }
                                }
                                else
                                {
                                    _logger.LogError($"The Translation Unit {counter} contain {languages.Count()} languages");
                                }

                                counter++;
                            }
                            sw.Close();
                        }

                        // Write the footer to the file
                        using (StreamWriter sw = new StreamWriter(newTMX, true, Encoding.UTF8))
                        {
                            sw.WriteLine(footer);
                            sw.Close();
                        }
                    }
                }

                // Get all subdirectories in the current directory
                string[] subdirectories = Directory.GetDirectories(path);

                foreach (string subdirectory in subdirectories)
                {
                    // Recursively call ListFiles on each subdirectory
                    ProcessFiles(subdirectory);
                }
            }
        }
    }
}



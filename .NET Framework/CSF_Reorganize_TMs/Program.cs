using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using NLog;

namespace CSF_Reorganize_TMs
{
    internal class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        //private const string rootFolder = @"C:\Users\maliao\Documents\PS Projects\104 CSF Inc\2025-01-20 NAV second TM to import\csfinc_segments-ltw-nav-2025-01-16_1104-zip_2025-01-20_1555\segments-ltw-NAV-2025-01-16_1104";
        private const string header = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<tmx version=\"1.4\">\r\n<header creationtool=\"SDL Language Platform\" creationtoolversion=\"8.1\" o-tmf=\"SDL TM8 Format\" datatype=\"xml\" segtype=\"sentence\" adminlang=\"en-US\" srclang=\"en-US\" creationdate=\"20241107T103809Z\" creationid=\"SDLPRODUCTS\\maliao\">\r\n<prop type=\"x-sourceFilename:SingleString\"/>\r\n<prop type=\"x-targetFilename:SingleString\"/>\r\n</header>\r\n<body>";
        private const string footer = "</body>\r\n</tmx>";

        static void Main(string[] args)
        {
            string rootFolder = "";

            try
            {
                Console.WriteLine("Veuillez saisir un chemin de répertoire valide : ");
                rootFolder = Path.GetFullPath(Console.ReadLine());                
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Veuillez vérifier si le chemin est valide.");
            }

            logger.Info($"Starting the execution...");
            
            ProcessFiles(rootFolder);

            logger.Info($"Finalizing the execution...");
        }       

        static void ProcessFiles(string path)
        {
            // Get all files in the current directory
            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                if (!Path.GetFileNameWithoutExtension(file).Contains("_modified"))
                {
                    logger.Info($"Processing the file {file}");

                    XDocument xFile = XDocument.Load(file);

                    var translationUnits = from c in xFile.Descendants()
                                           where (c.Name == "tu" || c.Name == "TU")
                                           select c;
                    int counter = 0;

                    XElement sourceProp = null;
                    XElement targetProp = null;

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
                                logger.Error($"The Translation Unit {counter} contain {languages.Count()} languages");
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

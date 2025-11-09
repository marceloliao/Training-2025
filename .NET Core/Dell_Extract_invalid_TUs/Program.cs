using NLog;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
//using NLog.Config;
//using NLog.Targets;

namespace Dell_Extract_invalid_TUs 
{
    public class Program()
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        //private const string fileLocation = @"C:\Users\maliao\Documents\PS Projects\101 Dell TE transition\2025-01-30 Extract TUs\From Christiane\Interim TMX - 1";
        private const string header = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<tmx version=\"1.4\">\r\n  <header creationtool=\"SDL Language Platform\" creationtoolversion=\"8.1\" o-tmf=\"SDL TM8 Format\" datatype=\"xml\" segtype=\"sentence\" adminlang=\"en-US\" srclang=\"en-US\" creationdate=\"20240722T095304Z\" creationid=\"unknown\">\r\n    <prop type=\"x-Recognizers\">RecognizeAll</prop>\r\n    <prop type=\"x-IncludesContextContent\">True</prop>\r\n    <prop type=\"x-TMName\">Legal</prop>\r\n    <prop type=\"x-TokenizerFlags\">DefaultFlags</prop>\r\n    <prop type=\"x-WordCountFlags\">DefaultFlags</prop>\r\n  </header>\r\n  <body>";
        private const string footer = "</body>\r\n</tmx>";

        private static void Main(string[] args)
        {
            // Configure NLog -- This block is not really needed
            //var config = new NLog.Config.XmlLoggingConfiguration("NLog.config");
            //LogManager.Configuration = config;

            string? fileLocation = ""; 

            try
            {
                Console.WriteLine("Please enter a valid directory path: ");
                fileLocation = Console.ReadLine();
                
            }
            catch (FormatException)
            {
                Console.WriteLine("Please enter a valid path.");
            }

            // Create a new TMX file that contains all problematic TUs
            string combinedTMX = $"{fileLocation}\\TUs_exceeding_2000_combined.tmx";

            if (File.Exists(combinedTMX))
                File.Delete(combinedTMX);

            // Write the header
            using (StreamWriter sw = new StreamWriter(combinedTMX, true, Encoding.UTF8))
            {
                sw.WriteLine(header);
                sw.Close();
            }            

            logger.Info($"Starting...");

            // Call the method to list files
            ProcessFiles(fileLocation, combinedTMX);

            // Write the footer to the file
            using (StreamWriter sw = new StreamWriter(combinedTMX, true, Encoding.UTF8))
            {
                sw.WriteLine(footer);
                sw.Close();
            }            
        }

        static void ProcessFiles(string path, string combinedFile)
        {
            // Get all files in the current directory
            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                if (!Path.GetFileNameWithoutExtension(file).Contains("exceeding_2000"))
                {
                    logger.Info($"Processing the file {file}");

                    // Read all lines from the file
                    string[] lines = File.ReadAllLines(file);

                    // Append the TU to the file
                    using (StreamWriter sw = new StreamWriter(combinedFile, true, Encoding.UTF8))
                    {
                        int counter = 0;
                        bool foundStart = false;
                        bool foundEnd = false;
                        StringBuilder sb = new StringBuilder();

                        // Loop through each line and process it                        
                        foreach (string line in lines)
                        {
                            counter++;
                            if (line.Contains("<!--Error: TMTUSourceSegmentSizeLimitExceeded-->") || line.Contains("<!--Error: TMTUTargetSegmentSizeLimitExceeded-->"))
                            {
                                foundStart = true;
                                foundEnd = false;
                                sb.Clear();
                                sb.Append(line + "\r\n");
                                logger.Info($"Found a new problematic TU at the line {counter}");
                            }
                            else if (line.Contains("</tu>") && (foundStart) && (!foundEnd))
                            {
                                foundStart = false;
                                foundEnd = true;
                                sb.Append(line + "\r\n");
                                sw.Write(sb.ToString());
                                sb.Clear();                            }
                            else if ((foundStart) && (!foundEnd))
                            {
                                sb.Append(line + "\r\n");
                            }
                            else
                            {
                                // Do nothing
                            }
                        }

                        sw.Close();
                    }
                }                             
            }

            // Get all subdirectories in the current directory
            string[] subdirectories = Directory.GetDirectories(path);

            foreach (string subdirectory in subdirectories)
            {
                // Recursively call ListFiles on each subdirectory
                ProcessFiles(subdirectory, combinedFile);
            }
        }
    }
}



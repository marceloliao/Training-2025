using NLog;
using System.Text;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dell_Extract_Cloud_CIQ_TUs
{
    public class Program()
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        //private const string fileLocation = @"C:\Users\maliao\Documents\PS Projects\101 Dell TE transition\2025-01-30 Extract TUs\From Christiane\Interim TMX - 1";
        private const string header = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<tmx version=\"1.4\">\r\n  <header creationtool=\"SDL Language Platform\" creationtoolversion=\"8.1\" o-tmf=\"SDL TM8 Format\" datatype=\"xml\" segtype=\"sentence\" adminlang=\"en-US\" srclang=\"en-US\" creationdate=\"20241105T092720Z\" creationid=\"SDLPRODUCTS\\tpearce\">\r\n    <prop type=\"x-StructureContext:MultipleString\"></prop>\r\n    <prop type=\"x-TmsOriginalUpdateTmTimestamp:DateTime\"></prop>\r\n    <prop type=\"x-TmsOriginalFileName:SingleString\"></prop>\r\n    <prop type=\"x-Tms JobId:Integer\"></prop>\r\n    <prop type=\"x-Quality Category:SingleString\"></prop>\r\n    <prop type=\"x-Quote Approval Required:SingleString\"></prop>\r\n    <prop type=\"x-Document Type:SingleString\"></prop>\r\n    <prop type=\"x-Target Audience:SingleString\"></prop>\r\n    <prop type=\"x-Cost Center:SingleString\"></prop>\r\n    <prop type=\"x-Translation PO Number:SingleString\"></prop>\r\n    <prop type=\"x-Secondary PO Number:SingleString\"></prop>\r\n    <prop type=\"x-FTP/OneDrive Location:SingleString\"></prop>\r\n    <prop type=\"x-Taxonomy - Business Unit:SingleString\"></prop>\r\n    <prop type=\"x-Taxonomy - Line of Business:SingleString\"></prop>\r\n    <prop type=\"x-Taxonomy - Product Family:SingleString\"></prop>\r\n    <prop type=\"x-Dell PM Email:SingleString\"></prop>\r\n    <prop type=\"x-Engagement Model:SingleString\"></prop>\r\n    <prop type=\"x-OrganizationName:SingleString\"></prop>\r\n    <prop type=\"x-OrganizationId:Integer\"></prop>\r\n    <prop type=\"x-ConfigName:SingleString\"></prop>\r\n    <prop type=\"x-ConfigId:Integer\"></prop>\r\n    <prop type=\"x-VendorId:Integer\"></prop>\r\n    <prop type=\"x-VendorName:SingleString\"></prop>\r\n    <prop type=\"x-Rapid Response:SingleString\"></prop>\r\n    <prop type=\"x-Details:SingleString\"></prop>\r\n    <prop type=\"x-Recognizers\">RecognizeAll</prop>\r\n    <prop type=\"x-IncludesContextContent\">True</prop>\r\n    <prop type=\"x-TMName\">Software</prop>\r\n    <prop type=\"x-TokenizerFlags\">DefaultFlags</prop>\r\n    <prop type=\"x-WordCountFlags\">DefaultFlags</prop>\r\n  </header>\r\n  <body>";
        private const string footer = "</body>\r\n</tmx>";

        private static void Main(string[] args)
        {
            // Configure NLog -- This block is not really needed
            //var config = new NLog.Config.XmlLoggingConfiguration("NLog.config");
            //LogManager.Configuration = config;

            string fileLocation = "";
            string languagePair = "";

            try
            {
                Console.WriteLine("Please enter a valid directory path: ");
                fileLocation = Console.ReadLine();

                Console.WriteLine("Please enter the language combination: ");
                languagePair = Console.ReadLine();
            }
            catch (FormatException)
            {
                Console.WriteLine("Please enter a valid path.");
            }

            logger.Info($"Starting...");

            // Create 4 new TMX files that contains all Cloud CIQ TUs
            string[] cloudCIQConfigs = new string[4];

            cloudCIQConfigs[0] = "CIQ APEX Software GUI";
            cloudCIQConfigs[1] = "CIQ APEX Software GUI - InContext Review";
            cloudCIQConfigs[2] = "CIQ APEX Software GUI - Review - Defect Fixing";
            cloudCIQConfigs[3] = "CIQ Software GUI_Testing";

            for (int i = 0; i < cloudCIQConfigs.Length; i++)
            {
                string tmxFileName = $"{fileLocation}\\TUs_{languagePair}_CloudCIQ_{cloudCIQConfigs[i]}.tmx";

                if (File.Exists(tmxFileName))
                    File.Delete(tmxFileName);

                // Write the header
                using (StreamWriter sw = new StreamWriter(tmxFileName, true, Encoding.UTF8))
                {
                    sw.WriteLine(header);
                    sw.Close();
                }

                // Call the method to list files
                ProcessFiles(fileLocation ?? "no path found", tmxFileName, cloudCIQConfigs[i]);

                // Write the footer to the file
                using (StreamWriter sw = new StreamWriter(tmxFileName, true, Encoding.UTF8))
                {
                    sw.WriteLine(footer);
                    sw.Close();
                }
            }

            logger.Info($"Finalizing...");
        }

        static void ProcessFiles(string path, string combinedFile, string configName)
        {
            // Get all files in the current directory
            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                if (!Path.GetFileNameWithoutExtension(file).Contains("CloudCIQ"))
                {
                    logger.Info($"Processing the file {file}");

                    // Extract all TUs
                    XDocument xFile = XDocument.Load(file);

                    var translationUnits = from c in xFile.Descendants()
                                           where (c.Name == "tu" || c.Name == "TU")
                                           select c;                    
                   
                    // Append Cloud CIQ TUs to the file
                    using (StreamWriter sw = new StreamWriter(combinedFile, true, Encoding.UTF8))
                    {
                        foreach (var translationUnit in translationUnits)
                        {
                            string orgName = (from c in translationUnit.Descendants()
                                              where ((string?)c.Attribute("type") == "x-OrganizationName:SingleString")
                                              select c).First().Value;

                            string config = (from c in translationUnit.Descendants()
                                             where ((string?)c.Attribute("type") == "x-ConfigName:SingleString")
                                             select c).First().Value;

                            if (orgName == "Global Cloud CIQ" && config == configName)
                                sw.WriteLine(translationUnit.ToString());
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
                ProcessFiles(subdirectory, combinedFile, configName);
            }
        }
    }
}



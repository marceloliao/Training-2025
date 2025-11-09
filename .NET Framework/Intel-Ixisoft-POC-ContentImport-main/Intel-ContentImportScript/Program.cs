using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelContentImportScript
{
    public class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));
        public static string RunExePreorPostConversion = ConfigurationManager.AppSettings["RunExePreorPostConversion"];
        public static string customPropertiesExtension = ConfigurationManager.AppSettings["custompropertiesExttension"];
        public static string documentPropertiesExtension = ConfigurationManager.AppSettings["documentPropertiesExtension"];     
        private static string sourceFolder = ConfigurationManager.AppSettings["ContentImporterSource"];
        private static string workFolder = ConfigurationManager.AppSettings["ContentImporterWork"];
        private static string outFolder = ConfigurationManager.AppSettings["ContentImporterOut"];
        private static string versionFolderPrefix = ConfigurationManager.AppSettings["VersionFolderPrefix"];
        private static string versionSearchPattern = ConfigurationManager.AppSettings["VersionSearchPattern"];
        private static string versionReplacePattern = ConfigurationManager.AppSettings["VersionReplacePattern"];
        private static string revisionReplacePattern = ConfigurationManager.AppSettings["RevisionReplacePattern"];
        private static string ditaFilePattern = ConfigurationManager.AppSettings["DitaFilePattern"];
        private static string zipArchivePattern = ConfigurationManager.AppSettings["ZipArchivePattern"];
        private static string imageFileExtension = ConfigurationManager.AppSettings["ImageFileExtension"];
        private static string defaultTargetFolder = ConfigurationManager.AppSettings["DefaultTargetFolder"];
        private static string folderInfoFolder = ConfigurationManager.AppSettings["FolderInfoFolder"];
        private static string folderInfoSourcePattern = ConfigurationManager.AppSettings["FolderInfoSourcePattern"];
        private static string inFolder = ConfigurationManager.AppSettings["ContentImporterIn"];
        private static string filemapFilename = ConfigurationManager.AppSettings["FilemapFilename"];
        private static string databaseFilename = ConfigurationManager.AppSettings["DatabaseFilename"];
        private static string tridionServerUrl = ConfigurationManager.AppSettings["TridionDocsServerUrl"];
        private static string parseTimeOption = ConfigurationManager.AppSettings["ParseTime"];
        private static bool parseTime = string.IsNullOrEmpty(parseTimeOption) ? false : Convert.ToBoolean(parseTimeOption);
        private static string ishFilePattern = ConfigurationManager.AppSettings["IshFilePattern"];
        private static string noTranslationManagement = string.IsNullOrEmpty(ConfigurationManager.AppSettings["FNOTRANSLATIONMGMT"]) ?
            "no" : ConfigurationManager.AppSettings["FNOTRANSLATIONMGMT"];


        public static List<KeyValuePair<string, string>> languageMaps = GetLanguageMaps();

        static void Main(string[] args)
        {
            Logger.Debug("Starting Intel-ContentImportScripts.exe");

            if (string.IsNullOrEmpty(RunExePreorPostConversion))
            {
                if (args != null && args.Length == 0)
                {
                    //Console.WriteLine("Commandline args is empty.");
                    Console.Write("Running script on PREConversion or POST converstion. Enter -pre/-post: ");
                    string command = Console.ReadLine();
                    RunExePreorPostConversion = command;

                }
            }

            if (RunExePreorPostConversion.ToLower() != "-pre" && RunExePreorPostConversion.ToLower() != "-post")
            {
                Console.Write("IntelContentImportScript.exe args invalid. Enter IntelContentImportScript.exe -pre/-post");
                Console.Write("Or update IntelContentImportScript.exe config to execute  -pre/-post");
                Logger.Error("IntelContentImportScript.exe args invalid. Enter IntelContentImportScript.exe -pre/-post. Or update IntelContentImportScript.exe config to execute  -pre/-post");

                return;
            }

            try
            {
                if (RunExePreorPostConversion.ToLower() == "-pre")
                {
                    PreConversionProcess preConversionProcess = new PreConversionProcess(Logger);
                    preConversionProcess.PreProcess(sourceFolder,
                        workFolder,
                        versionFolderPrefix,
                        versionSearchPattern,
                        versionReplacePattern,
                        revisionReplacePattern,
                        inFolder,
                        ditaFilePattern,
                        zipArchivePattern,
                        folderInfoFolder,
                        folderInfoSourcePattern,
                        imageFileExtension,
                        defaultTargetFolder,
                        languageMaps);
                }
                else if (RunExePreorPostConversion.ToLower() == "-post")
                {
                    PostConversionProcess postConversionProcess = new PostConversionProcess(Logger);
                    postConversionProcess.PostProcess(outFolder,
                        workFolder,
                        filemapFilename,
                        databaseFilename,
                        tridionServerUrl,
                        parseTime,
                        ishFilePattern,
                        customPropertiesExtension,
                        documentPropertiesExtension,
                        noTranslationManagement);

                }
            }
            catch
            {
                throw;
            }

        }

        private static List<KeyValuePair<string, string>> GetLanguageMaps()
        {
            List<KeyValuePair<string, string>> maps = new List<KeyValuePair<string, string>>();

            var mapSettingsConfig = (MapSettingsConfig)ConfigurationManager.GetSection("mapSettings");

            foreach(MapElement element in mapSettingsConfig.Map)
            {
                maps.Add(new KeyValuePair<string, string>(element.Client, element.Rws));
            }

            return maps;
        }
    }
}

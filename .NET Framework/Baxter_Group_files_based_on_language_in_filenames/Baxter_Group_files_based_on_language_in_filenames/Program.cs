using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Baxter_Group_files_based_on_language_in_filenames
{
    class Program
    {
        // Define basic starting point
        private const string startingFolder = @"E:\2025-02-20 Dell TMX Exports from Tim Pearce Unzipped";
        private const string targetFolder = @"E:\Dell all TMX by language";

        static void Main(string[] args)
        {
            // Loop through all subfolders and extract product name and language from the file name
            string[] directories = Directory.GetDirectories(startingFolder);
            foreach (string directory in directories)
            {
                // Iterate subdirectories
                CheckAllFiles(directory);
            }
        }

        private static void CheckAllFiles(string currentFolder)
        {
            string[] files = Directory.GetFiles(currentFolder);

            foreach (string file in files)
            {
                // Only move the files that have software in the name
                if (Path.GetFileNameWithoutExtension(file).ToLower().Contains("marketing"))
                {
                    // string language = ReturnLanguage(Path.GetFileNameWithoutExtension(file));
                    string languagePair = ReturnLanguagePair(Path.GetFileNameWithoutExtension(file));

                    // Check whether language folder already exists, if not create it
                    string languageFolder = targetFolder + "\\" + languagePair;

                    if (!Directory.Exists(languageFolder))
                        Directory.CreateDirectory(languageFolder);

                    // Copy the file to the target folder
                    string targetFile = languageFolder + "\\" + Path.GetFileName(file);

                    // If file already exists, append additional number at the end
                    //if (File.Exists(targetFile))
                    //{
                    //    // Extract the last letter and check whether it's a number, if yes, increment by 1
                    //    // This logic wouldn't work if some files already have numbers
                    //    string pattern = @"\d";
                    //    string lastLetter = Path.GetFileNameWithoutExtension(targetFile).Substring(Path.GetFileNameWithoutExtension(targetFile).Length - 1);

                    //    Match m = Regex.Match(lastLetter, pattern, RegexOptions.IgnoreCase);

                    //    if (m.Success)
                    //    {
                    //        string newTargetFile = Path.GetDirectoryName(targetFile) + "\\" + Path.GetFileNameWithoutExtension(targetFile).Substring(0, Path.GetFileNameWithoutExtension(targetFile).Length - 2) + "_" + (Convert.ToInt16(m.Value) + 1).ToString() + Path.GetExtension(targetFile);
                    //        File.Copy(file, newTargetFile);
                    //    }
                    //    else
                    //    {
                    //        string newTargetFile = Path.GetDirectoryName(targetFile) + "\\" + Path.GetFileNameWithoutExtension(targetFile).Substring(0, Path.GetFileNameWithoutExtension(targetFile).Length) + "_1" + Path.GetExtension(targetFile);

                    //        if (!File.Exists(newTargetFile))
                    //            File.Copy(file, newTargetFile);
                    //    }
                    //}
                    //else
                    //{
                    //    File.Copy(file, targetFile);
                    //}

                    while (File.Exists(targetFile))
                    {
                        targetFile = Path.GetDirectoryName(targetFile) + "\\" + Path.GetFileNameWithoutExtension(targetFile) + "_1" + Path.GetExtension(targetFile);
                    }
                    File.Copy(file, targetFile);
                }                
            }
        }

        private static string ReturnProductName(string inputText)
        {
            List<string> split = inputText.Split('_').ToList();
            return split[1];
        }

        private static string ReturnLanguage(string inputText)
        {
            List<string> split = inputText.Split('_').ToList();
            return split[split.Count - 1];
        }

        private static string ReturnLanguagePair(string inputText)
        {
            string pattern = @"[a-zA-Z]{2}-[a-zA-Z]{2}_[a-zA-Z]{2}-[a-zA-Z]{2}"; // Lionbridge example: Baxter - Miscellaneous_Final_AMS_new.de-de_fr-fr.tmx
            //string pattern = @"_.*(?=_\d{4}-\d{2}-\d{2}-\d{6})"; // Transperfect example: Baxter Healthcare_zh_en-US_2021-05-27-012549.tmx

            MatchCollection matches = Regex.Matches(inputText, pattern, RegexOptions.IgnoreCase);
            if (matches.Count ==1)
                return matches[0].Value;
            else
                return "No language pair found";
        }
    }

    internal class LangPair
    {
        public string sourceLanguage { get; set; }
        public string targetLanguage { get; set; }

        public LangPair(string sourceLanguage, string targetLanguage)
        {
            this.sourceLanguage = sourceLanguage ?? throw new ArgumentNullException(nameof(sourceLanguage));
            this.targetLanguage = targetLanguage ?? throw new ArgumentNullException(nameof(targetLanguage));
        }
    }
}

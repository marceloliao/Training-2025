using System;
using System.Reflection;
using log4net;
using System.Linq;
using log4net.Config;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Beckman_reorder_Regex_list
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));
        private static readonly string regexFilesPath = System.Configuration.ConfigurationManager.AppSettings["regexFilesLocation"];
        private static readonly string beforeEditingFile = System.Configuration.ConfigurationManager.AppSettings["beforeEditingFilePath"];
        private static readonly string postEditingFile = System.Configuration.ConfigurationManager.AppSettings["postEditingFilePath"];
        private static readonly bool alterMeasureNumbering = System.Configuration.ConfigurationManager.AppSettings["changeMeasurementNumbering"] == "Yes" ? true : false;

        static void Main(string[] args)
        {
            // Get the logger, we need to use the following 3 lines
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            Logger.Debug("Starting Beckman_reorder_Regex_list.exe");

            // Compare both files and compile a report showing differences
            //SortedDictionary<int, string> pattern = CompareTwoFiles(beforeEditingFile, postEditingFile);

            //AddCommentSymbol(new List<int> {67, 68, 80, 81 });

            // maliao: 2022-12-07 Since the numbers have changed, we need a new logic
            SortedDictionary<int, string> pattern = CompareTwoFiles(beforeEditingFile, postEditingFile);

            // Propagate changes to other files
            PropagateChanges(pattern, regexFilesPath);
        }

        private static void AddCommentSymbol(List<int> lineNumbers)
        {
            // Get files from the file path
            string[] files = Directory.GetFiles(regexFilesPath);

            DateTime today = DateTime.Now;

            string newFolder = Path.GetDirectoryName(regexFilesPath) + @"\" + today.ToString("yyyy'-'MM'-'dd' T'HH'.'mm'.'ss");
            Directory.CreateDirectory(newFolder);

            foreach (string file in files)
            {
                if (Path.GetExtension(file) == ".properties")
                {
                    SortedDictionary<int, string> toInsertComment = ReadFromFile(file);
                    SortedDictionary<int, string> toBuild = new SortedDictionary<int, string>();                    

                    foreach(int lineNumber in lineNumbers)
                    {

                        toInsertComment[lineNumber] = "#" + toInsertComment[lineNumber];
                    }
                    

                    // New file to build
                    string newFile = newFolder + @"\" + Path.GetFileName(file);

                    // Rebuild the properties file
                    using (StreamWriter sw = new StreamWriter(newFile, false, Encoding.UTF8))
                    {
                        foreach (KeyValuePair<int, string> pair in toInsertComment)
                        {
                            sw.WriteLine(pair.Value);
                        }

                        sw.Close();
                    }
                }
            }
        }

        private static void PropagateChanges(SortedDictionary<int, string> pattern, string regexFilesPath)
        {
            // Get files from the file path
            string[] files = Directory.GetFiles(regexFilesPath);

            DateTime today = DateTime.Now;

            string newFolder = Path.GetDirectoryName(regexFilesPath) + @"\" + today.ToString("yyyy'-'MM'-'dd' T'HH'.'mm'.'ss");
            Directory.CreateDirectory(newFolder);

            foreach (string file in files)
            {
                if (Path.GetExtension(file) == ".properties")
                {
                    SortedDictionary<int, string> toOrder = ReadFromFile(file);
                    SortedDictionary<int, string> toBuild = new SortedDictionary<int, string>();
                    SortedDictionary<int, string> toBuildTemp = new SortedDictionary<int, string>();

                    foreach (KeyValuePair<int, string> pairPattern in pattern)
                    {
                        string toCompare = pairPattern.Value.Contains('=') ? pairPattern.Value.Substring(0, pairPattern.Value.IndexOf('=')) : pairPattern.Value;

                        bool found = false;
                        foreach (KeyValuePair<int, string> pairToOrder in toOrder)
                        {
                            string compareAgainst = pairToOrder.Value.Contains('=') ? pairToOrder.Value.Substring(0, pairToOrder.Value.IndexOf('=')) : pairToOrder.Value;

                            if (toCompare == compareAgainst)
                            {
                                toBuild.Add(pairPattern.Key, pairToOrder.Value);
                                found = true;
                                break;
                            }

                        }

                        if (!found)
                        {
                            toBuild.Add(pairPattern.Key, pairPattern.Value);
                        }
                    }

                    // If alterMeasureNumbering == Yes, then re-number all measurement units from top to bottom
                    if (alterMeasureNumbering)
                    {
                        int counter = 1;

                        foreach (KeyValuePair<int, string> pair in toBuild)
                        {
                            if (pair.Value.Contains("lqacheck.units.measure.regexp"))
                            {
                                int tempKey = pair.Key;
                                string tempValue = pair.Value;

                                // Define a Regex with the pattern 
                                Regex r = new Regex(@"lqacheck\.units\.measure\.regexp\d{1,4}");
                                if (r.IsMatch(pair.Value))
                                {
                                    MatchCollection matches = r.Matches(pair.Value);

                                    if (matches.Count == 1)
                                    {
                                        string matched = matches[0].Value;
                                        toBuildTemp.Add(tempKey, pair.Value.Replace(matched, "lqacheck.units.measure.regexp" + counter));
                                    }
                                }

                                counter++;
                            }
                            else
                            {
                                toBuildTemp.Add(pair.Key, pair.Value);
                            }
                        }
                    }

                    // New file to build
                    string newFile = newFolder + @"\" + Path.GetFileName(file);

                    SortedDictionary<int, string> toBuildFinal = alterMeasureNumbering ? toBuildTemp : toBuild;

                    // Rebuild the properties file
                    using (StreamWriter sw = new StreamWriter(newFile, false, Encoding.UTF8))
                    {
                        foreach (KeyValuePair<int, string> pair in toBuildFinal)
                        {
                            sw.WriteLine(pair.Value);
                        }

                        sw.Close();
                    }
                }
            }
        }

        // maliao: This is the new method to compare 2 versions when lqacheck.units.measure.regexpXX have already been re-numbered 
        static public SortedDictionary<int, string> CompareTwoFiles2(string beforeEditing, string postEditing)
        {
            // Open both files and store the strings in dictionaries
            SortedDictionary<int, string> before = ReadFromFile(beforeEditing);
            SortedDictionary<int, string> after = ReadFromFile(postEditing);
            SortedDictionary<int, string> reordered = new SortedDictionary<int, string>();

            // Add aditional items to before so the logic won't fail
            for (int i = before.Count + 1; i <= after.Count; i++)
            {
                before.Add(i, " ");
            }

            // Compare line by line and re-compile the matched one in a dictionary
            foreach (KeyValuePair<int, string> pairAfter in after)
            {
                // Check whether the same keys have the same value, if not the same, search for the matching value and return the key
                if (pairAfter.Value == before[pairAfter.Key])
                {
                    reordered.Add(pairAfter.Key, pairAfter.Value);
                }
                else
                {
                    bool found = false;
                    string pairAfterAfterEqualSign = ReturnPortionAfterEqualSign(pairAfter.Value);

                    // Search in "after" for matching value and find the new key position, but this time we only compare what comes after the "=" sign
                    foreach (KeyValuePair<int, string> pairBefore in before)
                    {
                        // Extract the portion after equal sign
                        if (pairAfterAfterEqualSign == ReturnPortionAfterEqualSign(pairBefore.Value))
                        {
                            Logger.Info("The line " + pairBefore.Key + " in the original file has been moved to the line " + pairAfter.Key);
                            reordered.Add(pairAfter.Key, pairBefore.Value);
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        Logger.Info("The line " + pairAfter.Key + " is new and cannot be found in the original file.");
                        reordered.Add(pairAfter.Key, pairAfter.Value);
                    }
                }
            }

            //foreach (KeyValuePair<int, string> pair in reordered)
            //    Logger.Info("Index: " + pair.Key + " Value: " + pair.Value);

            return reordered;
        }

        // Return the portion after equal sign
        static public string ReturnPortionAfterEqualSign(string text)
        {
            if ((!text.Contains('=')) || (text.Count(t => t == '=') > 1))
            {
                return text;
            }
            else
            {
                return text.Substring(text.IndexOf('=') + 1);
            }

        }

        static public SortedDictionary<int, string> CompareTwoFiles(string beforeEditing, string postEditing)
        {
            // Open both files and store the strings in dictionaries
            SortedDictionary<int, string> before = ReadFromFile(beforeEditing);
            SortedDictionary<int, string> after = ReadFromFile(postEditing);
            SortedDictionary<int, string> reordered = new SortedDictionary<int, string>();

            // Compare line by line and re-compile the matched one in a dictionary
            foreach (KeyValuePair<int, string> pairBefore in before)
            {
                // Check whether the same keys have the same value, if not the same, search for the matching value and return the key
                if (pairBefore.Value == after[pairBefore.Key])
                {
                    reordered.Add(pairBefore.Key, pairBefore.Value);
                }
                else
                {
                    // Search in "after" for matching value and find the new key position
                    foreach (KeyValuePair<int, string> pairAfter in after)
                    {
                        if (pairAfter.Value == pairBefore.Value)
                        {
                            Logger.Info("The line " + pairBefore.Key + " in the original file has been moved to the line " + pairAfter.Key);
                            reordered.Add(pairAfter.Key, pairBefore.Value);

                            break;
                        }
                    }
                }
            }

            //foreach (KeyValuePair<int, string> pair in reordered)
            //    Logger.Info("Index: " + pair.Key + " Value: " + pair.Value);

            return reordered;
        }

        static private SortedDictionary<int, string> ReadFromFile(string filePath)
        {
            SortedDictionary<int, string> lines = new SortedDictionary<int, string>();

            try
            {
                int counter = 1;
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line = "";

                    // Read line by line  
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Length > 0)
                        {
                            lines.Add(counter, line);
                            counter++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }

            return lines;
        }
    }
}

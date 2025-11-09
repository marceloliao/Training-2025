using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace Baxter_Group_files
{
    class Program
    {
        // Define basic input information 
        private const string tmList = @"C:\Users\maliao\Documents\PS Projects\42 Baxter LC setup\To_import.xlsx";
        private const string tmFolder = @"C:\Users\maliao\Documents\PS Projects\42 Baxter LC setup\Lionbridge TM Refresher 2021-03-24";
        private const string languageFoldersRoot = @"C:\Users\maliao\Documents\PS Projects\42 Baxter LC setup\Lionbridge TM Refresher 2021-03-24_by_language";
        private static bool foundFile = false;

        static void Main(string[] args)
        {
            // Populate records from the list
            List<Record> records = ReturnRecordsFromList(tmList);

            // Loop through all records and copy files to language folders
            int counter = 1;
            foreach (Record record in records)
            {
                Console.WriteLine("Row: " + counter.ToString());
                Console.WriteLine("tmFolder File" + record.tmFile);
                counter++;
                
                if (record.vendorName == "Lionbridge")
                {
                    string languageFolder = languageFoldersRoot + "\\" + record.targetLanguage;
                    string targetTmFileFullName = languageFolder + "\\" + record.tmFile;

                    if (!Directory.Exists(languageFolder))
                    {
                        Directory.CreateDirectory(languageFolder);
                    }

                    // Iterate all folders and search for TM file, usually in TMX
                    // Reset foundFile 
                    foundFile = false;
                    FindAndCopyFile(tmFolder, record.tmName, targetTmFileFullName);
                }
            }
        }

        private static void FindAndCopyFile(string currentFolder, string tmName, string targetFile)
        {
            // Only proceed if foundFile is false
            if (!foundFile)
            {
                string[] files = Directory.GetFiles(currentFolder);

                foreach (string file in files)
                {
                    // Only match if tmName is part of the directoryname
                    if ((Path.GetFileName(file).ToLower() == Path.GetFileName(targetFile).ToLower()) && (Path.GetDirectoryName(file).Contains(tmName)))
                    {
                        //if (Path.GetFileName(file) == "Baxter_Prismaflex_Client_Approved.en-us_cs-cz.tmx")
                        //    Console.Write("Copying from: " + Path.GetDirectoryName(file));

                        if (!File.Exists(targetFile))
                            File.Copy(file, targetFile);
                        else
                        {
                            if (!File.Exists(Path.GetDirectoryName(targetFile) + "\\" + Path.GetFileNameWithoutExtension(targetFile) + "_01" + Path.GetExtension(targetFile)))
                                File.Copy(file, Path.GetDirectoryName(targetFile) + "\\" + Path.GetFileNameWithoutExtension(targetFile) + "_01" + Path.GetExtension(targetFile));
                            else
                                File.Copy(file, Path.GetDirectoryName(targetFile) + "\\" + Path.GetFileNameWithoutExtension(targetFile) + "_02" + Path.GetExtension(targetFile));
                        }

                        foundFile = true;
                    }
                }

                // Only search in subfolders if not found
                if (!foundFile)
                {
                    string[] directories = Directory.GetDirectories(currentFolder);

                    foreach (string directory in directories)
                    {
                        // iterate subdirectories
                        FindAndCopyFile(directory, tmName, targetFile);
                    }
                }
            }            
        }

        private static List<Record> ReturnRecordsFromList(string filePath)
        {
            List<Record> records = new List<Record>();

            // Read from the list
            Application excel = new Application();
            Workbook book;
            Worksheet sheet;
            string columnIndex = string.Empty;

            excel.Visible = false;
            object format = 5; //Nothing value.
            object missing = System.Reflection.Missing.Value;
            object saveChange = false;

            if (!File.Exists(tmList))
            {
                book = excel.Workbooks.Add(missing);
                book.SaveAs(tmList, XlFileFormat.xlOpenXMLWorkbook, missing, missing, missing, missing, XlSaveAsAccessMode.xlNoChange, missing, missing, missing, missing, missing);
            }
            else
            {
                book = excel.Workbooks.Open(tmList, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);

                //columnIndex = "1:525";
                //columnIndex = @"" + columnIndex + "";
                sheet = (Worksheet)book.Worksheets[1];

                Range myRange = (Range)sheet.UsedRange.Rows;
                foreach (Range row in myRange.Rows)
                {
                    if (row.Value2 != null)
                    {
                        Record record = new Record();

                        record.tmName = row.Value2[1, 1];
                        record.vendorName = row.Value2[1, 2];
                        record.tmFile = row.Value2[1, 3];
                        record.sourceLanguage = row.Value2[1, 4];
                        record.targetLanguage = row.Value2[1, 5];

                        records.Add(record);
                    }
                }
            }

            book.Close();
            excel.Quit();

            return records;
        }
    }

    struct Record
    {
        public string tmName;
        public string vendorName;
        public string tmFile;
        public string sourceLanguage;
        public string targetLanguage;
    }
}

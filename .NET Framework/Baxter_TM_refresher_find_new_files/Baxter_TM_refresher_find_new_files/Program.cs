using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace Baxter_TM_refresher_find_new_files
{
    class Program
    {
        // Define basic input information 
        private const string tmList = @"C:\Users\maliao\Documents\PS Projects\42 Baxter LC setup\To_import.xlsx";
        private const string tmFolder = @"C:\Users\maliao\Documents\PS Projects\42 Baxter LC setup\Lionbridge TM Refresher 2021-03-24";
        private const string newFilesRoot = @"C:\Users\maliao\Documents\PS Projects\42 Baxter LC setup\Lionbridge TM Refresher 2021-03-24_new_files";

        static void Main(string[] args)
        {
            // Populate records from the list
            List<Record> records = ReturnRecordsFromList(tmList);

            // Loop through all subfolders and see if all files can be found amoung the records
            string[] directories = Directory.GetDirectories(tmFolder);
            foreach (string directory in directories)
            {
                // iterate subdirectories
                CheckAllFiles(directory, records);
            }
        }

        private static void CheckAllFiles(string currentFolder, List<Record> records)
        {
            string[] files = Directory.GetFiles(currentFolder);

            foreach (string file in files)
            {
                bool fileFound = false;
                foreach (Record record in records)
                {
                    if (Path.GetFileName(file).ToLower() == record.tmFile.ToLower())
                    {
                        fileFound = true;
                        break;
                    }
                }

                if (!fileFound)
                {
                    // Copy the file to a new folder if not found
                    string targetFile = newFilesRoot + "\\" + Path.GetFileName(file);
                    File.Copy(file, targetFile);
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

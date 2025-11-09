using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace Baxter_Combine_termbases_in_one_Excel
{
    class Program
    {
        // Define basic starting point
        private const string startingFolder = @"C:\Users\maliao\Documents\PS Projects\42 Baxter LC setup\Transperfect Glossaries 2021-05-31\TEST";

        static void Main(string[] args)
        {
            // Loop through all subfolders and combine all glossaries in one Excel
            string[] directories = Directory.GetDirectories(startingFolder);

            foreach (string directory in directories)
            {
                // Iterate subdirectories
                CombineAllGlossaries(directory);
            }
        }

        private static void CombineAllGlossaries(string currentFolder)
        {
            // Declare one file per language
            Termbase tb = new Termbase();

            string[] files = Directory.GetFiles(currentFolder);

            foreach (string file in files)
            {
                // We know all files are in Excel format so no further validation needed
                List<Record> terms = ReturnRecordsFromList(file);
                string product = ReturnProductName(Path.GetFileNameWithoutExtension(file));

                foreach (Record term in terms)
                {
                    tb.AddToTermbase(term.sourceText.Trim(), term.targetText.Trim(), product.Trim());
                }
            }

            // Write to a XLSX file in each language folder
            string ExcelFile = currentFolder + "\\combined_termbase.xlsx";

            // Declare an Excel object
            Excel combined = new Excel(ExcelFile);

            // Write the header
            combined.WriteColumn(tb.sourceTerms[0], 1, 1);
            combined.WriteColumn("Product", 2, 1);
            combined.WriteColumn(tb.targetTerms[0], 3, 1);
            combined.WriteColumn("Product", 4, 1);

            for (int i = 1; i < tb.sourceTerms.Count; i++)
            {
                combined.WriteColumn(tb.sourceTerms[i], 1, i + 1);
                combined.WriteColumn(tb.sourceTermProducts[i], 2, i + 1);
                combined.WriteColumn(tb.targetTerms[i], 3, i + 1);
                combined.WriteColumn(tb.targetTermProducts[i], 4, i + 1);
            }

            combined.QuitWithSaving();
        }

        private static string ReturnProductName(string inputText)
        {
            List<string> split = inputText.Split('_').ToList();
            return split[1];
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

            book = excel.Workbooks.Open(filePath, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);

            //columnIndex = "1:525";
            //columnIndex = @"" + columnIndex + "";
            sheet = (Worksheet)book.Worksheets[1];

            Range myRange = (Range)sheet.UsedRange.Rows;
            foreach (Range row in myRange.Rows)
            {
                if (row.Value2 != null)
                {
                    Record record = new Record();

                    record.sourceText = (row.Value2[1, 1]).ToString();
                    record.targetText = row.Value2[1, 2].ToString();

                    records.Add(record);
                }
            }

            book.Close();
            excel.Quit();

            return records;
        }
    }

    struct Record
    {
        public string sourceText;
        public string targetText;
    }

    internal class Termbase
    {
        public List<string> sourceTerms { get; set; }
        public List<string> sourceTermProducts { get; set; }
        public List<string> targetTerms { get; set; }
        public List<string> targetTermProducts { get; set; }

        public Termbase()
        {
            this.sourceTerms = new List<string>();
            this.sourceTermProducts = new List<string>();
            this.targetTerms = new List<string>();
            this.targetTermProducts = new List<string>();
        }

        public void AddToTermbase(string sourceText, string targetText, string product)
        {
            // Alway add to the termbase, even repeated
            //sourceTerms.Add(sourceText);
            //sourceTermProducts.Add(product);
            //targetTerms.Add(targetText);
            //targetTermProducts.Add(product);

            if (!sourceTerms.Contains(sourceText))
            {
                sourceTerms.Add(sourceText);
                sourceTermProducts.Add(product);
                targetTerms.Add(targetText);
                targetTermProducts.Add(product);
            }
            else
            {
                // Identify where sourceText is found
                for (int i = 0; i < sourceTerms.Count; i++)
                {
                    if (sourceTerms[i].Trim() == sourceText)
                    {
                        // If target text is different, add as a new term
                        if (targetTerms[i].Trim() != targetText)
                        {
                            sourceTerms.Add(sourceText);
                            sourceTermProducts.Add(product);
                            targetTerms.Add(targetText);
                            targetTermProducts.Add(product);
                        }
                        else // Both source and target are the same, no need to add a new term, only add Product to both if it hasn't been added
                        {
                            if (!CheckWhetherInCollection(sourceTermProducts[i], product))
                                sourceTermProducts[i] = sourceTermProducts[i] + " • " + product;

                            if (!CheckWhetherInCollection(targetTermProducts[i], product))
                                targetTermProducts[i] = targetTermProducts[i] + " • " + product;

                        }
                    }
                }
            }
        }

        // This method detects whether a string is already in a collection of strings where all strings are concactenated
        // For example: One | Two | Three, or One \ Two \ Three 
        public bool CheckWhetherInCollection(string target, string newTarget)
        {
            bool foundMatch = false;

            string[] splits = target.Split('•');

            foreach (string split in splits)
            {
                if (split.Trim() == newTarget)
                    foundMatch = true;
            }

            return foundMatch;
        }
    }
}


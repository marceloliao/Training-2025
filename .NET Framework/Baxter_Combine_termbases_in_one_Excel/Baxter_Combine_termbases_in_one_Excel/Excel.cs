using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace Baxter_Combine_termbases_in_one_Excel
{
    class Excel
    {
        // Fields
        private Application oExcel = new Application();
        private Workbook oBook;
        private Worksheet oSheet;

        // Constructor
        public Excel()
        {
        }

        // Constructor
        public Excel(string filePath)
        {
            oExcel.Visible = false;
            object format = 5; //Nothing value.
            object missing = System.Reflection.Missing.Value;

            if (!File.Exists(filePath))
            {
                oBook = oExcel.Workbooks.Add(missing);
                oBook.SaveAs(filePath, XlFileFormat.xlOpenXMLWorkbook, missing, missing, missing, missing, XlSaveAsAccessMode.xlNoChange, missing, missing, missing, missing, missing);
            }
            else
            {
                oBook = oExcel.Workbooks.Open(filePath, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            }
        }

        // Constructor
        public Excel(string filePath, bool visible)
        {
            oExcel.Visible = visible;
            object format = 5; //Nothing value.
            object missing = System.Reflection.Missing.Value;

            if (!File.Exists(filePath))
            {
                oBook = oExcel.Workbooks.Add(missing);
                oBook.SaveAs(filePath, XlFileFormat.xlOpenXMLWorkbook, missing, missing, missing, missing, XlSaveAsAccessMode.xlNoChange, missing, missing, missing, missing, missing);
            }
            else
            {
                oBook = oExcel.Workbooks.Open(filePath, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            }
        }

        public Excel(string filePath, bool visible, bool alreadyOpen)
        {
            oExcel.Visible = visible;
            object format = 5; //Nothing value.
            object missing = System.Reflection.Missing.Value;

            if (!File.Exists(filePath))
            {
                oBook = oExcel.Workbooks.Add(missing);
                oBook.SaveAs(filePath, XlFileFormat.xlOpenXMLWorkbook, missing, missing, missing, missing, XlSaveAsAccessMode.xlNoChange, missing, missing, missing, missing, missing);
            }
            else
            {
                if (!alreadyOpen)
                    oBook = oExcel.Workbooks.Open(filePath, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            }
        }

        public List<string> ReturnColumn(string columnIndex)
        {
            List<string> oColumn = new List<string>();
            object missing = System.Reflection.Missing.Value;
            object saveChange = false;

            columnIndex = columnIndex + ":" + columnIndex;
            columnIndex = @"" + columnIndex + "";
            oSheet = (Worksheet)this.oBook.Worksheets[1];
            Range myRange = (Range)oSheet.UsedRange.Columns[columnIndex, Type.Missing];
            foreach (Range oCell in myRange.Cells)
            {
                if (oCell.Value2 != null)
                {
                    oColumn.Add(oCell.Value2.ToString());
                }
            }

            return oColumn;
        }

        public void WriteColumn(string content, object columnIndex, object rowIndex)
        {
            object missing = System.Reflection.Missing.Value;
            object saveChange = false;
            oSheet = (Worksheet)this.oBook.Worksheets[1];
            Range myRange = (Range)oSheet.Cells[rowIndex, columnIndex];
            myRange.NumberFormat = "@";
            myRange.Value2 = content;
        }

        public void WriteColumn(string content, object columnIndex, object rowIndex, bool asHyperlink)
        {
            if (!asHyperlink)
            {
                object missing = System.Reflection.Missing.Value;
                object saveChange = false;
                oSheet = (Worksheet)this.oBook.Worksheets[1];
                Range myRange = (Range)oSheet.Cells[rowIndex, columnIndex];
                myRange.NumberFormat = "@";
                myRange.Value2 = content;
            }
            else
            {
                object missing = System.Reflection.Missing.Value;
                object saveChange = false;
                oSheet = (Worksheet)this.oBook.Worksheets[1];
                Range myRange = (Range)oSheet.Cells[rowIndex, columnIndex];
                myRange.NumberFormat = "@";
                myRange.Value2 = content;

                // Add hyperlinks from "sheet1 A1" to "sheet2 A1"
                oSheet.Hyperlinks.Add(oSheet.Cells[rowIndex, columnIndex], content, Type.Missing, Type.Missing, Type.Missing);
            }
        }

        public void WriteColumn(string content, object columnIndex, object rowIndex, int width)
        {
            object missing = System.Reflection.Missing.Value;
            object saveChange = false;
            oSheet = (Worksheet)this.oBook.Worksheets[1];
            Range myRange = (Range)oSheet.Cells[rowIndex, columnIndex];
            myRange.NumberFormat = "@";
            myRange.ColumnWidth = width;
            myRange.Value2 = content;
        }

        public void WriteColumn(string content, object columnIndex, object rowIndex, int width, string alignment, bool ifBold)
        {
            object missing = System.Reflection.Missing.Value;
            object saveChange = false;
            oSheet = (Worksheet)this.oBook.Worksheets[1];
            Range myRange = (Range)oSheet.Cells[rowIndex, columnIndex];
            myRange.NumberFormat = "@";
            myRange.ColumnWidth = width;
            myRange.Value2 = content;
            myRange.Font.Bold = ifBold;
            switch (alignment)
            {
                case "center":
                    myRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    break;
                case "left":
                    myRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    break;
                case "right":
                    myRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    break;
                default:
                    myRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                    break;
            }
        }

        public void SaveAs(object filePath)
        {
            object missing = System.Reflection.Missing.Value;

            if (File.Exists((string)filePath))
            {
                File.Delete((string)filePath);
            }

            //oBook.SaveAs(targetFile, Excel.XlFileFormat.xlWorkbookNormal, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
            if (((string)filePath).Contains(".xlsx"))
            {
                oBook.SaveAs(filePath, XlFileFormat.xlOpenXMLWorkbook, missing, missing, missing, missing, XlSaveAsAccessMode.xlNoChange, missing, missing, missing, missing, missing);
            }
            else
            {
                oBook.SaveAs(filePath, XlFileFormat.xlWorkbookNormal, missing, missing, missing, missing, XlSaveAsAccessMode.xlNoChange, missing, missing, missing, missing, missing);
            }
            object saveChange = true;
            oBook.Close(saveChange, missing, missing);
            oExcel.Quit();

        }

        public void QuitWithoutSaving()
        {
            object missing = System.Reflection.Missing.Value;
            object saveChange = false;
            oBook.Close(saveChange, missing, missing);
            oExcel.Quit();
        }

        public void QuitWithSaving()
        {
            object missing = System.Reflection.Missing.Value;
            object saveChange = true;
            oBook.Close(saveChange, missing, missing);
            oExcel.Quit();
        }
    }
}

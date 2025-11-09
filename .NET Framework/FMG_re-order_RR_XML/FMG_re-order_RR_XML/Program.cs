using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Text.RegularExpressions;
using System.Web;

namespace FMG_re_order_RR_XML
{
    class Program
    {
        private const string ROOT = @"C:\Users\maliao\Documents\PS Projects\40 FM Global - TMS Upgrade\RR-Reorder";
        private const string OUTPUT = @"C:\Users\maliao\Documents\PS Projects\40 FM Global - TMS Upgrade\RR-Reorder\Re-ordered\";
        private const string STYLESHEET = "rr_xml_sortorder.xsl";

        static private string _xml = @"C:\Users\maliao\Documents\PS Projects\40 FM Global - TMS Upgrade\RR-Reorder\Test Files\0001343180-9_1093768_2351516_EN_FRCA.xml";

        static void Main(string[] args)
        {
            // First, find the stylesheet in the root directory
            string xsl = FindStyleSheet(ROOT, STYLESHEET);

            // Run XML transform
            XmlTransform(_xml, xsl);

        }

        static void XmlTransform(string filePath, string xslFilePath)
        {
            string fileNew = OUTPUT + Path.GetFileName(filePath);
            string xslTrans = xslFilePath;

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(filePath);

            // Save the document to a file and auto-indent the output.
            xDoc.Save(fileNew);

            XslCompiledTransform myXslTransform = new XslCompiledTransform();

            // Load Stylesheet from TMS stage arguments
            myXslTransform.Load(xslTrans); //arg[2] is FM Global resource file XSL: Re-order Risk Report XML

            // Execute the transform and output the results to a file
            myXslTransform.Transform(fileNew, filePath); //arg[0] TMS source file path

            string zFileNameL = fileNew;

            FileInfo TheFileInfo = new FileInfo(zFileNameL);
            if (TheFileInfo.Exists)
            {
                File.Delete(zFileNameL);
            }

            string fileName = filePath; //TMS Arguments

            //search for trailing white space on text segments and remove to elimate ITDs going to recovery.
            xDoc.Load(filePath);
            StreamReader sr2 = new StreamReader(fileName);
            string content2 = sr2.ReadToEnd();
            sr2.Close();

            StreamWriter sw2 = new StreamWriter(fileName, false, System.Text.Encoding.UTF8);
            content2 = Regex.Replace(content2, " +?]]", "]]");
            sw2.WriteLine(content2);
            sw2.Close();

            xDoc.Load(filePath);

            //Encode all TEXT node extended Latin Characters to decimal
            foreach (XmlNode node in xDoc.SelectNodes("//TEXT[@RICH_TEXT='Yes']"))
            {
                if (node.InnerText.Length != 0)
                {
                    //Create a CData section
                    XmlCDataSection CData;
                    CData = xDoc.CreateCDataSection("<p>" + node.InnerText + "</p>");

                    //Replace the selected node with new one that includes CData.
                    node.ReplaceChild(CData, node.FirstChild);

                    //Decode HTML entities
                    //node.InnerText = HttpUtility.HtmlDecode(node.InnerText);
                }
            }

            //Encode all TEXT node extended Latin Characters to decimal
            foreach (XmlNode node in xDoc.SelectNodes("//TEXT[@RICH_TEXT='No']"))
            {

                if (node.InnerText.Length != 0)
                {
                    //Create a CData section
                    XmlCDataSection CData;

                    //Modified 12-10-2019 - to resolve "<" & ">" in Non-RichText fields
                    CData = xDoc.CreateCDataSection(HttpUtility.HtmlEncode(node.InnerText));

                    //Replace the selected node with new one that includes CData.
                    node.ReplaceChild(CData, node.FirstChild);
                }
            }
        }

        static string FindStyleSheet(string directory, string xslFileName)
        {
            // Process the list of files found in the directory.
            string[] files = Directory.GetFiles(directory);
            string stylesheet = "";

            foreach (string file in files)
            {
                if ((Path.GetFileNameWithoutExtension(file).ToLower() + Path.GetExtension(file).ToLower()).Contains(xslFileName))
                {
                    stylesheet = file;
                    break;
                }
            }

            return stylesheet;
        }
    }
}

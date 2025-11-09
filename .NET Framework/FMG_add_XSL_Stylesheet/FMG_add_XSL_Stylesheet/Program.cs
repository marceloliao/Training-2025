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

namespace FMG_add_XSL_Stylesheet
{
    class Program
    {
        private const string ROOT = @"C:\Users\maliao\Documents\PS Projects\40 FM Global - TMS Upgrade\Case Add XSL_Stylesheet\";
        private const string OUTPUT = @"C:\Users\maliao\Documents\PS Projects\40 FM Global - TMS Upgrade\Case Add XSL_Stylesheet\Stylesheet added\";
        private const string STYLESHEET = "https://fmglobal.sdlproducts.com/risk_report_output/RR_Preview.xsl";

        static private string _xml = @"C:\Users\maliao\Documents\PS Projects\40 FM Global - TMS Upgrade\Case Add XSL_Stylesheet\Test Files\0001337932-4_1114954_2353001_EN_FR.xml";

        static void Main(string[] args)
        {
            // First, find the stylesheet in the root directory
            string xsl = FindStyleSheet(ROOT, STYLESHEET);

            // Add a target file
            string modifiedXml = OUTPUT + Path.GetFileName(_xml);

            if (File.Exists(modifiedXml))
                File.Delete(modifiedXml);

            File.Copy(_xml, modifiedXml);

            FMGlobalModifySrcXML(modifiedXml);
        }

        static void FMGlobalModifySrcXML(string xmlFile)
        {
            string fileType = Path.GetExtension(xmlFile);
            string fileName = Path.GetFileName(xmlFile);

            if (fileType.ToLower() == ".xml")
            {
                XmlDocument xDoc = new XmlDocument();

                // Load TMS source file
                xDoc.Load(xmlFile);

                //Select the book node with the matching attribute value.
                XmlNode nodeToFind;
                XmlElement root = xDoc.DocumentElement;

                string rootName = xDoc.SelectSingleNode("/*").Name;

                if (rootName == "xmlcontent")
                {
                    // Selects all the title elements that have an attribute named lang
                    nodeToFind = root.SelectSingleNode("//filename");

                    if (nodeToFind == null)
                    {
                        // Create a procesing instruction.
                        XmlProcessingInstruction newPI;
                        String PItext = "type='text/xsl' href='elearning_XML.xsl'";
                        newPI = xDoc.CreateProcessingInstruction("xml-stylesheet", PItext);

                        // Display the target and data information.
                        Console.WriteLine("<?{0} {1}?>", newPI.Target, newPI.Data);

                        // Add the processing instruction node to the document.
                        xDoc.InsertBefore(newPI, xDoc.DocumentElement);

                        XmlElement NodeFilename = xDoc.CreateElement("filename");
                        NodeFilename.InnerText = STYLESHEET;
                        //NodeFilename.InnerText = fileName;
                        root.InsertAfter(NodeFilename, root.SelectSingleNode("//navnode"));

                        //Save TMS source file
                        xDoc.Save(xmlFile);
                        //xDoc.Save(filePath);
                    }
                    else
                    {
                        //  filename node was found
                    }
                }
                else if (rootName == "captions")
                {
                    // Selects all the title elements that have an attribute named lang
                    nodeToFind = root.SelectSingleNode("//filename");

                    if (nodeToFind == null)
                    {
                        // Create a procesing instruction.
                        XmlProcessingInstruction newPI;
                        String PItext = "type='text/xsl' href='elearning_XML.xsl'";
                        newPI = xDoc.CreateProcessingInstruction("xml-stylesheet", PItext);

                        // Display the target and data information.
                        Console.WriteLine("<?{0} {1}?>", newPI.Target, newPI.Data);

                        // Add the processing instruction node to the document.
                        xDoc.InsertBefore(newPI, xDoc.DocumentElement);

                        XmlElement NodeFilename = xDoc.CreateElement("filename");
                        NodeFilename.InnerText = STYLESHEET;
                        //NodeFilename.InnerText = fileName;
                        root.InsertAfter(NodeFilename, root.SelectSingleNode("//navnode"));

                        //Save TMS source file
                        xDoc.Save(xmlFile);
                        //xDoc.Save(filePath);
                    }
                    else
                    {
                        // filename node was found
                    }
                }
                else if (rootName.ToUpper() == "HORIZON_TRANSLATION")
                {
                    // Create a procesing instruction.
                    XmlProcessingInstruction newPI;

                    String PItext = "type='text/xsl' href='" + STYLESHEET + "'";
                    newPI = xDoc.CreateProcessingInstruction("xml-stylesheet", PItext);
                                        
                    // Add the processing instruction node to the document.
                    xDoc.InsertBefore(newPI, xDoc.DocumentElement);

                    //XmlElement NodeFilename = xDoc.CreateElement("filename");
                    //string baseURL = HttpContext.Current.Request.Url.Host;
                    //NodeFilename.InnerText = baseURL;
                    ////NodeFilename.InnerText = fileName;
                    //root.InsertAfter(NodeFilename, root.SelectSingleNode("//navnode"));
                                        
                    xDoc.Save(xmlFile);                    
                }
                else
                {
                    // Console.WriteLine("The XML root element is not <xmlcontent> or <caption>");
                }

                //if (rootName == "xmlcontent")
                //{
                //    // Selects all the title elements that have an attribute named lang
                //    nodeToFind = root.SelectSingleNode("//filename");

                //    if (nodeToFind == null)
                //    {
                //        // Create a procesing instruction.
                //        XmlProcessingInstruction newPI;
                //        String PItext = "type='text/xsl' href='elearning_XML.xsl'";
                //        newPI = xDoc.CreateProcessingInstruction("xml-stylesheet", PItext);

                //        // Display the target and data information.
                //        Console.WriteLine("<?{0} {1}?>", newPI.Target, newPI.Data);

                //        // Add the processing instruction node to the document.
                //        xDoc.InsertBefore(newPI, xDoc.DocumentElement);

                //        XmlElement NodeFilename = xDoc.CreateElement("filename");
                //        NodeFilename.InnerText = STYLESHEET;
                //        //NodeFilename.InnerText = fileName;
                //        root.InsertAfter(NodeFilename, root.SelectSingleNode("//navnode"));

                //        //Save TMS source file
                //        xDoc.Save(xmlFile);
                //        //xDoc.Save(filePath);
                //    }
                //    else
                //    {
                //        //  filename node was found
                //    }
                //}
                //else if (rootName == "captions")
                //{
                //    // Selects all the title elements that have an attribute named lang
                //    nodeToFind = root.SelectSingleNode("//filename");

                //    if (nodeToFind == null)
                //    {
                //        // Create a procesing instruction.
                //        XmlProcessingInstruction newPI;
                //        String PItext = "type='text/xsl' href='elearning_XML.xsl'";
                //        newPI = xDoc.CreateProcessingInstruction("xml-stylesheet", PItext);

                //        // Display the target and data information.
                //        Console.WriteLine("<?{0} {1}?>", newPI.Target, newPI.Data);

                //        // Add the processing instruction node to the document.
                //        xDoc.InsertBefore(newPI, xDoc.DocumentElement);

                //        XmlElement NodeFilename = xDoc.CreateElement("filename");
                //        NodeFilename.InnerText = STYLESHEET;
                //        //NodeFilename.InnerText = fileName;
                //        root.InsertAfter(NodeFilename, root.SelectSingleNode("//navnode"));

                //        //Save TMS source file
                //        xDoc.Save(xmlFile);
                //        //xDoc.Save(filePath);
                //    }
                //    else
                //    {
                //        // filename node was found
                //    }
                //}
                //else
                //{
                //    // Console.WriteLine("The XML root element is not <xmlcontent> or <caption>");
                //}
            }
            else
            {
                // Console.WriteLine("Return Value = 0");
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

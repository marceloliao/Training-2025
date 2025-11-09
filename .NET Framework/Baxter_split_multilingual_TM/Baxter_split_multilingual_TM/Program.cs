using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace Baxter_split_multilingual_TM
{
    class Program
    {
        public const string tmFile = @"C:\Users\maliao\Documents\PS Projects\124 Ford TE Onboarding\TM to import\GSLTS\GSLTS.tmx";
        public const string export = @"C:\Users\maliao\Documents\PS Projects\124 Ford TE Onboarding\TM to import\GSLTS\all_language_pairs.txt";
        public const string splitFile = @"C:\Users\maliao\Documents\PS Projects\124 Ford TE Onboarding\TM to import\GSLTS\GSLTS_en-us_.tmx";
        public const string header = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<tmx version=\"1.4\">\r\n  <header creationtool=\"SDL Language Platform\" creationtoolversion=\"8.0\"\r\n    datatype=\"xml\" segtype=\"sentence\" adminlang=\"en-US\" srclang=\"en-us\"\r\n    o-tmf=\"SDL TM8 Format\" creationdate=\"20250411T214800Z\" creationid=\"Marcelo Liao\">\r\n    <prop type=\"x-Recognizers\">RecognizeDates, RecognizeTimes, RecognizeNumbers, RecognizeAcronyms, RecognizeMeasurements</prop>\r\n    <prop type=\"x-idiom-db-description\">null</prop>\r\n    <prop type=\"x-TMName\">GSLTS</prop>\r\n    <prop type=\"x-idiom-o-tmf\">Idiom TM v11.0.0</prop>\r\n  </header>\r\n  <body>";
        public const string footer = "</body>\r\n</tmx>";
        public const string namespaceXml = "http://www.w3.org/XML/1998/namespace";

        static void Main(string[] args)
        {
            XDocument xFile = XDocument.Load(tmFile);

            XNamespace xml = namespaceXml;

            var segments = from c in xFile.Descendants()
                           where (c.Name == "tu" || c.Name == "TU")
                           select c;

            // This block was used to find all language pairs
            List<LanguagePair> langpairs = new List<LanguagePair>();
            List<string> targetLanguages = new List<string>();

            int counter = 0;



            foreach (var segment in segments)
            {
                var languages = from c in segment.Descendants()
                                where (c.Name == "tuv" || c.Name == "TUV")
                                select c;

                if (languages.Count() == 2)
                {
                    string source = languages.First().Attribute(xml + "lang").Value;
                    string target = languages.Last().Attribute(xml + "lang").Value;

                    LanguagePair newLanguage = new LanguagePair();
                    newLanguage.SourceLanguage = source;
                    newLanguage.TargetLanguage = target;

                    if (!langpairs.Contains(newLanguage))
                    {
                        langpairs.Add(new LanguagePair() { SourceLanguage = source, TargetLanguage = target });
                        targetLanguages.Add(target);
                    }
                }

                counter++;
            }

            // Write to an external TXT file
            using (StreamWriter sw = new StreamWriter(export))
            {
                foreach (LanguagePair lp in langpairs)
                {
                    sw.Write(lp.SourceLanguage + " -> " + lp.TargetLanguage + "\r\n");
                }

                sw.Close();
            }

            // This block is to split by language pair
            List<TranslationUnit> tus = new List<TranslationUnit>();
            //string[] targetLanguages = { "FR-FR", "HR-HR", "HU-HU", "IT-IT", "KO-KR", "MS-MY", "NB-NO", "NL-NL", "HE-IL", "PL-PL", "PT-BR", "PT-PT", "RO-RO", "RU-RU", "SK-SK", "SL-SI", "SR-RS", "SV-SE", "TH-TH", "TR-TR", "UK-UA", "VI-VN", "ZH-CN", "ZH-HK", "ZH-TW" };

            foreach (var segment in segments)
            {
                var languages = from c in segment.Descendants()
                                where (c.Name == "tuv" || c.Name == "TUV")
                                select c;

                if (languages.Count() == 2)
                {
                    string source = languages.First().Attribute(xml + "lang").Value;
                    string target = languages.Last().Attribute(xml + "lang").Value;

                    tus.Add(new TranslationUnit() { TU = segment, SourceLanguage = source, TargetLanguage = target });
                }
            }

            foreach (TranslationUnit tu in tus)
            {
                if (targetLanguages.Contains(tu.TargetLanguage))
                {
                    bool isNewFile = false;
                    string splitFileByLanguage = Path.GetDirectoryName(splitFile) + "\\" + Path.GetFileNameWithoutExtension(splitFile) + tu.TargetLanguage.ToLower() + ".tmx";

                    if (!File.Exists(splitFileByLanguage))
                        isNewFile = true;

                    // Append the TU to the file
                    using (StreamWriter sw = new StreamWriter(splitFileByLanguage, true, Encoding.UTF8))
                    {
                        // If the file doesn't exist, insert the header
                        if (isNewFile)
                        {
                            sw.WriteLine(header);
                            isNewFile = false;
                        }

                        sw.WriteLine(tu.TU);
                        sw.Close();
                    }
                }
            }

            foreach (string language in targetLanguages)
            {
                string splitFileByLanguage = Path.GetDirectoryName(splitFile) + "\\" + Path.GetFileNameWithoutExtension(splitFile) + language.ToLower() + ".tmx";

                // Append the footer at the end of the file
                using (StreamWriter sw = new StreamWriter(splitFileByLanguage, true, Encoding.UTF8))
                {
                    sw.WriteLine(footer);

                    sw.Close();
                }
            }

        }
    }

    struct LanguagePair
    {
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
    }

    struct TranslationUnit
    {
        public XElement TU { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
    }
}

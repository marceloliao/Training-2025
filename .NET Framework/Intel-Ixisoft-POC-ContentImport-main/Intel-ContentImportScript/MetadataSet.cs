using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IntelContentImportScript
{
    public class MetadataSet
    {
        public string IxiaSoftId { get; private set; }
        public string Author { get; private set; }
        public string SystemComments { get; private set; }
        public string UserComments { get; private set; }
        public string CreatedOn { get; private set; }
        public string ModifiedOn { get; private set; }
        public string ModifiedBy { get; private set; }
        public string Version { get; private set; }

        public const string FIELD_AUTHOR = "FAUTHOR";
        public const string FIELD_SYSTEM_COMMENTS = "FIXIASOFTSYSTEMCOMMENTS";
        public const string FIELD_USER_COMMENTS = "FIXIASOFTUSERCOMMENTS";
        public const string FIELD_IXIASOFT_ID = "FIXIASOFTID";
        public const string FIELD_CREATED_ON = "FIXIASOFTCREATEDDATE";
        public const string FIELD_MODIFIED_ON = "FIXIASOFTMODIFIEDDATE";
        public const string FIELD_MODIFIED_BY = "FIXIASOFTMODIFIEDBY";
        public const string FIELD_VERSION = "VERSION";
        public const string FIELD_NOTRANSLATIONMGMT = "FNOTRANSLATIONMGMT";

        private const string XPATH_SYSTEM_COMMENTS = "/customproperties/systemComment";
        private const string XPATH_USER_COMMENTS = "/customproperties/userComment";
        private const string XPATH_IXIASOFT_ID = "/documentproperties/systemproperties/name";
        private const string XPATH_AUTHOR = "/documentproperties/systemproperties/creatorinfo/user";
        private const string XPATH_CREATED_ON = "/documentproperties/systemproperties/creatorinfo/date";
        private const string XPATH_MODIFIED_ON = "/documentproperties/systemproperties/modifierinfo/date";
        private const string XPATH_CREATED_ON_TIME = "/documentproperties/systemproperties/creatorinfo/time";
        private const string XPATH_MODIFIED_ON_TIME = "/documentproperties/systemproperties/modifierinfo/time";
        private const string XPATH_MODIFIED_BY = "/documentproperties/systemproperties/modifierinfo/user";
        private const string XPATH_VERSION = "/documentproperties/otherproperties/versionsinfo/currentdocumentversion";

        private const string DATE_FORMAT = "yyyy-MM-dd";
        private const string TIME_FORMAT = "HH:mm:ss";
        private const string DATETIME_FORMAT = "d/M/yyyy HH:mm:ss";
        
        public MetadataSet(string customPropertiesFilePath, string documentProperitesFilePath, bool parseTime = false)
        {
            Parse(customPropertiesFilePath, documentProperitesFilePath, parseTime);
        }

        private void Parse(string customPropertiesFilePath, string documentProperitesFilePath, bool parseTime)
        {
            XmlDocument customPropertiesXml = new XmlDocument();
            customPropertiesXml.Load(customPropertiesFilePath);

            XmlDocument documentPropertiesXml = new XmlDocument();
            documentPropertiesXml.Load(documentProperitesFilePath);

            IxiaSoftId = GetNodeValueAsString(documentPropertiesXml, XPATH_IXIASOFT_ID);
            Author = GetNodeValueAsString(documentPropertiesXml, XPATH_AUTHOR);
            SystemComments = GetNodeOuterXml(customPropertiesXml, XPATH_SYSTEM_COMMENTS);
            UserComments = GetNodeOuterXml(customPropertiesXml, XPATH_USER_COMMENTS);
            Version = GetNodeValueAsString(documentPropertiesXml, XPATH_VERSION);
            ModifiedBy = GetNodeValueAsString(documentPropertiesXml, XPATH_MODIFIED_BY);
            CreatedOn = parseTime ?
                GetNodeValueAsDateTimeString(documentPropertiesXml, XPATH_CREATED_ON, XPATH_CREATED_ON_TIME) :
                GetNodeValueAsDateTimeString(documentPropertiesXml, XPATH_CREATED_ON);
            ModifiedOn = parseTime ?
                GetNodeValueAsDateTimeString(documentPropertiesXml, XPATH_MODIFIED_ON, XPATH_MODIFIED_ON_TIME) :
                GetNodeValueAsDateTimeString(documentPropertiesXml, XPATH_MODIFIED_ON);
        }

        private string GetNodeOuterXml(XmlDocument doc, string xpath)
        {
            string values = string.Empty;

            XmlNodeList list = doc.SelectNodes(xpath);

            if(list != null && list.Count > 0)
            {
                values = string.Join("", list.Cast<XmlNode>().Select(x => x.OuterXml));
            }     

            return values;
        }

        private string GetNodeValueAsString(XmlDocument doc, string xpath)
        {
            string values = string.Empty;

            XmlNode node = doc.SelectSingleNode(xpath);

            if (node != null && !string.IsNullOrEmpty(node.InnerText))
            {
                values = node.InnerText;
            }

            return values;
        }

        private string GetNodeValueAsDateTimeString(XmlDocument doc, string dateXpath, string timeXpath = null)
        {
            string dateString = GetNodeValueAsString(doc, dateXpath);
            string timeString = string.IsNullOrEmpty(timeXpath) ? "00:00:00" : GetNodeValueAsString(doc, timeXpath);
            string datetimeString = $"{dateString} {timeString}";
            DateTime dateTime = DateTime.ParseExact(datetimeString, $"{DATE_FORMAT} {TIME_FORMAT}", CultureInfo.InvariantCulture);
            return dateTime.ToString(DATETIME_FORMAT);
        }
    }
}

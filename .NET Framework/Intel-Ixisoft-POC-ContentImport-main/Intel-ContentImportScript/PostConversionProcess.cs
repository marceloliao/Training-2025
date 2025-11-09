using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IntelContentImportScript
{
    public class PostConversionProcess
    {
        private ILog Logger;

        private const string ISVALUETYPE_ATTR_NAME = "ishvaluetype";
        private const string NAME_ATTR_NAME = "name";
        private const string LEVEL_ATTR_NAME = "level";
        private const string XMLSPACE_ATTR_NAME = "xml:space";
        private const string ISHFIELD_NODE_NAME = "ishfield";
        private const string VERSION_LEVEL_ATTR_VALUE = "version";
        private const string LANGUAGE_LEVEL_ATTR_VALUE = "lng";
        private const string LOGICAL_LEVEL_ATTR_VALUE = "logical";
        private const string VALUE_TYPE_ATTR_VALUE = "value";
        private const string PRESERVE_XMLSPACE_ATTR_VALUE = "preserve";

        public PostConversionProcess(ILog logger)
        {
            this.Logger = logger;
        }

        public void PostProcess (string outFolder,
            string workFolder,
            string filemapFilename,
            string dbFilename,
            string tridionServerUrl,
            bool parseTime,
            string ishFilePattern,
            string customPropertiesExtension,
            string documentPropertiesExtension,
            string noTranslationManagement)
        {
            Logger.Info($"Start PostProcess ([{outFolder}], [{filemapFilename}])");

            //Initializing the database
            dbFilename = Path.GetFullPath(dbFilename);
            Logger.Info($"Initializing database [{dbFilename}]");
            Util.InitializeDatabase(dbFilename);
            Logger.Info($"Database initialized successfully.");

            //Updating GUIDs with old GUIDs from the database
            UpdateGuids(outFolder, filemapFilename, dbFilename, tridionServerUrl);

            //Updating the database with new map
            AddGuids(outFolder, filemapFilename, dbFilename, tridionServerUrl);

            //Updating metadata info
            UpdateMetadata(workFolder, outFolder, parseTime, ishFilePattern, customPropertiesExtension, documentPropertiesExtension, noTranslationManagement);

            Logger.Info($"End PostProcess ([{outFolder}], [{filemapFilename}])");
        }

        private void UpdateGuids(string outFolder,
            string filemapFilename,
            string dbFilename,
            string tridionServerUrl)
        {
            Logger.Info($"Start UpdateGuids ([{outFolder}], [{filemapFilename}], [{dbFilename}], [{tridionServerUrl}]).");
            string filemapFilePath = Path.Combine(outFolder, filemapFilename);
            XmlDocument document = new XmlDocument();
            document.Load(filemapFilePath);
            XmlNodeList nodeList = document.SelectNodes("//file");

            if (nodeList != null && nodeList.Count > 0)
            {
                foreach (XmlNode node in nodeList)
                {
                    string guid = node.Attributes["id"].Value;
                    string filePath = node.Attributes["filepath"].Value;
                    string filename = Path.GetFileName(filePath);
                    string oldGuid = Util.RetrieveOldGuid(dbFilename, tridionServerUrl, filename);

                    if (!string.IsNullOrEmpty(oldGuid))
                    {
                        //Update the GUID in Filemap
                        int count = Util.UpdateFileContent(filemapFilePath, guid, oldGuid);
                        Logger.Info($"Guid [{oldGuid}] found in database for file [{filename}]. " +
                            $"Made {count} replacement(s) in filemap [{filemapFilePath}");
                        
                        //Update the GUID in 3ish file
                        string ishFile = $"{filePath}.3sish";
                        count = Util.UpdateFileContent(ishFile, guid, oldGuid);
                        Logger.Info($"Guid [{oldGuid}] found in database for file [{filename}]. " +
                            $"Made {count} replacement(s) in 3sish file [{ishFile}");
                        count = Util.UpdateFileContent(filemapFilename, guid, oldGuid);
                        Logger.Info($"Guid [{oldGuid}] found in database for file [{filename}]. " +
                            $"Made {count} replacement(s) in 3sish file [{filemapFilename}");
                    }
                }
            }

            Logger.Info($"End UpdateGuids ([{outFolder}], [{filemapFilename}], [{dbFilename}], [{tridionServerUrl}]).");
        }

        private void AddGuids(string outFolder,
            string filemapFilename,
            string dbFilename,
            string tridionServerUrl)
        {
            Logger.Info($"Start AddGuids ([{outFolder}], [{filemapFilename}], [{dbFilename}], [{tridionServerUrl}]).");
            string filemapFilePath = Path.Combine(outFolder, filemapFilename);
            XmlDocument document = new XmlDocument();
            document.Load(filemapFilePath);
            XmlNodeList nodeList = document.SelectNodes("//file");

            if(nodeList != null && nodeList.Count > 0)
            {
                foreach(XmlNode node in nodeList)
                {
                    string guid = node.Attributes["id"].Value;
                    string filePath = node.Attributes["filepath"].Value;
                    Logger.Info($"Adding GUID [{guid}] - Filepath [{filePath}]");
                    Util.AddNewGuid(dbFilename, tridionServerUrl, guid, Path.GetFileName(filePath));
                }
            }

            Logger.Info($"End AddGuids ([{outFolder}], [{filemapFilename}], [{dbFilename}], [{tridionServerUrl}]).");
        }

        private void UpdateMetadataForAFile(string ishFilePath, 
            string customPropertiesFilePath, 
            string documentPropertiesFilePath, 
            bool parseTime,
            string noTranslationManagement)
        {
            try
            {
                Logger.Info($"Start UpdateMetadataForAFile([{ishFilePath}], " +
                    $"[{customPropertiesFilePath}], [{documentPropertiesFilePath}], [{parseTime}])");

                MetadataSet metadataSet = new MetadataSet(customPropertiesFilePath, documentPropertiesFilePath, parseTime);

                XmlDocument doc = new XmlDocument();
                doc.Load(ishFilePath);

                XmlNode ishFieldsNode = doc.SelectSingleNode("//ishfields");

                XmlNode authorNode = CreateNewIshfieldNode(doc, MetadataSet.FIELD_AUTHOR, LANGUAGE_LEVEL_ATTR_VALUE, null, PRESERVE_XMLSPACE_ATTR_VALUE, metadataSet.Author);
                ishFieldsNode.AppendChild(authorNode);

                XmlNode versionNode = CreateNewIshfieldNode(doc, MetadataSet.FIELD_VERSION, VERSION_LEVEL_ATTR_VALUE, null, PRESERVE_XMLSPACE_ATTR_VALUE, metadataSet.Version);
                ishFieldsNode.AppendChild(versionNode);

                XmlNode noTMGNode = CreateNewIshfieldNode(doc, MetadataSet.FIELD_NOTRANSLATIONMGMT, LOGICAL_LEVEL_ATTR_VALUE, null, PRESERVE_XMLSPACE_ATTR_VALUE, noTranslationManagement);
                ishFieldsNode.AppendChild(noTMGNode);

                XmlNode systemCommentsNode = CreateNewIshfieldNode(doc, MetadataSet.FIELD_SYSTEM_COMMENTS, VERSION_LEVEL_ATTR_VALUE, VALUE_TYPE_ATTR_VALUE, PRESERVE_XMLSPACE_ATTR_VALUE, metadataSet.SystemComments);
                ishFieldsNode.AppendChild(systemCommentsNode);

                XmlNode userCommentsNode = CreateNewIshfieldNode(doc, MetadataSet.FIELD_USER_COMMENTS, VERSION_LEVEL_ATTR_VALUE, VALUE_TYPE_ATTR_VALUE, PRESERVE_XMLSPACE_ATTR_VALUE, metadataSet.UserComments);
                ishFieldsNode.AppendChild(userCommentsNode);

                XmlNode createdOnNode = CreateNewIshfieldNode(doc, MetadataSet.FIELD_CREATED_ON, VERSION_LEVEL_ATTR_VALUE, VALUE_TYPE_ATTR_VALUE, PRESERVE_XMLSPACE_ATTR_VALUE, metadataSet.CreatedOn);
                ishFieldsNode.AppendChild(createdOnNode);

                XmlNode modifiedOnNode = CreateNewIshfieldNode(doc, MetadataSet.FIELD_MODIFIED_ON, VERSION_LEVEL_ATTR_VALUE, VALUE_TYPE_ATTR_VALUE, PRESERVE_XMLSPACE_ATTR_VALUE, metadataSet.ModifiedOn);
                ishFieldsNode.AppendChild(modifiedOnNode);

                XmlNode modifiedByNode = CreateNewIshfieldNode(doc, MetadataSet.FIELD_MODIFIED_BY, VERSION_LEVEL_ATTR_VALUE, VALUE_TYPE_ATTR_VALUE, PRESERVE_XMLSPACE_ATTR_VALUE, metadataSet.ModifiedBy);
                ishFieldsNode.AppendChild(modifiedByNode);

                XmlNode idNode = CreateNewIshfieldNode(doc, MetadataSet.FIELD_IXIASOFT_ID, VERSION_LEVEL_ATTR_VALUE, VALUE_TYPE_ATTR_VALUE, PRESERVE_XMLSPACE_ATTR_VALUE, metadataSet.IxiaSoftId);
                ishFieldsNode.AppendChild(idNode);

                doc.Save(ishFilePath);

                Logger.Info($"End UpdateMetadataForAFile([{ishFilePath}], " +
                    $"[{customPropertiesFilePath}], [{documentPropertiesFilePath}], [{parseTime}])");
            }
            catch(Exception ex)
            {
                string message = $"Failed to update metadata in UpdateMetadataForAFile([{ishFilePath}], " +
                    $"[{customPropertiesFilePath}], [{documentPropertiesFilePath}], [{parseTime}])";
                throw new Exception(message, ex);
            }
        }

        private XmlNode CreateNewIshfieldNode(XmlDocument doc, 
            string nameValue, 
            string levelValue,
            string typeValue, 
            string xmlSpaceValue, 
            string value)
        {
            XmlNode existingNode = doc.SelectSingleNode($"//ishfield[@name='{nameValue}']");

            if(existingNode != null)
            {
                existingNode.ParentNode.RemoveChild(existingNode);
            }

            XmlNode node = doc.CreateNode(XmlNodeType.Element, ISHFIELD_NODE_NAME, null);

            XmlAttribute nameAttribute = doc.CreateAttribute(NAME_ATTR_NAME);
            nameAttribute.Value = nameValue;
            node.Attributes.Append(nameAttribute);

            XmlAttribute levelAttribute = doc.CreateAttribute(LEVEL_ATTR_NAME);
            levelAttribute.Value = levelValue;
            node.Attributes.Append(levelAttribute);

            if (!string.IsNullOrEmpty(typeValue))
            {
                XmlAttribute typeAttribute = doc.CreateAttribute(ISVALUETYPE_ATTR_NAME);
                typeAttribute.Value = typeValue;
                node.Attributes.Append(typeAttribute);
            }

            XmlAttribute xmlSpaceAttribute = doc.CreateAttribute(XMLSPACE_ATTR_NAME);
            xmlSpaceAttribute.Value = xmlSpaceValue;
            node.Attributes.Append(xmlSpaceAttribute);

            node.InnerText = value;

            return node;
        }

        private void UpdateMetadata(string workFolder,
            string outFolder,
            bool parseTime,
            string ishFilePattern,
            string customPropertiesExtension,
            string documentPropertiesExtension,
            string noTranslationManagement)
        {
            Logger.Info($"Start UpdateMetadata([{workFolder}], [{outFolder}], [{parseTime}]");

            string[] ishFilePaths = Util.GetFiles(outFolder, ishFilePattern, SearchOption.AllDirectories);

            foreach(string ishFilePath in ishFilePaths)
            {
                string ishFilename = Path.GetFileName(ishFilePath);
                ishFilename = ishFilename.Substring(0, ishFilename.IndexOf('.'));
                string[] customProperties = Directory.GetFiles(workFolder, $"{ishFilename}{customPropertiesExtension}", SearchOption.AllDirectories);
                string[] documentProperties = Directory.GetFiles(workFolder, $"{ishFilename}{documentPropertiesExtension}", SearchOption.AllDirectories);

                if(customProperties != null && customProperties.Length > 0
                    && documentProperties != null && documentProperties.Length > 0)
                {
                    UpdateMetadataForAFile(ishFilePath, customProperties[0], documentProperties[0], parseTime, noTranslationManagement);
                }
                else
                {
                    Logger.Info($"Updating metadata was skipped because customProperties file or documentproperties not found for [{ishFilePath}.");
                }
            }

            Logger.Info($"End UpdateMetadata([{workFolder}], [{outFolder}], [{parseTime}]");
        }
    }
}

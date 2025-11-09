using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace IntelContentImportScript
{
    class PreConversionProcess
    {
        private ILog Logger;

        public PreConversionProcess(ILog logger)
        {
            this.Logger = logger;
        }

        public void PreProcess(string sourceFolder, 
            string workFolder,
            string versionFolderPrefix,
            string versionSearchPattern,
            string versionReplacePattern,
            string revisionReplacePattern,
            string inFolder,
            string ditaFilePattern,
            string zipArchivePattern,
            string folderInfoFolder,
            string folderInfoSourcePattern,
            string imageFileExtension,
            string defaultTargetFolder,
            List<KeyValuePair<string, string>> languageMaps)
        {
            //Copy files to work folder and resolving version issues
            PrepareWorkFolder(sourceFolder, workFolder, versionFolderPrefix, versionSearchPattern, versionReplacePattern);

            //Update revision filenames
            string[] revisionFiles = ProcessRevision(workFolder, revisionReplacePattern);

            //Update revision references
            UpdateRevisionReferences(revisionFiles, ditaFilePattern, revisionReplacePattern);

            //Load filemap or import item folder info
            XmlDocument[] importItemFolderInfo = GetImportItemFolderInfo(folderInfoFolder, folderInfoSourcePattern);

            string[] ditaFiles = ProcessDitaFiles(workFolder,
                inFolder,
                ditaFilePattern,
                importItemFolderInfo,
                defaultTargetFolder,
                versionFolderPrefix);

            //Copy zipfiles to infolder
            string[] zipFiles = ProcessImageZipArchive(workFolder, 
                inFolder, 
                zipArchivePattern, 
                imageFileExtension, 
                importItemFolderInfo, 
                defaultTargetFolder, 
                versionFolderPrefix);

            //Update DITA references
            UpdateDITAReferences(ditaFiles);

            //Update image references
            UpdateImageReferences(ditaFiles, zipFiles, imageFileExtension);

            //Update language code
            UpdateLanguageCode(ditaFiles, languageMaps);
        }

        public string[] ProcessDitaFiles(string workFolder,
            string inFolder,
            string ditaFilePattern,
            XmlDocument[] importItemFolderInfo,
            string defaultTargetFolder,
            string versionFolderPrefix)
        {
            //Retrieve all dita files (xml, dita, ditamap) from the source folder
            string[] ditaFiles = Util.GetFiles(workFolder, ditaFilePattern, SearchOption.AllDirectories);

            //Cleanup infolder before copying
            CleanUpFolder(inFolder);

            string[] outputFiles = new string[ditaFiles.Length];

            //Copy files to inFolder
            for (int i = 0; i < ditaFiles.Length; i++)
            {
                outputFiles[i] = CopyFile(ditaFiles[i], inFolder, importItemFolderInfo, defaultTargetFolder, versionFolderPrefix);
            }

            return outputFiles;
        }

        private string[] ProcessRevision(string workFolder, string revisionReplacePattern)
        {
            List<string> revisionFiles = new List<string>();
            string[] files = Directory.GetFiles(workFolder, "*.*", SearchOption.AllDirectories);

            foreach(string file in files)
            {
                string newfile = Regex.Replace(file, revisionReplacePattern, string.Empty);

                if(file != newfile)
                {
                    File.Move(file, newfile);
                    revisionFiles.Add(newfile);
                }
            }

            return revisionFiles.ToArray();
        }

        private void UpdateRevisionReferences(string[] revisionFiles, string ditaFilePattern, string revisionReplacePattern)
        {
            Logger.Debug($"Start UpdateRevisionReferences(revisionFiles.Count: [{revisionFiles.Length}], ditaFilePattern: [{ditaFilePattern}], revisionReplacePattern: [{revisionReplacePattern}])");

            foreach (string revisionFile in revisionFiles)
            {
                if(Regex.IsMatch(revisionFile, ditaFilePattern)) 
                { 
                    int count = Util.UpdateFileContent(revisionFile, revisionReplacePattern, string.Empty);

                    if (count > 0)
                    {
                        Logger.Info($"Done UpdateFileContent([{revisionFile}], [{revisionReplacePattern}], [{string.Empty}]). Made {count} replacement(s).");
                    }
                    
                }
            }

            Logger.Debug($"End UpdateRevisionReferences(revisionFiles.Count: [{revisionFiles.Length}], ditaFilePattern: [{ditaFilePattern}], revisionReplacePattern: [{revisionReplacePattern}])");
        }

        private void CleanUpFolder(string folder)
        {
            DirectoryInfo di = new DirectoryInfo(folder);

            foreach (FileInfo file in di.EnumerateFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.EnumerateDirectories())
            {
                dir.Delete(true);
            }
        }

        private string GetMatchGroupValue(string source, string pattern, int groupNumber)
        {
            Match m = Regex.Match(source, pattern, RegexOptions.None);
            
            if (m.Success && m.Groups.Count > groupNumber)
            {
                return m.Groups[groupNumber].Value;
            }

            return null;
        }

        public void PrepareWorkFolder(string sourceFolder, 
            string workFolder, 
            string versionFolderPrefix, 
            string versionSearchPattern,
            string versionReplacePattern
            )
        {
            //Delete contents in workfolder
            CleanUpFolder(workFolder);

            string[] files = Directory.GetFiles(sourceFolder, "*.*", SearchOption.AllDirectories);

            foreach(string file in files)
            {
                string versionText = GetMatchGroupValue(file, versionSearchPattern, 0);
                int version = string.IsNullOrEmpty(versionText) ? 0 : Convert.ToInt32(versionText);

                string relativePath = GetRelativePath(Path.GetDirectoryName(file), sourceFolder);
                string targetFolderPath = Path.Combine(workFolder, $"{versionFolderPrefix}{version}", relativePath);
                string targetFilename = Regex.Replace(Path.GetFileName(file), versionReplacePattern, string.Empty);

                if (!Directory.Exists(targetFolderPath))
                {
                    Directory.CreateDirectory(targetFolderPath);
                }

                File.Copy(file, Path.Combine(targetFolderPath, targetFilename));
            }
        }

        private string GetRelativePath(string filePath, string refPath)
        {
            filePath = Path.GetFullPath(filePath);
            refPath = Path.GetFullPath(refPath).TrimEnd('\\', '/');

            if (!filePath.StartsWith(refPath))
            {
                return string.Empty;
            }

            return filePath.Substring(refPath.Length + 1);
        }

        /// <summary>
        /// Copy a file from sourceFilePath to inFolder.
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="inFolder"></param>
        /// <param name="importItemFolderInfo"></param>
        /// <param name="defaultTargetFolder"></param>
        /// <returns></returns>
        public string CopyFile(string sourceFilePath, 
            string inFolder, 
            XmlDocument[] importItemFolderInfo, 
            string defaultTargetFolder, 
            string versionFolderPrefix)
        {
            string versionFolder = string.Empty;

            if (!string.IsNullOrEmpty(versionFolderPrefix))
            {
                versionFolder = GetMatchGroupValue(sourceFilePath, $"(?<=\\\\)({versionFolderPrefix}\\d+)(?=\\\\)", 0);
            }

            string targetFolderPath = string.IsNullOrEmpty(versionFolder) ?
                inFolder
                : Path.Combine(inFolder, versionFolder);
            string targetFilePath = Path.Combine(targetFolderPath, Path.GetFileName(sourceFilePath));

            try
            {
                string folderInfo = string.Empty;
                
                if(importItemFolderInfo != null && importItemFolderInfo.Length > 0)
                {
                    folderInfo = GetImportItemFolder(importItemFolderInfo, Path.GetFileName(sourceFilePath));
                }
                    
                if (string.IsNullOrEmpty(folderInfo))
                {
                    targetFolderPath = Path.Combine(targetFolderPath, defaultTargetFolder);
                    Logger.Info($"Target folder was not found for [{sourceFilePath}]. Default folder [{defaultTargetFolder}] will be used.");
                }
                else
                {
                    targetFolderPath = Path.Combine(targetFolderPath, folderInfo);
                    Logger.Info($"Target folder [{targetFolderPath}] was found for [{sourceFilePath}].");
                }

                targetFilePath = Path.Combine(targetFolderPath, Path.GetFileName(sourceFilePath));

                if (!Directory.Exists(targetFolderPath))
                {
                    Directory.CreateDirectory(targetFolderPath);
                }

                File.Copy(sourceFilePath, targetFilePath, true);
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to copy [{sourceFilePath}] to [{targetFilePath}].", ex);
            }

            return targetFilePath;
        }

        public XmlDocument[] GetImportItemFolderInfo(string folder, string searchPattern)
        {
            Logger.Debug($"Start GetImportItemFolderInfo([{folder}], {searchPattern})");

            try
            {
                string[] files = Util.GetFiles(folder, searchPattern, SearchOption.AllDirectories);

                Logger.Debug($"Found {(files == null ? 0 : files.Length)}");
                
                if(files != null && files.Length > 0)
                {
                    XmlDocument[] xmlDocuments = new XmlDocument[files.Length];

                    for(int i = 0; i < files.Length; i++)
                    {
                        XmlDocument xmlDocument = new XmlDocument();

                        try
                        {
                            xmlDocument.Load(files[i]);
                            xmlDocuments[i] = xmlDocument;

                            Logger.Debug($"File [{files[i]}] loaded successfully.");                            
                        }
                        catch(Exception ex)
                        {
                            Logger.Error($"Error occurred while loading [{files[i]}]", ex);
                            throw new Exception($"Error occurred while loading [{files[i]}]", ex);
                        }
                    }
                    
                    Logger.Debug($"End GetImportItemFolderInfo([{folder}], {searchPattern})");
                    return xmlDocuments;
                }       
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred in GetImportItemFolderInfo([{folder}], {searchPattern})", ex);
            }
            
            Logger.Debug($"End GetImportItemFolderInfo([{folder}], {searchPattern}) with exception.");
            return null;
        }

        public string GetImportItemFolder(XmlDocument[] xmlDocuments, string itemName)
        {
            string folder = string.Empty;

            string xPath = $"//*[@file='{itemName}']";

            foreach (XmlDocument xmlDocument in xmlDocuments)
            {
                XmlNode node = xmlDocument.SelectSingleNode(xPath);

                if(node != null && node.ChildNodes != null && node.ChildNodes.Count > 0)
                {
                    string[] folders = new string[node.ChildNodes.Count];

                    for(int i = 0; i < node.ChildNodes.Count; i++)
                    {
                        folders[i] = node.ChildNodes[i].InnerText;
                    }

                    folder = Path.Combine(folders);
                    break;
                }
            }

            Logger.Info($"Folder {(string.IsNullOrEmpty(folder) ? "not" : folder)} found for item [{itemName}] using XPath {xPath}.");

            return folder;
        }

        /// <summary>
        /// Retrieve all zip files from the sourceFolder, extract each of them to the inFolder. The targetFolder
        /// is determined by searching for the image file with the same name as the zip in the importItemFolderInfo.
        /// If the targetFolder is not found, the defaultTargetFolder is used.
        /// </summary>
        /// <param name="sourcefolder"></param>
        /// <param name="inFolder"></param>
        /// <param name="zipArchivePattern"></param>
        /// <param name="imageFileExtension"></param>
        /// <param name="importItemFolderInfo"></param>
        /// <param name="defaultTargetFolder"></param>
        /// <returns></returns>
        public string[] ProcessImageZipArchive(string sourcefolder, 
            string inFolder, 
            string zipArchivePattern, 
            string imageFileExtension,
            XmlDocument[] importItemFolderInfo, 
            string defaultTargetFolder,
            string versionFolderPrefix)
        {
            Logger.Info($"Start ProcessImageZipArchive([{sourcefolder}], [{zipArchivePattern}])");

            string[] zipFiles = Util.GetFiles(sourcefolder, zipArchivePattern, SearchOption.AllDirectories);
            Logger.Info($"Found {(zipFiles == null ? 0 : zipFiles.Length)} zip file(s).");
            string[] outputZipFiles = new string[zipFiles.Length];

            for(int i = 0; i < zipFiles.Length; i++)
            {
                string versionFolder = string.Empty;

                if (!string.IsNullOrEmpty(versionFolderPrefix))
                {
                    versionFolder = GetMatchGroupValue(zipFiles[i], $"(?<=\\\\)({versionFolderPrefix}\\d+)(?=\\\\)", 0);
                }

                string imageFilename = $"{Path.GetFileNameWithoutExtension(zipFiles[i])}{imageFileExtension}";
                string imageFolder = GetImportItemFolder(importItemFolderInfo, imageFilename);

                imageFolder = string.IsNullOrEmpty(imageFolder) ? defaultTargetFolder : imageFolder;
                imageFolder = string.IsNullOrEmpty(versionFolder) ? imageFolder : Path.Combine(versionFolder, imageFolder);

                outputZipFiles[i] = ExtractZipEntry(zipFiles[i], Path.Combine(inFolder, imageFolder));
            }
            
            Logger.Info($"End ProcessImageZipArchive([{sourcefolder}], [{zipArchivePattern}])");

            return outputZipFiles;
        }

        private string ExtractZipEntry(string zipArchivePath, string outputFolder)
        {
            using (ZipArchive archive = ZipFile.OpenRead(zipArchivePath))
            {
                if(archive.Entries.Count >= 1)
                {
                    ZipArchiveEntry entry = archive.Entries[0];
                    string extractedFilename = $"{Path.GetFileNameWithoutExtension(zipArchivePath)}{Path.GetExtension(entry.FullName)}";
                    string extractedFilePath = Path.Combine(outputFolder, extractedFilename);

                    if (!Directory.Exists(outputFolder))
                    {
                        Directory.CreateDirectory(outputFolder);
                    }

                    entry.ExtractToFile(extractedFilePath, true);
                    Logger.Info($"Zip archive [{zipArchivePath}] entry [{entry.FullName}] was successfully extracted to [{extractedFilePath}].");
                    return extractedFilePath;
                }
                else if(archive.Entries.Count > 1)
                {
                    Logger.Info($"Zip archive [{zipArchivePath}] is contains more than one files. Only the first entry will be extracted.");
                }
                else
                {
                    Logger.Info($"Zip archive [{zipArchivePath}] is empty. Extraction was skipped.");
                }
            }

            return string.Empty;
        }

        public void UpdateDITAReferences(string[] contentFiles)
        {
            Logger.Debug($"Start UpdateDITAReferences(contentFiles.Count: {contentFiles.Length})");

            foreach (string contentFile in contentFiles)
            {
                foreach (string refFile in contentFiles)
                {
                    if(contentFile != refFile)
                    {
                        string relativePath = Util.GetRelativePath(contentFile, refFile);
                        
                        //Find path in front of the refFile name and remove it
                        string searchPattern = $"(?<=href\\s*=\\s*\").*(?={Path.GetFileName(refFile)}(\"|#))";
                        string replacePattern = string.Empty;

                        int count = Util.UpdateFileContent(contentFile, searchPattern, replacePattern);

                        if (count > 0)
                        {
                            Logger.Info($"Done UpdateFileContent([{contentFile}], [{searchPattern}], [{replacePattern}]). Made {count} replacement(s).");
                        }

                        //Find the find name and replace it with the relative path. Assuming the filename is unique in the system
                        searchPattern = $"(?<=href\\s*=\\s*\"){Path.GetFileName(refFile)}(?=(\"|#))";
                        replacePattern = $"{relativePath}";

                        count = Util.UpdateFileContent(contentFile, searchPattern, replacePattern);

                        if (count > 0)
                        {
                            Logger.Info($"Done UpdateFileContent([{contentFile}], [{searchPattern}], [{replacePattern}]). Made {count} replacement(s).");
                        }

                        //Find path in front of the refFile name and remove it
                        searchPattern = $"(?<=conref\\s*=\\s*\").*(?={Path.GetFileName(refFile)}(\"|#))";
                        replacePattern = string.Empty;

                        count = Util.UpdateFileContent(contentFile, searchPattern, replacePattern);

                        if (count > 0)
                        {
                            Logger.Info($"Done UpdateFileContent([{contentFile}], [{searchPattern}], [{replacePattern}]). Made {count} replacement(s).");
                        }

                        //Find the find name and replace it with the relative path. Assuming the filename is unique in the system
                        searchPattern = $"(?<=conref\\s*=\\s*\"){Path.GetFileName(refFile)}(?=(\"|#))";
                        replacePattern = $"{relativePath}";

                        count = Util.UpdateFileContent(contentFile, searchPattern, replacePattern);

                        if (count > 0)
                        {
                            Logger.Info($"Done UpdateFileContent([{contentFile}], [{searchPattern}], [{replacePattern}]). Made {count} replacement(s).");
                        }
                    }
                }
            }

            Logger.Debug($"End UpdateDITAReferences(contentFiles.Count: {contentFiles.Length})");
        }

        public void UpdateImageReferences(string[] contentFiles, string[] imageFiles, string imageExtension)
        {
            Logger.Debug($"Start UpdateImageReferences(contentFiles.Count: {contentFiles.Length}, imageFiles: {imageFiles.Length}, [{imageExtension}])");
            
            foreach(string contentFile in contentFiles)
            {
                foreach(string imageFile in imageFiles)
                {
                    string relativePath = Util.GetRelativePath(contentFile, imageFile);

                    string searchPattern = $"(?<=href\\s*=\\s*\"){Path.GetFileNameWithoutExtension(imageFile)}{imageExtension}(?=\")";
                    string replacePattern = $"{relativePath}";

                    int count = Util.UpdateFileContent(contentFile, searchPattern, replacePattern);

                    if(count > 0)
                    {
                        Logger.Info($"Done UpdateFileContent([{contentFile}], [{searchPattern}], [{replacePattern}]). Made {count} replacement(s).");
                    }                    
                }
            }

            Logger.Debug($"End UpdateImageReferences(contentFiles.Count: {contentFiles.Length}, imageFiles: {imageFiles.Length}, [{imageExtension}])");
        }

        public void UpdateLanguageCode(string[] contentFiles, List<KeyValuePair<string, string>> languageMaps)
        {
            Logger.Debug($"Start UpdateLanguageCode(contentFiles.Count: {contentFiles.Length}, languageMaps: {languageMaps.Count})");

            foreach (string contentFile in contentFiles)
            {
                foreach (KeyValuePair<string, string> languageMap in languageMaps)
                {
                    string searchPattern = $"(?<=xml:lang\\s*=\\s*\"){languageMap.Key}(?=\")";
                    string replacePattern = $"{languageMap.Value}";

                    int count = Util.UpdateFileContent(contentFile, searchPattern, replacePattern);

                    if(count > 0)
                    {
                        Logger.Info($"Done UpdateFileContent([{contentFile}], [{searchPattern}], [{replacePattern}]). Made {count} replacement(s).");
                    }                    
                }
            }

            Logger.Debug($"End UpdateLanguageCode(contentFiles.Count: {contentFiles.Length}, languageMaps: {languageMaps.Count})");
        }
    }
}

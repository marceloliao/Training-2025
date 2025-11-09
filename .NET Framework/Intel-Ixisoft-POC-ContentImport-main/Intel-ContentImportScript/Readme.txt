#Execute
IntelContentImportScript.exe
<add key="RunExePreorPostConversion" value="" /> in .config can be set -pre or -post.
If none is added Command prompts for -pre or -post argument.

#Execute Pre Command Sample 1:
IntelContentImportScript.exe -pre
No functionality added currently. Place holder

#Execute Post Command Sample 2:
IntelContentImportScript.exe -post
if ReUpload value in .config file is yes then GUID is taken from filemapXML and version number will be updated on all .xml/.dita and .3sish files
else
all the .3sish files will be updated in the OUT folder with meta from .properties and .customproperties.
The values for which metadata to be updated in configured in \bin\Debug\InsertXMLConfig.xml
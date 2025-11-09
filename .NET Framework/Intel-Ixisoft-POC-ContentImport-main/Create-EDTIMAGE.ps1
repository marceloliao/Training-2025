# This script creates the EDT with the specified values or sets the specified values if the EDT already exists
# 
# Create-EDTIMAGE.ps1 relies on resolved InstallTool parameters
# Create-EDTIMAGE.ps1 -webServicesBaseUrl "https://example.com/InfoShareWS/" -trisoftUserName "username" -trisoftPassword "userpassword"  
# 
# Remarks
# 1. When running on an installation linked with Windows Authentication, the trisoftUserName (and trisoftPassword) should be left empty

#
# PARAM statement must be first non-comment, non-blank line in the script
#
[CmdletBinding(SupportsShouldProcess=$true)]
Param(
	[alias("WSBaseUrl")]
	$webServicesBaseUrl = 'https://inteldocsapp2.sdlkcps.com/ISHWS/',
	[alias("UserName")][parameter(Mandatory=$true,HelpMessage="For InfoShareSTS enter the name of a Trisoft User",ParameterSetName="TrisoftUser")]
	$trisoftUserName = '',
    [alias("Password")][parameter(Mandatory=$true,HelpMessage="For InfoShareSTS enter the password of that Trisoft User",ParameterSetName="TrisoftUser")]
	$trisoftPassword = ''
)

Write-Host "`r`nSetting preferences..."
$DebugPreference   = "SilentlyContinue"   # Continue or SilentlyContinue
$VerbosePreference = "SilentlyContinue"   # Continue or SilentlyContinue
$WarningPreference = "Continue"   # Continue or SilentlyContinue or Stop
$ProgressPreference= "Continue"   # Continue or SilentlyContinue

Write-Host "Setting current directory..."

[string]$currentScriptDirectory = split-path -parent $MyInvocation.MyCommand.Definition

Write-Host "`r`nImporting Modules..."

Import-Module $currentScriptDirectory\Modules\ISHRemote.dll -DisableNameChecking
Import-Module $currentScriptDirectory\Modules\EDTUtilities.psm1 -DisableNameChecking

try
{
    # Initializing new ishSession
    $ishSession = InitializeIshSession -wsBaseUrl $webServicesBaseUrl -ishUserName $trisoftUserName -ishPassword $trisoftPassword

    # Create (or Update) EDTIMAGE
    $edtId = CreateOrUpdateEDT -ishSession $ishSession -edtId "EDTIMAGE" -name "IMAGE" -fileExtension "image" -candidateFileExtensions "image" -mimeType "application/xml"
    
    # Retrieve and log EDTMPEG 
    GetEDT $ishSession $edtId
}
catch
{
    Write-Host "`r`nException"
    Write-Host "========="
    Write-Host $_.Exception.Message
    Write-Host "========="
}
finally
{
	Write-Debug "Unloading All Modules"
	Get-Module | Remove-Module
}
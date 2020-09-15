#Requires -Version 3.0
<#
    .DESCRIPTION
        This script iterates through an array of resource group names and rbac roles (You must login to Azure via powershell before running this script)

    .PARAMETER parameterfilename
        Name and location of the JSON parameter file.
    
    .NOTES
        AUTHOR: Steve Owens
        LASTEDIT: Sep 15, 2020
        VERSION: 1.0.0.1
#>

param(
    [string] [Parameter(Mandatory=$true)] $filePath
)


function Test-IsGuid
{
    param
    (
        [Parameter(Mandatory = $true)]
        [string]$objectGuid
    )

    # Define verification regex
    [regex]$guidRegex = '(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$'

    # Check guid against regex
    if ($objectGuid -match $guidRegex)
    {
        return $objectGuid
    }
    return $null

}

function Test-FileExists
{
    param
    (
        [Parameter(Mandatory = $true)]
        [string]$filePath
    )

    if (Test-Path $filePath -PathType Leaf)
    {
        return $filePath
    }
    return $null
}

function AddGuidToProject
{

    [OutputType([string])]
    param
    (
        [Parameter(Mandatory = $true)]
        [string]$objectGuid,
        [Parameter(Mandatory = $true)]
        [string]$filePath
    )

        $doc = New-Object System.Xml.XmlDocument
        $doc.Load($filePath)
        
        $projectGuid = $doc.GetElementsByTagName("ProjectGuid")
        if( "" -eq $projectGuid )
        {
            $child = $doc.CreateElement("ProjectGuid")
            $child.InnerText = "{"+[guid]::NewGuid().ToString().ToUpper()+"}"
            $node = $doc.SelectSingleNode("//Project/PropertyGroup")
            $node.AppendChild($child)
            $doc.Save($filePath)
            return "File Updated"
        }

        return "ProjectGuid already exists!"   
}


$guid = New-Guid

if(Test-IsGuid -objectGuid $guid)
{
    if(Test-FileExists -filePath $filePath)
    {
        Write-Host(AddGuidToProject -objectGuid $guid -filePath $filePath)
    }
    else 
    {
        Write-Host("Invalid file Was Provided!")
    }
}
else 
{
    Write-Host("Invalid Guid Was Provided!")
}

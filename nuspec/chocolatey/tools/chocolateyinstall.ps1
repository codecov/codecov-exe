$ErrorActionPreference = 'Stop'

$toolsDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"

$packageArgs = @{
    Destination    = $toolsDir
    FileFullPath   = "$toolsDir/codecov-win7-x86.zip"
    FileFullPath64 = "$toolsDir/codecov-win7-x64.zip"
    PackageName    = $env:chocolateyPackageName
}

Get-ChocolateyUnzip @packageArgs

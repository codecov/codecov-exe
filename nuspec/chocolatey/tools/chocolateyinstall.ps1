$toolsDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$zipFilePath = Join-Path $toolsDir 'Codecov-win7-x64.zip'
Get-ChocolateyUnzip -FileFullPath $zipFilePath -Destination $toolsDir

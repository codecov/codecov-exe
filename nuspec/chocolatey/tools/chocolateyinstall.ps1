$toolsDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$zipFilePath = Join-Path $toolsDir 'Codecov.zip'
Get-ChocolateyUnzip -FileFullPath $zipFilePath -Destination $toolsDir

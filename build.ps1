$SCRIPT_DIR = Split-Path -Parent $MyInvocation.MyCommand.Definition
$TOOLS_DIR = "$SCRIPT_DIR/tools"
if ($IsMacOS -or $IsLinux) {
    $CAKE_EXE = "$TOOLS_DIR/dotnet-cake"
}
else {
    $CAKE_EXE = "$TOOLS_DIR/dotnet-cake.exe"
}

$DOTNET_EXE = "$(Get-Command dotnet -ea 0 | select -Expand Source)"
$INSTALL_NETCORE = $false
[string[]]$DOTNET_SDKS = ""
[string]$CAKE_VERSION = ""
foreach ($line in Get-Content "$SCRIPT_DIR/build.config" -Encoding utf8) {
    if ($line -like "CAKE_VERSION=*") {
        $CAKE_VERSION = $line.Substring($line.IndexOf('=') + 1)
    }
    elseif ($line -like "DOTNET_SDKS=*") {
        $DOTNET_SDKS = $line.Substring($line.IndexOf('=') + 1) -split ','
    }
}

if ([string]::IsNullOrWhiteSpace($CAKE_VERSION) -or !$DOTNET_SDKS) {
    "An errer occured while parsing Cake / .NET Core SDK version."
    exit 1
}

if (Test-Path "$SCRIPT_DIR/.dotnet") {
    $env:PATH = "$SCRIPT_DIR/.dotnet${PathSep}${env:PATH}"
    $env:DOTNET_ROOT = "$SCRIPT_DIR/.dotnet"
    $DOTNET_EXE = Get-ChildItem -Path "$SCRIPT_DIR/.dotnet/dotnet*" -Exclude "*.ps1" | select -First 1 -Expand FullName
}

if ([string]::IsNullOrWhiteSpace($DOTNET_EXE)) {
    $INSTALL_NETCORE = $true
}
elseif (($DOTNET_SDKS | ? { $_ -ne 'ANY' })) {
    foreach ($sdk in $DOTNET_SDKS) {
        $re = "^\s*$([regex]::Escape($sdk))\s+"
        $DOTNET_INSTALLED_VERSION = . $DOTNET_EXE --list-sdks 2>&1 | ? { $_ -match $re }
        if (!$DOTNET_INSTALLED_VERSION) {
            $INSTALL_NETCORE = $true
            break
        }
    }
}

if ($true -eq $INSTALL_NETCORE) {
    if (!(Test-Path "$SCRIPT_DIR/.dotnet")) {
        New-Item -Path "$SCRIPT_DIR/.dotnet" -ItemType Directory -Force | Out-Null
    }

    $ScriptPath = ""
    $LaunchUrl = ""
    $ScriptUrl = ""
    $PathSep = ';'

    if ($IsMacOS -or $IsLinux) {
        $ScriptPath = "$SCRIPT_DIR/.dotnet/dotnet-install.sh"
        $ScriptUrl = "https://dot.net/v1/dotnet-install.sh"
        $LaunchUrl = "$(Get-Command bash)"
        $PathSep = ":"
    }
    else {
        $ScriptPath = "$SCRIPT_DIR/.dotnet/dotnet-install.ps1"
        $ScriptUrl = "https://dot.net/v1/dotnet-install.ps1"
        $LaunchUrl = "$ScriptPath"
    }
    (New-Object System.Net.WebClient).DownloadFile($ScriptUrl, $ScriptPath)

    foreach ($DOTNET_VERSION in $DOTNET_SDKS) {
        $arguments = @()
        if ($IsMacOS -or $IsLinux) {
            $arguments = @(
                $ScriptPath
                "--install-dir"
                "$SCRIPT_DIR/.dotnet"
                "--no-path"
            )
            if ($DOTNET_VERSION -ne "ANY") {
                $arguments += @(
                    "--version"
                    "$DOTNET_VERSION"
                )
            }
        }
        else {
            $arguments = @{
                InstallDir = "$SCRIPT_DIR/.dotnet"
                NoPath     = $true
                Version    = "$DOTNET_VERSION"
            }
        }

        & $LaunchUrl @arguments
    }

    $env:PATH = "$SCRIPT_DIR/.dotnet${PathSep}${env:PATH}"
    $env:DOTNET_ROOT = "$SCRIPT_DIR/.dotnet"

    $DOTNET_EXE = Get-ChildItem -Path "$SCRIPT_DIR/.dotnet/dotnet*" -Exclude "*.ps1" | select -First 1 -Expand FullName

}
elseif (Test-Path "/opt/dotnet/sdk" -ea 0) {
    $env:DOTNET_ROOT = "/opt/dotnet/sdk"
}

$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = 1
$env:DOTNET_CLI_TELEMETRY_OPTOUT = 1
$env:DOTNET_SYSTEM_NET_HTTP_USESOCKETSHTTPHANDLER = 0

$CAKE_INSTALLED_VERSION = Get-Command dotnet-cake -ea 0 | % { & $_.Source --version }

if ($CAKE_INSTALLED_VERSION -eq $CAKE_VERSION) {
    $CAKE_EXE = Get-Command dotnet-cake | % Source
}
else {
    $CakePath = "$TOOLS_DIR/.store/cake.tool/$CAKE_VERSION"
    $CAKE_EXE = (Get-ChildItem -Path $TOOLS_DIR -Filter "dotnet-cake*" -File -ea 0 | select -First 1 -Expand FullName)

    if (!(Test-Path -Path $CakePath -PathType Container) -or !(Test-Path $CAKE_EXE -PathType Leaf)) {
        if (!([string]::IsNullOrWhiteSpace($CAKE_EXE)) -and (Test-Path $CAKE_EXE -PathType Leaf)) {
            & $DOTNET_EXE tool uninstall --tool-path $TOOLS_DIR Cake.Tool
        }

        & $DOTNET_EXE tool install --tool-path $TOOLS_DIR --version $CAKE_VERSION Cake.Tool
        if ($LASTEXITCODE -ne 0) {
            "An error occured while installing Cake."
            exit 1
        }

        $CAKE_EXE = (Get-ChildItem -Path $TOOLS_DIR -Filter "dotnet-cake*" -File | select -First 1 -Expand FullName)
    }
}

& "$CAKE_EXE" "$SCRIPT_DIR/setup.cake" --bootstrap
if ($LASTEXITCODE -eq 0) {
    & "$CAKE_EXE" "$SCRIPT_DIR/setup.cake" $args
}

exit $LASTEXITCODE

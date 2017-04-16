# Codecov global executable uploader for PowerShell and Windows Command Line.

| [https://codecov.io/](https://codecov.io/) | [@codecov](https://twitter.com/codecov) | [hello@codecov.io](mailto:hello@codecov.io) |
| ------------------------ | ------------- | --------------------- |

## Introduction

[![AppVeyor branch](https://img.shields.io/appveyor/ci/larzw/codecov-exe/master.svg)](https://ci.appveyor.com/project/larzw/codecov-exe/branch/master)
[![NuGet](https://img.shields.io/nuget/v/Codecov.svg)](https://www.nuget.org/packages/Codecov/)
[![Chocolatey](https://img.shields.io/chocolatey/v/codecov.svg)](https://chocolatey.org/packages/codecov)
[![Gitter](https://img.shields.io/gitter/room/nwjs/nw.js.svg?maxAge=2592000)](https://gitter.im/codecov/support)

1. This uploader supports Windows Command Line and PowerShell on Windows 7 (x64) and above. If you need support for OS X or Linux use the [bash global uploader](https://github.com/codecov/codecov-bash). However, since this is a .NET Core app, builds for OS X and Linux will eventually come.

2. The following Services are supported: AppVeyor, TeamCity (see section on TeamCity), and Git.

3. Many Codecov CLI options are supported. Run `.\codecov.exe --help` or see [Options.cs](https://github.com/codecov/codecov-exe/blob/master/Source/Codecov/Program/Options.cs) for more details.

4. You can download the exe from NuGet or Chocolatey. As an alternative to NuGet or Chocolatey you can download the exe as the asset *Codecov.zip* under the release. The following PowerShell (version 5) commands might be helpful

```PowerShell
(New-Object System.Net.WebClient).DownloadFile("<url>", (Join-Path $pwd "Codecov.zip")) # Download Codecov.zip from github release.
Expand-Archive .\Codecov.zip -DestinationPath . # UnZip the file.
.\Codecov\codecov.exe # Run codecov.exe with whatever commands you need.
```

## Quick Start

In PowerShell run the following commands.
```PowerShell 
choco install codecov

# Note that, the token is not needed for AppVeyor.
.\codecov.exe -f <path to coverage report> -t <Codecov upload token>
```

## TeamCity

TeamCity does not automatically make build parameters available as environment variables. You will need to add the [following environment parameters](https://github.com/codecov/support/wiki/TeamCity) to the build configuration. To do this make sure your *Branch specification* under the VCS Root is configured correctly,

<p>
  <img src="./Images/branch-spec.png" width="1000em"/>
</p>

Then set your environment variables,

<p>
  <img src="./Images/envs.png" width="1000em"/>
</p>

Note that, the above environment variables (except for `env.TEAMCITY_BUILD_URL`) can alternatively be set via the command line,

```
env.TEAMCITY_BUILD_BRANCH => --branch
env.TEAMCITY_BUILD_ID => --build
env.TEAMCITY_BUILD_COMMIT => --sha
env.TEAMCITY_BUILD_REPOSITORY => --slug
```

## Questions and Contributions

All types of contributions are welcome! Feel free to open an [issue](https://github.com/codecov/codecov-exe/issues) or **@larzw** me via [Gitter](https://gitter.im/codecov/support)

## Maintainers

To create a release (to be automated)
1. Update chocolatey nuspec version.
2. Update the version url in `chocolateyinstall.ps1`.
3. Update nuget nuspec version.
4. Update Codecov.csproj version.
5. In PowerShell run `.\build.ps1`.
6. Zip contents of `.\codecov-exe\Source\Codecov\bin\Release\netcoreapp1.1\win7-x64\publish` into a folder called `Codecov`.
7. Push changes to Github, tag the release, and add #6 as an asset.
8. Upload the artifacts to Nuget.org and/or chocolatey.org.

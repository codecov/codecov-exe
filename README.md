# Codecov global executable uploader for PowerShell and Windows Command Line.

| [https://codecov.io/](https://codecov.io/) | [@codecov](https://twitter.com/codecov) | [hello@codecov.io](mailto:hello@codecov.io) |
| ------------------------ | ------------- | --------------------- |

## Introduction

[![Build status](https://ci.appveyor.com/api/projects/status/589rt9yq1p1e43di?svg=true)](https://ci.appveyor.com/project/AdmiringWorm/codecov-exe)
[![NuGet](https://img.shields.io/nuget/v/Codecov.svg)](https://www.nuget.org/packages/Codecov/)
[![Chocolatey](https://img.shields.io/chocolatey/v/codecov.svg)](https://chocolatey.org/packages/codecov)
[![Gitter](https://img.shields.io/gitter/room/nwjs/nw.js.svg?maxAge=2592000)](https://gitter.im/codecov-exe/community)
[![codecov](https://codecov.io/gh/codecov/codecov-exe/branch/master/graph/badge.svg)](https://codecov.io/gh/codecov/codecov-exe)

1. This uploader supports Windows Command Line and PowerShell on Windows 7 (x64) and above. If you need support for OS X or Linux use the [bash global uploader](https://github.com/codecov/codecov-bash). However, since this is a .NET Core app, builds for OS X and Linux will eventually come.

2. The following Services are supported: AppVeyor, TeamCity ([see section on TeamCity](#teamcity)), and Git.

3. Many Codecov CLI options are supported. Run `.\codecov.exe --help` or see [CommandLineOptions.cs](https://github.com/codecov/codecov-exe/blob/master/Source/Codecov/Program/CommandLineOptions.cs) for more details.

4. You can download the exe from NuGet or Chocolatey. As an alternative to NuGet or Chocolatey you can download the exe as the asset *Codecov.zip* under the release. The following PowerShell (version 5) commands might be helpful

```PowerShell
(New-Object System.Net.WebClient).DownloadFile("<url>", (Join-Path $pwd "Codecov.zip")) # Download Codecov.zip from github release.
Expand-Archive .\Codecov.zip -DestinationPath . # UnZip the file.
.\Codecov\codecov.exe # Run codecov.exe with whatever commands you need.
```

## Quick Start

For a basic use case, in PowerShell run the following commands,

```PowerShell
> choco install codecov
> .\codecov.exe -f <path to coverage report> -t <Codecov upload token>
```

For an AppVeyor build, the *appveyor.yml* file would look something like

```yml
before_build:
- choco install codecov
test_script:
# Note that, a Codecov upload token is not required.
- codecov -f <path to coverage report>
```

## Cake Addin

If you use [Cake](http://cakebuild.net/) (C# Make) for your builds, you may be intrested in the [Cake.Codecov](https://github.com/cake-contrib/Cake.Codecov) addin.

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

All types of contributions are welcome! Feel free to open an [issue](https://github.com/codecov/codecov-exe/issues) or contact us through the [gitter channel](https://gitter.im/codecov-exe/community) mentioning either **[@larzw](https://gitter.im/larzw)** or **[@AdmiringWorm](https://gitter.im/admiringworm)**.

## Maintainers

To create a relase, please do the following (to be automated):
* Push the latest changes to github
* Set the following environment variables:
  * `GITHUB_USERNAME` to your username on github
  * `GITHUB_PASSWORD` to either your password, or a Personal access token
* In PowerShell run `.\build.ps1 -Target releasenotes`
* Navigate to the release page and view the drafted release.
* Make any necessary changes.
* Publish the draft release on GitHub, targeting the master branch.
* In PowerShell run `.\build.ps1`
* Upload the artifacts to github, nuget.org and chocolatey.org

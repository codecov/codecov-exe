# Codecov global executable uploader for .NET Framework/Core based builds.

| [https://codecov.io/](https://codecov.io/) | [@codecov](https://twitter.com/codecov) | [hello@codecov.io](mailto:hello@codecov.io) |
| ------------------------------------------ | --------------------------------------- | ------------------------------------------- |


## Introduction

[![Build status](https://img.shields.io/appveyor/ci/AdmiringWorm/codecov-exe?logo=appveyor)](https://ci.appveyor.com/project/AdmiringWorm/codecov-exe)
[![NuGet](https://img.shields.io/nuget/v/Codecov?logo=nuget)](https://www.nuget.org/packages/Codecov/)
[![DotNet Tool](https://img.shields.io/nuget/v/Codecov.Tool?label=dotnet%20tool&logo=nuget)](https://www.nuget.org/packages/Codecov.Tool/)
[![MSBuild Task](https://img.shields.io/nuget/v/Codecov.MSBuild?label=msbuild%20task&logo=nuget)](https://www.nuget.org/packages/Codecov.MSBuild/)
[![Chocolatey](https://img.shields.io/chocolatey/v/codecov.svg)](https://chocolatey.org/packages/codecov)
[![Gitter](https://img.shields.io/gitter/room/nwjs/nw.js.svg?maxAge=2592000&logo=gitter)](https://gitter.im/codecov-exe/community)
[![codecov](https://codecov.io/gh/codecov/codecov-exe/branch/master/graph/badge.svg)](https://codecov.io/gh/codecov/codecov-exe)
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Fcodecov%2Fcodecov-exe.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2Fcodecov%2Fcodecov-exe?ref=badge_shield)

1. This uploader supports Windows 7 (x64) and above, Linux (x64), and OS X (x64).

2. [See the following section of supported CI providers](#ci-providers).

3. Many Codecov CLI options are supported. Run `.\codecov.exe --help` or see [CommandLineOptions.cs](https://github.com/codecov/codecov-exe/blob/master/Source/Codecov/Program/CommandLineOptions.cs) for more details.

4. On windows you can download the exe from [NuGet](https://www.nuget.org/packages/Codecov) or [Chocolatey](https://chocolatey.org/packages/codecov). There is also the .Net tool called [Codecov.Tool](https://www.nuget.org/packages/Codecov.Tool) which supports all platforms. As an alternative to NuGet or Chocolatey you can download the exe as the asset _Codecov-{os}.zip_ under the release. The following PowerShell (version 5) commands might be helpful.

```PowerShell
(New-Object System.Net.WebClient).DownloadFile("<url>", (Join-Path $pwd "Codecov.zip")) # Download Codecov.zip from github release.
Expand-Archive .\Codecov.zip -DestinationPath . # UnZip the file.
.\Codecov\codecov.exe # Run codecov.exe with whatever commands you need.
```

## Quick Start

For a basic use case, in PowerShell run the following commands,

```PowerShell
> choco install codecov
> codecov.exe -f <path to coverage report> -t <Codecov upload token>
```

or using the .NET Core tool

```shell
dotnet tool install --global Codecov.Tool
codecov -f <path to coverage report> -t <Codecov upload token>
```

For an AppVeyor build, the _appveyor.yml_ file would look something like

```yml
before_build:
  - choco install codecov # Can be changed to dotnet tool install --global Codecov.Tool
test_script:
  # Note that, a Codecov upload token is not required.
  - codecov -f <path to coverage report>
```

You may also use globbing patterns for specifying files and codecov-exe will take care of resolving these paths, make sure to quote the path or depending on your shell it may be resolved before calling codecov-exe.

```shell
codecov -f "artifacts/coverage/**/*.xml" -t <Codecov upload token>
```

You can see additional globbing patterns supported by codecov-exe by heading over to: https://github.com/kthompson/glob/#supported-pattern-expressions

## MSBuild Integration

Alternatively, you can use the [Codecov.MSBuild](https://www.nuget.org/packages/Codecov.MSBuild/) NuGet pacakge which provides the `Codecov` task for use in your project files.

For example, to upload reports generated with the [coverlet.msbuild](https://github.com/coverlet-coverage/coverlet#msbuild-integration-suffers-of-possible-known-issue) task which produces the `CoverletReport` items:

```xml
<Target Name="UploadCoverageToCodecov" AfterTargets="GenerateCoverageResultAfterTest">
  <Codecov ReportFiles="@(CoverletReport)" />
</Target>
```

The only required parameter is `ReportFiles`, all other parameters are automatically guessed based on current environment variables and git repository status but you can override them if needed. See [Codecov.cs](https://github.com/codecov/codecov-exe/blob/master/Source/Codecov.MSBuild/Codecov.cs) for the complete list of supported parameters.

## Cake Addin

If you use [Cake](http://cakebuild.net/) (C# Make) for your builds, you may be intrested in the [Cake.Codecov](https://github.com/cake-contrib/Cake.Codecov) addin.

## CI Providers

The following CI providers are supported:

| Company         | Supported           | Token Required   |
| --------------- | ------------------- | ---------------- |
| AppVeyor        | Yes                 | Private only     |
| Azure Pipelines | Yes                 | Private          |
| Git             | Yes (as a fallback) | Public & Private |
| GitHub Actions  | Yes                 | Private only     |
| Jenkins         | Yes                 | Public & Private |
| TeamCity        | Yes (See below)     | Public & Private |
| Travis CI       | Yes                 | Private only     |

### TeamCity

TeamCity does not automatically make build parameters available as environment variables. You will need to add the [following environment parameters](https://github.com/codecov/support/wiki/TeamCity) to the build configuration. To do this make sure your _Branch specification_ under the VCS Root is configured correctly,

<p>
  <img src="./Images/branch-spec.png" width="1000em"/>
</p>

Then set your environment variables,

<p>
  <img src="./Images/envs.png" width="1000em"/>
</p>

Note that, the above environment variables (except for `env.TEAMCITY_BUILD_URL`) can alternatively be set via the command line,

```shell
env.TEAMCITY_BUILD_BRANCH => --branch
env.TEAMCITY_BUILD_ID => --build
env.TEAMCITY_BUILD_COMMIT => --sha
env.TEAMCITY_BUILD_REPOSITORY => --slug
```

## Questions and Contributions

All types of contributions are welcome! Feel free to open an [issue](https://github.com/codecov/codecov-exe/issues) or contact us through the [gitter channel](https://gitter.im/codecov-exe/community) mentioning either **[@larzw](https://gitter.im/larzw)** or **[@AdmiringWorm](https://gitter.im/admiringworm)**.

### Known Issues

- Specifiyng file paths with spaces is currently not possible without a workaround.
  This is expected to be fixed when a new major release of codecov-exe is released (See issue [#71](https://github.com/codecov/codecov-exe/issues/71) for possible workaround and tracking).
- If you're seeing an **HTTP 400 error when uploading reports to S3**, make sure you've updated to at least version 1.11.0.

## Maintainers

To create a relase, please do the following:

- Creating hotfix releases
  - Create a branch called `hotfix/version` locally (replace the version with the actual version to release) (Make sure that a milestone exist for this release, and all fixed/resolved issues are attached to that milestone)
  - Make any changes that needs to be included in the release while targeting the hotfix branch
  - Merge the hotfix branch into `master` using `git merge hotfix/version --no-ff`
  - Push the merged branch upstream to github
  - Wait for a new release and a tag have been created
  - Backmerge the tag into the `develop` branch
- Creating new feature releases
  - Make sure that all commits have been targeted to the `develop` branch
  - Create a new release branch using the name `release/version` locally (replace version with actual version to release) (Make sure that a milestone exist for this release, and all fixed/resolved issues are attached to that milestone)
  - Make any additional changes that are necessary to this branch
  - Merge the release branch into `master` using `git merge release/version --no-ff`
  - Push the merged branch upstream to github
  - Wait for a new release and a tag have been created
  - Backmerge the tag into the `develop` branch

**NOTE:** As soon as changes are pushed to the master branch the automated release procedure is started. This procedure will create Release notes, create a new github release, upload archived assets, upload chocolatey and nuget packages and comment on issues when the release have been completed.

## License
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Fcodecov%2Fcodecov-exe.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2Fcodecov%2Fcodecov-exe?ref=badge_large)

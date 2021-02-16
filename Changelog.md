## 1.13.0 (2021-02-16)


As part of this release we had [20 issues](https://github.com/codecov/codecov-exe/milestone/27?closed=1) closed.


__Bugs__

- [__#170__](https://github.com/codecov/codecov-exe/issues/170) Running on azure fails due to question mark not being escaped in the `build_url` parameter
- [__#146__](https://github.com/codecov/codecov-exe/issues/146) Execution broken with AppVeyor starting from codecov-exe v1.12.0

__Features__

- [__#172__](https://github.com/codecov/codecov-exe/issues/172) Add support for .NET 5.0
- [__#135__](https://github.com/codecov/codecov-exe/pull/135) Add new Codecov.MSBuild package providing a `Codecov`MSBuild task

__Improvements__

- [__#154__](https://github.com/codecov/codecov-exe/pull/154) Bump JetBrains.Annotations from 2020.1.0 to 2020.3.0 in /Source
- [__#133__](https://github.com/codecov/codecov-exe/pull/133) Bump Serilog from 2.9.0 to 2.10.0 in /Source


## 1.12.3 (2020-09-07)


As part of this release we had [26 commits](https://github.com/codecov/codecov-exe/compare/1.12.2...1.12.3) which resulted in [4 issues](https://github.com/codecov/codecov-exe/milestone/26?closed=1) being closed.


__Improvement__

- [__#129__](https://github.com/codecov/codecov-exe/pull/129) Bump Glob from 1.1.6 to 1.1.8 in /Source


## 1.12.2 (2020-08-17)


As part of this release we had [2 commits](https://github.com/codecov/codecov-exe/compare/1.12.1...1.12.2) which resulted in [1 issue](https://github.com/codecov/codecov-exe/milestone/25?closed=1) being closed.


__Bug__

- [__#122__](https://github.com/codecov/codecov-exe/issues/122) Incorrect Build Url is created when running on appveyor


## 1.12.1 (2020-07-10)


As part of this release we had [2 commits](https://github.com/codecov/codecov-exe/compare/1.12.0...1.12.1) which resulted in [1 issue](https://github.com/codecov/codecov-exe/milestone/24?closed=1) being closed.


__Bug__

- [__#120__](https://github.com/codecov/codecov-exe/issues/120) Codecov package have not been packaged as a standalone executable


## 1.12.0 (2020-06-23)


As part of this release we had [16 commits](https://github.com/codecov/codecov-exe/compare/1.11.2...1.12.0) which resulted in [5 issues](https://github.com/codecov/codecov-exe/milestone/22?closed=1) being closed.

__Bug__

- [__#117__](https://github.com/codecov/codecov/issues/117) Upload URL is reported twice when running with verbose debugging

__Improvements__

- [__#118__](https://github.com/codecov/codecov-exe/issues/118) Remove time and date from logging
- [__#115__](https://github.com/codecov/codecov-exe/issues/115) Load supported VCS providers using reflection
- [__#114__](https://github.com/codecov/codecov-exe/issues/114) Load CI parsers using reflection


## 1.11.2 (2020-06-21)


As part of this release we had [2 commits](https://github.com/codecov/codecov-exe/compare/1.11.1...1.11.2) which resulted in [1 issue](https://github.com/codecov/codecov-exe/milestone/23?closed=1) being closed.


__Bug__

- [__#71__](https://github.com/codecov/codecov-exe/issues/71) --file cannot handle paths with spaces


## 1.11.1 (2020-06-10)


As part of this release we had [15 commits](https://github.com/codecov/codecov-exe/compare/1.11.0...1.11.1) which resulted in [3 issues](https://github.com/codecov/codecov-exe/milestone/21?closed=1) being closed.


__Bugs__

- [__#112__](https://github.com/codecov/codecov-exe/issues/112) Chocolatey package includes invalid "Where to get it" section
- [__#111__](https://github.com/codecov/codecov-exe/issues/111) Release Notes in codecov chocolatey package contains invalid links
- [__#110__](https://github.com/codecov/codecov-exe/issues/110) Symbols for Codecov.Tool and nuget Codecov package do not match


## 1.11.0 (2020-06-10)


As part of this release we had [41 commits](https://github.com/codecov/codecov-exe/compare/1.10.0...1.11.0) which resulted in [7 issues](https://github.com/codecov/codecov-exe/milestone/20?closed=1) being closed.


__Bugs__

- [__#108__](https://github.com/codecov/codecov-exe/issues/108) Github action CI detector is not added to pipeline check
- [__#100__](https://github.com/codecov/codecov-exe/issues/100) GitHub Actions is not detected in 1.10

__Documentation__

- [__#98__](https://github.com/codecov/codecov-exe/issues/98) Add an example of using globbing patterns with the file argument

__Feature__

- [__#103__](https://github.com/codecov/codecov-exe/issues/103) Support disabling default uploader via command line

__Improvements__

- [__#105__](https://github.com/codecov/codecov-exe/pull/105) Remove Public Read header from presigned PUT
- [__#102__](https://github.com/codecov/codecov-exe/issues/102) Add ability to detect pull requests when running on github actions
- [__#101__](https://github.com/codecov/codecov-exe/issues/101) Add support for setting build id and build url on github actions


## 1.10.0 (2020-02-01)


As part of this release we had [11 commits](https://github.com/codecov/codecov-exe/compare/1.9.0...1.10.0) which resulted in [2 issues](https://github.com/codecov/codecov-exe/milestone/19?closed=1) being closed.


__Improvements__

- [__#97__](https://github.com/codecov/codecov-exe/issues/97) Warn users when total upload is larger than 20 MB in size
- [__#95__](https://github.com/codecov/codecov-exe/issues/95) Support .NET Core 3.x for Codecov.Tool

__Dependencies__

- Update CommandLineParser to 2.7.82
- Update Glob dependency to 1.1.4


## 1.9.0 (2019-11-14)


As part of this release we had [14 commits](https://github.com/codecov/codecov-exe/compare/1.8.0...1.9.0) which resulted in [2 issues](https://github.com/codecov/codecov-exe/milestone/18?closed=1) being closed.


__Documentation__

- [__#93__](https://github.com/codecov/codecov-exe/issues/93) Azure pipelines no longer needs a token

__Improvement__

- [__#92__](https://github.com/codecov/codecov-exe/pull/92) Set all properties for Azure Pipelines uploads (Thanks to @sharwell for providing this)
  This includes changes to allow Azure Pipelines to upload coverage reports without the use of a Codecov Token (*public repositories only*).

__Dependencies__

- Updated [CommandLineParser](https://www.nuget.org/packages/CommandLineParser) to version `2.6.0`
- Updated [Glob](https://www.nuget.org/packages/Glob) to version `1.1.4`
- Updated [Serilog](https://www.nuget.org/packages/Serilog) to version `2.9.0`


## 1.8.0 (2019-10-31)


As part of this release we had [13 commits](https://github.com/codecov/codecov-exe/compare/1.7.2...1.8.0) which resulted in [4 issues](https://github.com/codecov/codecov-exe/milestone/17?closed=1) being closed.


__Documentation__

- [__#88__](https://github.com/codecov/codecov-exe/issues/88) Documentation Request: There is a "dotnet tool" and this is how to use it!

__Features__

- [__#90__](https://github.com/codecov/codecov-exe/issues/90) Add support for github actions
- [__#31__](https://github.com/codecov/codecov-exe/issues/31) Jenkins CI Detection (thanks to @mikebro for adding this support)

__Improvement__

- [__#91__](https://github.com/codecov/codecov-exe/issues/91) Azure pipeline service detector uses incorrect service name


## 1.7.2 (2019-08-16)


As part of this release we had [14 commits](https://github.com/codecov/codecov-exe/compare/1.7.1...1.7.2) which resulted in [3 issues](https://github.com/codecov/codecov-exe/milestone/16?closed=1) being closed.


__Bugs__

- [__#87__](https://github.com/codecov/codecov-exe/issues/87) Travis CI service parser does not set the tag name from travis environment variables
- [__#86__](https://github.com/codecov/codecov-exe/issues/86) invalid build url generated for travis-ci when repository slug have been overridden
- [__#81__](https://github.com/codecov/codecov-exe/issues/81) Invalid build url generated for appveyor when account name is not same as repository account


## 1.7.1 (2019-08-02)


As part of this release we had [6 commits](https://github.com/codecov/codecov-exe/compare/1.7.0...1.7.1) which resulted in [3 issues](https://github.com/codecov/codecov-exe/milestone/15?closed=1) being closed.


__Bugs__

- [__#84__](https://github.com/codecov/codecov-exe/issues/84) Branch name do not properly encode slashes
- [__#83__](https://github.com/codecov/codecov-exe/issues/83) nuget package places Linux and OSX binaries in incorrect directory

__Documentation__

- [__#85__](https://github.com/codecov/codecov-exe/issues/85) Codecov nuget package incorrectly says it only supports windows


## 1.7.0 (2019-07-28)


As part of this release we had [9 commits](https://github.com/codecov/codecov-exe/compare/1.6.1...1.7.0) which resulted in [2 issues](https://github.com/codecov/codecov-exe/milestone/14?closed=1) being closed.


__Bug__

- [__#79__](https://github.com/codecov/codecov-exe/issues/79) Slug generation is incorrect when using username and password in url.

__Improvement__

- [__#78__](https://github.com/codecov/codecov-exe/issues/78) Improve logging by including response content when an error happens


## 1.6.1 (2019-07-16)


As part of this release we had [1 commit](https://github.com/codecov/codecov-exe/compare/1.6.0...1.6.1) which resulted in [1 issue](https://github.com/codecov/codecov-exe/milestone/13?closed=1) being closed.


__Bug__

- [__#77__](https://github.com/codecov/codecov-exe/issues/77) Running on Unix returns a permission denied error


## 1.6.0 (2019-07-15)


As part of this release we had [28 commits](https://github.com/codecov/codecov-exe/compare/1.5.0...1.6.0) which resulted in [7 issues](https://github.com/codecov/codecov-exe/milestone/12?closed=1) being closed.

This release is expected to be the last release that will include any new features until the eventual release of 2.0.


__Bug__

- [__#52__](https://github.com/codecov/codecov-exe/issues/52) codecov.exe returns success on invalid arguments

__Features__

- [__#68__](https://github.com/codecov/codecov-exe/issues/68) Support glob expression for file paths (Thanks to @hanabi1224 for providing this support)
- [__#66__](https://github.com/codecov/codecov-exe/issues/66) Embedd resulting build output in self-contained native binary using warp
- [__#65__](https://github.com/codecov/codecov-exe/issues/65) Add support for Travis CI
- [__#38__](https://github.com/codecov/codecov-exe/issues/38) Support uploading to v2 endpoint

__Improvements__

- [__#70__](https://github.com/codecov/codecov-exe/issues/70) Change from using Webclient to using HttpClient
- [__#67__](https://github.com/codecov/codecov-exe/issues/67) Include linux and Mac OSX binaries in Codecov package along with Windows binaries


## 1.5.0 (2019-05-05)


As part of this release we had [6 issues](https://github.com/codecov/codecov-exe/milestone/9?closed=1) closed.

This release finally provides official support for Linux and OS X platforms.
This allows Codecov-exe to be used on either Linux or OS X.
The recommended way is by installing the .NET Core tool ([Codecov.Tool](https://nuget.org/packages/Codecov.Tool/1.5.0)), or by downloading one of the platform specific archives provided with this release.


__Bugs__

- [__#62__](https://github.com/codecov/codecov-exe/issues/62) SourceCode files are do not correctly ignore files on unix platform
- [__#59__](https://github.com/codecov/codecov-exe/issues/59) codecov yaml not located

__Feature__

- [__#64__](https://github.com/codecov/codecov-exe/issues/64) Provide pre-built archives for linux and OS X platforms

__Improvements__

- [__#58__](https://github.com/codecov/codecov-exe/issues/58) FileSystem class should normalize path when directory separator is using a /
- [__#57__](https://github.com/codecov/codecov-exe/issues/57) AppVeyor CI Service check should do case-insensitive comparison


## 1.4.0 (2019-04-07)


As part of this release we had [5 commits](https://github.com/codecov/codecov-exe/compare/1.3.0...1.4.0) which resulted in [2 issues](https://github.com/codecov/codecov-exe/milestone/8?closed=1) being closed.


__Bug__

- [__#55__](https://github.com/codecov/codecov-exe/issues/55) azure pipeline is broken

__Improvement__

- [__#54__](https://github.com/codecov/codecov-exe/pull/54) Make the HttpWebRequest provide more details for Put failures


## 1.3.0 (2019-02-28)


As part of this release we had [14 commits](https://github.com/codecov/codecov-exe/compare/1.2.0...1.3.0) which resulted in [2 issues](https://github.com/codecov/codecov-exe/milestone/6?closed=1) being closed.


__Bug__

- [__#49__](https://github.com/codecov/codecov-exe/pull/49) Fix terminal filter

__Feature__

- [__#50__](https://github.com/codecov/codecov-exe/issues/50) Add support for Azure Pipelines


## 1.2.0 (2019-02-06)


As part of this release we had [9 commits](https://github.com/codecov/codecov-exe/compare/1.1.1...1.2.0) which resulted in [2 issues](https://github.com/codecov/codecov-exe/milestone/4?closed=1) being closed.


__Feature__

- [__#44__](https://github.com/codecov/codecov-exe/issues/44) Add a dotnet tool for codecov exe (thanks to @ViktorHofer for providing this)

__Improvement__

- [__#48__](https://github.com/codecov/codecov-exe/issues/48) Use .NET Core 2.1 WebClient instead of using PowerShell


## 1.1.1 (2019-01-10)


As part of this release we had [4 commits](https://github.com/codecov/codecov-exe/compare/1.1.0...1.1.1) which resulted in [1 issue](https://github.com/codecov/codecov-exe/milestone/5?closed=1) being closed.


__Bug__

- [__#46__](https://github.com/codecov/codecov-exe/pull/46) -f and -e command line arguments don't accept multiple values as the docs suggest


## 1.1.0 (2018-09-12)


As part of this release we had [8 commits](https://github.com/codecov/codecov-exe/compare/1.0.5...1.1.0) which resulted in [2 issues](https://github.com/codecov/codecov-exe/issues?milestone=2&state=closed) being closed.


__Improvements__

- [__#42__](https://github.com/codecov/codecov-exe/pull/42) GZip compress content for uploading
- [__#39__](https://github.com/codecov/codecov-exe/issues/39) Warn the user when a coverage file can't be found.


## 1.0.5 (2018-07-01)


As part of this release we had [3 commits](https://github.com/codecov/codecov-exe/compare/1.0.4...1.0.5) which resulted in [2 issues](https://github.com/codecov/codecov-exe/issues?milestone=3&state=closed) being closed.


__Bugs__

- [__#37__](https://github.com/codecov/codecov-exe/issues/37) Codecov returns (404) Not Found
- [__#36__](https://github.com/codecov/codecov-exe/issues/36) Report uploads fails on certain branches and/or job IDs


## 1.0.4 (2018-06-17)


As part of this release we had [2 issues](https://github.com/codecov/codecov-exe/issues?milestone=1&state=closed) closed.


__Bugs__

- [__#36__](https://github.com/codecov/codecov-exe/issues/36) Report uploads fails on certain branches and/or job IDs
- [__#32__](https://github.com/codecov/codecov-exe/issues/32) Bad request (400) with non-standard branch name


## 1.0.3 (2017-08-11)


### Bug

* (GH-26) Version 1.0.2 is freezing on appveyor.
## 1.0.2 (2017-08-07)


### Bug

#20 Upload timed out for large files.
#22 Git service fails if project located at path with spaces. ~ Dmitry Popov

### Improvement

#19 Cleaned up Chocolatey package code.
## 1.0.1 (2017-06-03)


 ### Bug
* #17 - `Required` commandline arg bug. ~ Kim J. Nordmo
## v1.0.0 (2017-05-25)


* Added support for yaml file.
* Implemented must have CLI options.
* Bug fixes and code clean up.
* Added unit tests.
## 0.4.0-Beta (2017-04-10)


- Added ability to manually discover the source code files without using git. Also, fixed how git is detected.
- Bug fixes for TeamCity service and added documentation for it. *TeamCity is officially supported now*.
- Added command line option to disable including source code files in upload (network).
## 0.3.0-Beta (2017-03-30)


* Ported to .NET Core standalone app. We now support win7 (x64) and above.
## 0.2.0-beta (2017-03-07)


- Added support for Travis Ci, AppVeyor, TeamCity (not tested), and Git.
- Added a lot of the CLI options.
## 0.1.0-beta (2017-02-21)


Initial Beta Release

- Added command line option to disable including source code files in upload (network).

## 0.3.0-Beta (2017-03-30)

- Ported to .NET Core standalone app. We now support win7 (x64) and above.

## 0.2.0-beta (2017-03-07)

- Added support for Travis Ci, AppVeyor, TeamCity (not tested), and Git.
- Added a lot of the CLI options.

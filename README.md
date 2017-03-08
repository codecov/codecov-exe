Codecov Global Executable (.exe) Uploader: PowerShell, Windows Command Line (cmd), .NET 4.5 and Mono.
=======
| [https://codecov.io/][1] | [@codecov][2] | [hello@codecov.io][3] |
| ------------------------ | ------------- | --------------------- |

This repository is currently under active development working towards version 0.1.0. All types of contributions are welcome! Feel free to open an issue or ask a question.

## Current features

- Upload a coverage report using the following services: AppVeyor, Travis Ci (using mono), TeamCity (not tested), Git.
- Many Codecov CLI options are supported. Run `.\codecov.exe` or see [Options.cs](https://github.com/codecov/codecov-exe/blob/master/source/codecov/codecov/Program/Options.cs) for more details.
- You can obtain the uploader via 
    - nuget [![NuGet](https://img.shields.io/nuget/v/Codecov.svg)](https://www.nuget.org/packages/Codecov/)
    - [Chocolatey](https://chocolatey.org/) (package has been submitted for review) `choco install codecov`.
    - Downloading it as an asset from the GitHub release (may not support this feature in later releases) `(New-Object System.Net.WebClient).DownloadFile("<url>", "codecov.exe")`.

## ToDoList in order of priority

- Implement (if any) must have CLI options.
- Bug fixes and code cleanup in order to release version 0.1.0
- Support more platforms (in addition to .NET 4.5): .NET Core and .NET 4.0 (maybe).
- Unit Tests, build scripts, and automation.
- Support Mercurial and other CI services.

[1]: https://codecov.io/
[2]: https://twitter.com/codecov
[3]: mailto:hello@codecov.io

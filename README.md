Codecov Global Executable (.exe) Uploader for PowerShell, Windows Command Line (cmd), .NET and Mono.
=======
| [https://codecov.io/][1] | [@codecov][2] | [hello@codecov.io][3] |
| ------------------------ | ------------- | --------------------- |

This repository is currently under active development working towards version 0.1.0. All types of contributions are welcome! Feel free to open an issue or ask a question.

## Current features

- Upload a coverage report locally using Git. Support for Hg is not implimented yet.
- Upload coverage report using AppVeyor.
- Upload coverage report using Travis Ci.
- Upload coverage report using TeamCity. This has not been tested.
- Implement Codecov command line interface. A lot of commands are supported, see [Options.cs](https://github.com/codecov/codecov-exe/blob/master/source/codecov/codecov/Program/Options.cs) for a full list. Or run `.\codecov`in powershell.
- You can obtain the .exe via nuget [![NuGet](https://img.shields.io/nuget/v/Codecov.svg)](https://www.nuget.org/packages/Codecov/). Supports .NET 4.5 (more to come later).
- Support for codecov.yaml or .codecov.yaml files.

## ToDoList

	- Test the code!
    - Add .exe to Choco (like apt-get or homebrew but for windows).
    - Create a PowerShell bootstraper script to download the .exe. The .exe will be an assest on a release. - In PowerShell run `(New-Object System.Net.WebClient).DownloadFile("https://github.com/codecov/codecov-exe/releases/download/0.1.0-beta/codecov.exe", "codecov.exe")` and then `.\codecov.exe ...`
    - Implimented the entire command line interface.
    - Support more than .NET 4.5

[1]: https://codecov.io/
[2]: https://twitter.com/codecov
[3]: mailto:hello@codecov.io

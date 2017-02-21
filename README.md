Codecov Global Executable (.exe) Uploader for PowerShell, Windows Command Line (cmd), .NET and Mono.
=======
| [https://codecov.io/][1] | [@codecov][2] | [hello@codecov.io][3] |
| ------------------------ | ------------- | --------------------- |

This repository is currently under active development working towards version 0.1.0. All types of contributions are welcome! Feel free to open an issue or ask a question.

## ToDoList

- [x] Upload a coverage report locally using Git.
- [ ] Upload a coverage report locally using Hg.
- [x] Upload coverage report using AppVeyor - not tested very well.
- [ ] Upload coverage report using Travis-CI.
- [ ] Upload a coverage report using TeamCity.
- [x] Implement Codecov command line interface. - VERY basic CLI.
- [ ] Get an AppVeyor build setup (using Cake Script). Add some development tooling.
- [ ] Unit and integration tests.
- Three ways to obtain the .exe
    - [x] Add .exe to NuGet (For .NET developers). [![NuGet](https://img.shields.io/nuget/v/Codecov.svg)](https://www.nuget.org/packages/Codecov/)
    - [ ] Add .exe to Choco (like apt-get or homebrew but for windows).
    - [x] Create a PowerShell bootstraper script to download the .exe. The .exe will be an assest on a release. - In PowerShell run `(New-Object System.Net.WebClient).DownloadFile("https://github.com/codecov/codecov-exe/releases/download/0.1.0-beta/codecov.exe", "codecov.exe")` and then `.\codecov.exe ...`
    
## CI Providers
|                       Company                       |                                                                                     Supported                                                                                      |  Token Required  |
| --------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------------- |
| [Travis CI](https://travis-ci.org/)                 | No                                                                                                                                                                                | Private only     |
| [CircleCI](https://circleci.com/)                   | No                                                                                                                                                                                | Private only     |
| [Codeship](https://codeship.com/)                   | No                                                                                                                                                                                | Public & Private |
| [Jenkins](https://jenkins-ci.org/)                  | No                                                                                                                                                                                | Public & Private |
| [Semaphore](https://semaphoreci.com/)               | No                                                                                                                                                                                | Public & Private |
| [Drone.io](https://drone.io/)                       | No                                                                                                                                                                                | Public & Private |
| [AppVeyor](http://www.appveyor.com/)                | No                                                                                                                                                                                | Public & Private |
| [Wercker](http://wercker.com/)                      | No                                                                                                                                                                                | Public & Private |
| [Magnum CI](https://magnum-ci.com/)                 | No                                                                                                                                                                                | Public & Private |
| [Shippable](http://www.shippable.com/)              | No                                                                                                                                                                                | Public & Private |
| [Gitlab CI](https://about.gitlab.com/gitlab-ci/)    | No                                                                                                                                                                                | Public & Private |
| [Snap CI](https://snap-ci.com/)                     | No                                                                                                                                                                                | Public & Private |
| git / mercurial                                     | No (as a fallback)                                                                                                                                                                | Public & Private |
| [Buildbot](http://buildbot.net/)                    | No                                                                                           |                  |
| [Bamboo](https://www.atlassian.com/software/bamboo) | No                                                                                                                                                                      |                  |
| [Solano Labs](https://www.solanolabs.com/)          | No                                                                                                                                                                      |                  |

> Using **Travis CI**? Uploader is compatible with `sudo: false` which can speed up your builds. :+1:

[1]: https://codecov.io/
[2]: https://twitter.com/codecov
[3]: mailto:hello@codecov.io

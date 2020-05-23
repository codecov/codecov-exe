#load "./packaging.cake"
var testTask = Task("Test")
    .Does<BuildData>((data) =>
{
    if (DirectoryExists(data.Directories.TestResults)) {
        DeleteDirectory(data.Directories.TestResults, new DeleteDirectorySettings {
            Recursive = true,
            Force = true,
        });
    }

    DotNetCoreTest(data.Files.TestProject.ToString(), new DotNetCoreTestSettings {
        Configuration = data.Configuration,
        NoBuild = true,
        ArgumentCustomization = args=>args.Append("--collect:\"XPlat Code Coverage\""),
        Settings = data.Files.TestProjectSettings,
    });
});

var coverageGenTask = Task("Generate-LocalCoverage")
    .Does<BuildData>((data) =>
{
    var files = GetFiles(data.Directories.TestResults + "/**/*.xml");
    var exePrefix = IsRunningOnWindows() ? ".exe" : "";
    var outputDir = data.Directories.Coverage;
    if (DirectoryExists(outputDir)) {
        DeleteDirectory(outputDir, new DeleteDirectorySettings {
            Recursive = true,
            Force = true,
        });
    }

    ReportGenerator(files, outputDir, new ReportGeneratorSettings {
        ToolPath = $"./tools/reportgenerator{exePrefix}",
    });
});

var coverageTask = Task("Upload-CoverageReport")
    .IsDependentOn(createDotNetToolTask)
    .Does<BuildData>((data) =>
{
    var exePrefix = IsRunningOnWindows() ? ".exe" : "";
    DirectoryPath netToolPackage = data.Directories.Packages + "/dotnet";
    string exePath = string.Empty;
    if (IsRunningOnWindows())
    {
        exePath = data.Directories.Compiled + "/win7-x64/codecov.exe";
    }
    else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
    {
        exePath = data.Directories.Compiled + "/linux-x64/codecov";
    }
    else
    {
        exePath = data.Directories.Compiled + "/osx-x64/codecov";
    }

    Codecov(new CodecovSettings {
        Files    = new[] { data.Directories.TestResults + "/**/*.xml"},
        ToolPath = exePath,
        Required = true,
        Verbose  = true,
    });
});

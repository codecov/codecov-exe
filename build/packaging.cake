#load "./build.cake"
var createExecsTask = Task("Create-Executables");
var createArchivesTask = Task("Create-Archives");
var createChocolateyPackagesTask = Task("Create-ChocolateyPackages")
    .WithCriteria(IsRunningOnWindows)
    .Does<BuildData>((data) =>
{
    var outputDirectory = data.Directories.Packages.Combine("chocolatey");
    var nuspecFiles = GetFiles(data.Directories.Nuspecs + "/chocolatey/*.nuspec");
    EnsureDirectoryExists(outputDirectory);

    var files = (GetFiles(data.Directories.Nuspecs + "/chocolatey/tools/*")
        + GetFiles(data.Directories.Archives + "/*win7*.zip")
        + File("./LICENSE.txt")).Select((f) => new ChocolateyNuSpecContent { Source = MakeAbsolute(f).ToString(), Target = "tools" });

    var settings = new ChocolateyPackSettings {
        Version = data.Version.SemVersionPadded,
        OutputDirectory = outputDirectory,
        Files = files.ToList(),
    };

    var milestonePath = data.Files.Milestone;

    if (FileExists(milestonePath)) {
        settings.ReleaseNotes = System.IO.File.ReadAllLines(milestonePath.ToString());
    }

    ChocolateyPack(nuspecFiles, settings);
});

var project = ParseProject("./Source/Codecov/Codecov.csproj", Argument("configuration", "Release"));
var runtimes = Argument("runtimes", "").Split(";", StringSplitOptions.RemoveEmptyEntries);
bool buildNuget = false;
if (!runtimes.Any()) {
    runtimes = project.NetCore.RuntimeIdentifiers;
    buildNuget = true;
}

foreach (var runtime in runtimes) {
    Task($"Create-{runtime}Exec")
        .IsDependeeOf(createExecsTask.Task.Name)
        .Does<BuildData>((data) =>
    {
        var msBuildSettings = new DotNetCoreMSBuildSettings();
        foreach (var kv in data.MSBuildSettings.Properties) {
            string value = string.Join(" ", kv.Value);
            msBuildSettings.WithProperty(kv.Key, value);
        }
        msBuildSettings.WithProperty("PubllishSingleFile", "true")
            .WithProperty("PublishTrimmed", "true");

        var outputDir = data.Directories.Compiled.Combine(runtime);
        DotNetCorePublish(project.ProjectFilePath.FullPath, new DotNetCorePublishSettings {
            Configuration   = data.Configuration,
            Runtime         = runtime,
            SelfContained   = true,
            OutputDirectory = outputDir,
            MSBuildSettings = msBuildSettings,
        });
    });

    var t = Task($"Create-{runtime}Archive")
        .IsDependentOn($"Create-{runtime}Exec")
        .IsDependeeOf(createArchivesTask.Task.Name)
        .Does<BuildData>((data) =>
    {
        var output = data.Directories.Archives.CombineWithFilePath($"codecov-{runtime}.zip");
        EnsureDirectoryExists(data.Directories.Archives);
        Zip(data.Directories.Compiled.Combine(runtime), output);
    });
    if (runtime.Contains("win7")) {
        t.IsDependeeOf(createChocolateyPackagesTask.Task.Name);
    }
}

var createDotNetToolTask = Task("Create-DotNetToolPackage")
    .IsDependentOn(buildTask)
    .Does<BuildData>((data) =>
{
    var output = data.Directories.Packages.Combine("dotnet");

    DotNetCorePack("./Source/Codecov.Tool", new DotNetCorePackSettings {
        Configuration = data.Configuration,
        NoBuild = true,
        NoRestore = true,
        OutputDirectory = output,
        MSBuildSettings = data.MSBuildSettings,
        IncludeSymbols = true,
    });
});

var createNuGetPackagesTask = Task("Create-NuGetPackages")
    .WithCriteria(buildNuget)
    .IsDependentOn(createExecsTask)
    .Does<BuildData>((data) =>
{
    var outputDirectory = data.Directories.Packages.Combine("nuget");
    var nuspecsBase = data.Directories.Nuspecs.Combine("nuget").MakeAbsolute(Context.Environment);
    var nuspecFiles = GetFiles(nuspecsBase + "/*.nuspec");
    var basePath = data.Directories.Compiled.MakeAbsolute(Context.Environment);

    NuGetPack(nuspecFiles, new NuGetPackSettings {
        Version                 = data.Version.SemVersion,
        Symbols                 = false,
        BasePath                = basePath,
        OutputDirectory         = outputDirectory,
    });
});

var createPackagesTask = Task("Create-Packages")
    .IsDependentOn(createDotNetToolTask)
    .IsDependentOn(createNuGetPackagesTask)
    .IsDependentOn(createChocolateyPackagesTask);

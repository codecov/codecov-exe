public class BuildData
{
    public string Configuration { get; set; }

    public BuildDirectories Directories { get; } = new BuildDirectories();
    public BuildFiles Files { get; } = new BuildFiles();

    public DotNetCoreMSBuildSettings MSBuildSettings { get; set; }

    public bool PushTag { get; set; }

    public string RepositoryName { get; set; }
    public string RepositoryOwner { get; set; }

    public string[] Runtimes { get; set; }

    public BuildVersion Version { get; set; }
}

public class BuildVersion
{
    public string SemVersion { get; set; }
    public string SemVersionPadded { get; set; }
    public string MajorMinorPatch { get; set; }
    public string InformationalVersion { get; set; }
}

public class BuildDirectories
{
    public DirectoryPath Artifacts   => "./artifacts";
    public DirectoryPath Archives    => Artifacts.Combine("archives");
    public DirectoryPath Compiled    => Artifacts.Combine("output");
    public DirectoryPath Coverage    => Artifacts.Combine("coverage");
    public DirectoryPath Nuspecs     => "./nuspec";
    public DirectoryPath Packages    => Artifacts.Combine("packages");
    public DirectoryPath TestResults => "./Source/Codecov.Tests/TestResults";
}

public class BuildFiles
{
    public FilePath Solution            => "./Source/Codecov.sln";
    public FilePath MainProject         => "./Source/Codecov/Codecov.csproj";
    public FilePath Milestone { get; set; }
    public FilePath TestProject         => "./Source/Codecov.Tests/Codecov.Tests.csproj";
    public FilePath TestProjectSettings => "./Source/Codecov.Tests/coverlet.runSettings";
}

Setup<BuildVersion>((ctx) => {
        var gitVersion = ctx.GitVersion(new GitVersionSettings
        {
            OutputType = GitVersionOutput.Json,
        });

        ctx.Information($"Building Codecove.exe version {gitVersion.SemVer}!");

        return new BuildVersion
        {
            MajorMinorPatch      = gitVersion.MajorMinorPatch,
            SemVersion           = gitVersion.SemVer,
            SemVersionPadded     = gitVersion.LegacySemVerPadded,
            InformationalVersion = gitVersion.InformationalVersion,
        };
});

Setup<DotNetCoreMSBuildSettings>((ctx) => {
    var version = ctx.Data.Get<BuildVersion>();

    var buildSettings = new DotNetCoreMSBuildSettings()
        .WithProperty("Version", version.SemVersion)
        .WithProperty("AssemblyVersion", version.MajorMinorPatch)
        .WithProperty("FileVersion", version.MajorMinorPatch)
        .WithProperty("AssemblyInformationalVersion", version.InformationalVersion)
        .WithProperty("UseSourceLink", "true");

    if (!ctx.BuildSystem().IsLocalBuild)
    {
        buildSettings.WithProperty("ContinuousIntegrationBuild", "true")
            .WithProperty("EmbedUntrackedSources", "true")
            .WithProperty("PublishRepositoryUrl", "true")
            .WithProperty("Deterministic", "true");
    }

    return buildSettings;
});

Setup<BuildData>((ctx) => {
    var data = new BuildData {
        Configuration = ctx.Argument("configuration", "Release"),
        MSBuildSettings = ctx.Data.Get<DotNetCoreMSBuildSettings>(),
        PushTag = ctx.HasArgument("push"),
        RepositoryName = "codecov-exe",
        RepositoryOwner = "codecov",
        Version = ctx.Data.Get<BuildVersion>(),
    };
    data.Files.Milestone = data.Directories.Artifacts.CombineWithFilePath("Milestone.md");

    return data;
});

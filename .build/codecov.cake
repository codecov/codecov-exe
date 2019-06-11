#if !CUSTOM_CODECOV
Setup<CodecovSettings>((context) => {
    var settings = new CodecovSettings {
        Required = true,
        Verbose = true
    };

    if (BuildParameters.Version != null && !string.IsNullOrEmpty(BuildParameters.Version.FullSemVersion) && BuildParameters.IsRunningOnAppVeyor) {
        // Required to work correctly with appveyor because environment changes isn't detected until cake is done running.
        var buildVersion = string.Format("{0}.build.{1}",
            BuildParameters.Version.FullSemVersion,
            BuildSystem.AppVeyor.Environment.Build.Number);
        settings.EnvironmentVariables = new Dictionary<string, string> {{"APPVEYOR_BUILD_VERSION", buildVersion }};
    }

    return settings;
});
#endif

public static void SetToolPath(ICakeContext context, CodecovSettings settings)
{
    if (context.IsRunningOnUnix())
    {
        // Special case, as the addin version used by Cake.Recipe do not support
        // the Correct unix paths.
        settings.ToolPath = context.Tools.Resolve("codecov");
    }
}

((CakeTask)BuildParameters.Tasks.UploadCodecovReportTask.Task).Actions.Clear();
((CakeTask)BuildParameters.Tasks.UploadCodecovReportTask.Task).Criterias.Clear();

var isRunningOnTravisCi = BuildSystem.IsRunningOnTravisCI;

public bool CanPublishToCodecov
{
    get
    {
        return BuildParameters.ShouldRunCodecov &&
                (!string.IsNullOrEmpty(BuildParameters.Codecov.RepoToken) ||
                BuildParameters.IsRunningOnAppVeyor || isRunningOnTravisCi);
    }
}

BuildParameters.Tasks.UploadCodecovReportTask
    //.WithCriteria(() => BuildParameters.IsMainRepository, "This is not the main repository")
    .WithCriteria(() => CanPublishToCodecov, "Codecov not enabled, missing token, not running on appveyor or not running on travis")
    .Does<CodecovSettings>((settings) => {
    var script = $"#tool dotnet:file://{MakeAbsolute(BuildParameters.Paths.Directories.NuGetPackages)}?package=Codecov.Tool&version={BuildParameters.Version.SemVersion}&prerelease";
    RequireTool(script, () => {
        SetToolPath(Context, settings);

        var files = GetFiles(BuildParameters.Paths.Directories.TestCoverage + "/coverlet/*");
        if (FileExists(BuildParameters.Paths.Files.TestCoverageOutputFilePath)) {
            files += BuildParameters.Paths.Files.TestCoverageOutputFilePath;
        }

        if (files.Any()) {
            settings.Files = files.Select(f => f.FullPath);
            Codecov(settings);
        }
    });
});

BuildParameters.Tasks.DefaultTask.IsDependentOn(BuildParameters.Tasks.UploadCodecovReportTask);

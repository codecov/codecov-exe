var configuration = Argument("configuration", "Release");

var buildTask = Task("Build")
    .Does<BuildData>((data) =>
{
    DotNetCoreBuild("./Source/Codecov.sln", new DotNetCoreBuildSettings {
        Configuration = data.Configuration,
        MSBuildSettings = data.MSBuildSettings,
    });
});

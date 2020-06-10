#load "./build/buildData.cake"
#load "./build/*.cake"


var target = Argument("target", "Default");

testTask.IsDependentOn(buildTask);
createChocolateyPackagesTask.IsDependentOn(exportReleaseNotesTask);

Task("Cleanup")
    .Does<BuildData>((data) =>
{
    var directories = GetDirectories("./**/bin") + data.Directories.Artifacts;

    CleanDirectories(directories);
});

Task("Init")
    .Does(() =>
{
    GitVersion(new GitVersionSettings
    {
        OutputType = GitVersionOutput.BuildServer,
    });
});

Task("Default")
    .IsDependentOn("Cleanup")
    .IsDependentOn(testTask)
    .IsDependentOn(createExecsTask)
    .IsDependentOn(createDotNetToolTask);

Task("CI")
    .IsDependentOn("Default")
    .IsDependentOn(coverageTask)
    .IsDependentOn(createPackagesTask)
    .IsDependentOn(publishPackagesTask);

Task("Full-Build")
    .IsDependentOn("Default")
    .IsDependentOn(coverageGenTask)
    .IsDependentOn(createPackagesTask)
    .IsDependentOn(createArchivesTask);

RunTarget(target);

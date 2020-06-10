#load "./packaging.cake"
// ^^ Make sure that the file is available before we try to
// reference one of the tasks in that file

var exportReleaseNotesTask = Task("Export-MilestoneReleaseNotes")
    .WithCriteria(() => HasEnvironmentVariable("GITHUB_TOKEN"))
    .WithCriteria(() => BuildSystem.AppVeyor.IsRunningOnAppVeyor)
    .WithCriteria(() => BuildSystem.AppVeyor.Environment.Repository.Tag.IsTag)
    .Does<BuildData>((data) =>
{
    var destination = data.Files.Milestone;
    var token = EnvironmentVariable("GITHUB_TOKEN");

    GitReleaseManagerExport(token, data.RepositoryOwner, data.RepositoryName, destination, new GitReleaseManagerExportSettings {
        TagName = BuildSystem.AppVeyor.Environment.Repository.Tag.Name,
    });
});

var createReleaseNotesTask = Task("Create-MilestoneReleaseNotes")
    .WithCriteria(() => HasEnvironmentVariable("GITHUB_TOKEN"))
    .WithCriteria(() => BuildSystem.GitHubActions.IsRunningOnGitHubActions)
    .WithCriteria(() => !BuildSystem.GitHubActions.Environment.PullRequest.IsPullRequest)
    .WithCriteria(() => BuildSystem.GitHubActions.Environment.Workflow.Ref != BuildSystem.GitHubActions.Environment.Workflow.HeadRef)
    .Does<BuildData>((data) =>
{
    var token = EnvironmentVariable("GITHUB_TOKEN");

    GitReleaseManagerCreate(token, data.RepositoryOwner, data.RepositoryName, new GitReleaseManagerCreateSettings {
        Milestone = data.Version.MajorMinorPatch,
        Prerelease = data.Version.SemVersion.Contains('-'),
        TargetCommitish = BuildSystem.GitHubActions.Environment.Workflow.Ref,
    });
});

var createTagTask = Task("Create-Tag")
    .Does<BuildVersion>((buildVersion) =>
{
    string message;
    if (buildVersion.MajorMinorPatch == buildVersion.SemVersion)
    {
        message = $"New official release of {buildVersion.MajorMinorPatch}";
    }
    else
    {
        message = $"Unstable pre-release of upcoming {buildVersion.MajorMinorPatch} release ({buildVersion.SemVersion})";
    }

    StartProcess("git", $"tag master {buildVersion.SemVersion} --sign --message \"{message}\"");

    if (HasArgument("push"))
    {
        StartProcess("git", "push --follow-tags");
    }
});

var uploadReleaseArtifactsTask = Task("Upload-ReleaseArtifacts")
    .WithCriteria(() => HasEnvironmentVariable("GITHUB_TOKEN"))
    .WithCriteria(() => BuildSystem.AppVeyor.IsRunningOnAppVeyor)
    .WithCriteria(() => BuildSystem.AppVeyor.Environment.Repository.Tag.IsTag)
    .WithCriteria(IsRunningOnWindows)
    .IsDependentOn(createArchivesTask)
    .Does<BuildData>((data) =>
{
    var files = string.Join(',', GetFiles(data.Directories.Archives + "/*.zip").Select(a => a.ToString()));
    var token = EnvironmentVariable("GITHUB_TOKEN");

    GitReleaseManagerAddAssets(token, data.RepositoryOwner, data.RepositoryName, BuildSystem.AppVeyor.Environment.Repository.Tag.Name, files);
});

var closeMilestoneTask = Task("Close-Milestones")
    .WithCriteria(() => HasEnvironmentVariable("GITHUB_TOKEN"))
    .WithCriteria(() => BuildSystem.GitHubActions.IsRunningOnGitHubActions)
    .Does<BuildData>((data) =>
{
    var token = EnvironmentVariable("GITHUB_TOKEN");
    var tag = BuildSystem.GitHubActions.Environment.Workflow.Ref;
    int index = tag.LastIndexOf('/');
    if (index > -1)
    {
        tag = tag.Substring(index+1);
    }

    GitReleaseManagerClose(token, data.RepositoryOwner, data.RepositoryName, tag);
});

var publishChocolateyPackagesTask = Task("Publish-ChocolateyPackages")
    .WithCriteria(() => HasEnvironmentVariable("CHOCOLATEY_API_KEY"))
    .WithCriteria(() => HasEnvironmentVariable("CHOCOLATEY_SOURCE"))
    .WithCriteria(() => BuildSystem.AppVeyor.IsRunningOnAppVeyor)
    .WithCriteria(() => BuildSystem.AppVeyor.Environment.Repository.Tag.IsTag)
    .WithCriteria(IsRunningOnWindows)
    .IsDependentOn(createChocolateyPackagesTask)
    .Does<BuildData>((data) =>
{
    var packages = GetFiles(data.Directories.Packages + "/chocolatey/*.nupkg");

    ChocolateyPush(packages, new ChocolateyPushSettings {
        Source = EnvironmentVariable("CHOCOLATEY_SOURCE"),
        ApiKey = EnvironmentVariable("CHOCOLATEY_API_KEY"),
    });
});

var publishNuGetPackagesTask = Task("Publish-NuGetPackages")
    .WithCriteria(() => HasEnvironmentVariable("NUGET_API_KEY"))
    .WithCriteria(() => HasEnvironmentVariable("NUGET_SOURCE"))
    .WithCriteria(() => BuildSystem.AppVeyor.IsRunningOnAppVeyor)
    .WithCriteria(() => BuildSystem.AppVeyor.Environment.Repository.Tag.IsTag)
    .WithCriteria(IsRunningOnWindows)
    .IsDependentOn(createNuGetPackagesTask)
    .Does<BuildData>((data) =>
{
    var packages = GetFiles(data.Directories.Packages + "/nuget/*.nupkg");

    NuGetPush(packages, new NuGetPushSettings {
        Source = EnvironmentVariable("NUGET_SOURCE"),
        ApiKey = EnvironmentVariable("NUGET_API_KEY"),
        SkipDuplicate = true,
    });
});

var publishDotNetToolTask = Task("Publish-DotNetToolPackage")
    .WithCriteria(() => HasEnvironmentVariable("NUGET_API_KEY"))
    .WithCriteria(() => HasEnvironmentVariable("NUGET_SOURCE"))
    .WithCriteria(() => BuildSystem.AppVeyor.IsRunningOnAppVeyor)
    .WithCriteria(() => BuildSystem.AppVeyor.Environment.Repository.Tag.IsTag)
    .WithCriteria(IsRunningOnWindows)
    .IsDependentOn(createDotNetToolTask)
    .Does<BuildData>((data) =>
{
    var packages = GetFiles("./artifacts/packages/dotnet/*.nupkg");

    DotNetCoreNuGetPush(data.Directories.Packages + "/dotnet/*.nupkg", new DotNetCoreNuGetPushSettings {
        Source = EnvironmentVariable("NUGET_SOURCE"),
        ApiKey = EnvironmentVariable("NUGET_API_KEY"),
    });
});

var publishGitHubReleaseTask = Task("Publish-GitHubRelease")
    .WithCriteria(() => HasEnvironmentVariable("GITHUB_TOKEN"))
    .WithCriteria(() => BuildSystem.AppVeyor.IsRunningOnAppVeyor)
    .WithCriteria(() => BuildSystem.AppVeyor.Environment.Repository.Tag.IsTag)
    .WithCriteria(IsRunningOnWindows)
    .IsDependentOn(uploadReleaseArtifactsTask)
    .Does<BuildData>((data) =>
{
    var token = EnvironmentVariable("GITHUB_TOKEN");

    GitReleaseManagerPublish(token, data.RepositoryOwner, data.RepositoryName, BuildSystem.AppVeyor.Environment.Repository.Tag.Name);
});

var publishPackagesTask = Task("Publish-Packages")
    .IsDependentOn(publishChocolateyPackagesTask)
    .IsDependentOn(publishNuGetPackagesTask)
    .IsDependentOn(publishDotNetToolTask)
    .IsDependentOn(publishGitHubReleaseTask);

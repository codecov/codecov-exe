#tool nuget:?package=OpenCover
#tool nuget:?package=Codecov
#addin nuget:?package=Cake.Codecov
#addin nuget:?package=Cake.Figlet

var target = Argument("target", "Default");

Setup(context =>
{
    Information(Figlet("Codecov-exe"));
});

Task("Clean").Does(() =>
{
	DeleteFiles("./nuspec/**/*.nupkg");
	DeleteFile("./coverage.xml");
	CleanDirectories(new[]{"./Source/Codecov/bin","./Source/Codecov.Tests/bin"});
});

Task("Restore").IsDependentOn("Clean").Does(() =>
{
	DotNetCoreRestore("./Source");
});

Task("Build").IsDependentOn("Restore").Does(() =>
{
	DotNetCoreBuild("./Source");
});

Task("Build-Release").IsDependentOn("Restore").Does(() =>
{
	DotNetCoreBuild("./Source/Codecov", new DotNetCoreBuildSettings{Runtime = "win7-x64"});
	DotNetCorePublish("./Source/Codecov",new DotNetCorePublishSettings{Runtime = "win7-x64",Configuration = "Release"});
});

Task("Tests").IsDependentOn("Build").Does(() =>
{
	OpenCover(tool => tool.DotNetCoreTest("./Source/Codecov.Tests/Codecov.Tests.csproj"), new FilePath("coverage.xml"), new OpenCoverSettings { OldStyle = true }.WithFilter("+[codecov]*"));
});

Task("Push-Coverage").IsDependentOn("Tests").Does(() =>
{
	if(AppVeyor.IsRunningOnAppVeyor)
	{
		Codecov("coverage.xml");
	}
	else
	{
		Information("Skipping pushing coverage reports.");
	}
});

Task("NuGet-Pack").IsDependentOn("Build-Release").Does(() =>
{
	NuGetPack("./nuspec/nuget/Codecov.nuspec", new NuGetPackSettings{OutputDirectory = "./nuspec/nuget"});
});

Task("Chocolatey-Pack").IsDependentOn("Build-Release").Does(() =>
{
	ChocolateyPack("./nuspec/chocolatey/codecov.nuspec", new ChocolateyPackSettings{OutputDirectory = "./nuspec/chocolatey"});
});

Task("Default").IsDependentOn("Push-Coverage").IsDependentOn("NuGet-Pack").IsDependentOn("Chocolatey-Pack");

RunTarget(target);

#addin nuget:?package=Cake.Figlet

var target = Argument("target", "Default");

Setup(context =>
{
    Information(Figlet("Codecov-exe"));
});

Task("Clean").Does(() =>
{
	DeleteFiles("./nuspec/**/*.nupkg");
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
	DotNetCoreTest("./Source/Codecov.Tests/Codecov.Tests.csproj");
});

Task("NuGet-Pack").IsDependentOn("Build-Release").Does(() =>
{
	NuGetPack("./nuspec/nuget/Codecov.nuspec", new NuGetPackSettings{OutputDirectory = "./nuspec/nuget"});
});

Task("Chocolatey-Pack").IsDependentOn("Build-Release").Does(() =>
{
	ChocolateyPack("./nuspec/chocolatey/codecov.nuspec", new ChocolateyPackSettings{OutputDirectory = "./nuspec/chocolatey"});
});

Task("Default").IsDependentOn("Tests").IsDependentOn("NuGet-Pack").IsDependentOn("Chocolatey-Pack");

RunTarget(target);

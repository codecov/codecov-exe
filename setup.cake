#addin nuget:?package=Cake.Figlet

var target = Argument("target", "Default");

Setup(context =>
{
    Information(Figlet("Codecov-exe"));
    Information("\t\tMIT License");
    Information(string.Format("\tCopyright (c) {0} Larz White\n",DateTime.Now.Year));
});

Task("Clean").Does(() =>
{
	DeleteFiles("./nuspec/**/*.nupkg");
	CleanDirectories(new[]{"./Source/Codecov/bin","./Source/Codecov.Tests/bin", "./Reports"});
});

Task("Restore").IsDependentOn("Clean").Does(() =>
{
	DotNetCoreRestore("./Source");
});

Task("Build").IsDependentOn("Restore").Does(() =>
{
	DotNetCoreBuild("./Source/Codecov.sln");
});

Task("Build-Release").IsDependentOn("Restore").Does(() =>
{
	DotNetCoreBuild("./Source/Codecov/Codecov.csproj", new DotNetCoreBuildSettings{Runtime = "win7-x64"});
	DotNetCorePublish("./Source/Codecov/Codecov.csproj",new DotNetCorePublishSettings{Runtime = "win7-x64",Configuration = "Release"});
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


Task("Debug").IsDependentOn("Tests");

Task("Release").IsDependentOn("NuGet-Pack").IsDependentOn("Chocolatey-Pack");

Task("Default").IsDependentOn("Debug").IsDependentOn("Release");

RunTarget(target);
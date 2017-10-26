#tool "nuget:?package=xunit.runner.console"
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "BuildPackages");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var artifactsDir  = Directory("./artifacts/");
var rootAbsoluteDir = MakeAbsolute(Directory("./")).FullPath;

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(artifactsDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    // NuGetRestore("./GST.Library.sln");
	DotNetCoreRestore("src");
});

Task("BuildPackages")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
	var rootAbsoluteDir = "./build/bin";
    var nuGetPackSettings = new NuGetPackSettings
	{
		OutputDirectory = rootAbsoluteDir + @"\artifacts\",
		IncludeReferencedProjects = true,
		Properties = new Dictionary<string, string>
		{
			{ "Configuration", "Release" }
		}
	};

	var settings = new DotNetCoreBuildSettings
     {
         Framework = "netcoreapp2.0",
         Configuration = "Release",
         OutputDirectory = "./artifacts/"
     };

	DotNetCoreBuild("./src/**/project.json", settings);

	var packageSettings = new DotNetCorePackSettings
     {
         Configuration = "Release",
         OutputDirectory = "./artifacts/"
     };

    DotNetCorePack("./src/GST.Library.API.REST/", packageSettings);
	DotNetCorePack("./src/GST.Library.Data/", packageSettings);
	DotNetCorePack("./src/GST.Library.Helper/", packageSettings);
	DotNetCorePack("./src/GST.Library.StoredProcedureHelper/", packageSettings);

});


//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

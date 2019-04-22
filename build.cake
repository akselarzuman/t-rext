#addin "Cake.Incubator&version=5.0.1"

using System;
using System.Diagnostics;

var target = Argument("target", "Default");

Task("Default")
    .IsDependentOn("Build");

Task("Build")
    .Does(() =>
{
    DotNetCoreBuildSettings settings = new DotNetCoreBuildSettings
    {
        NoRestore = true,
        Configuration = "Release"
    };

    var projects = GetFiles("t-rext/t-rext.csproj");

    Information($"Restoring projects");
    foreach(var project in projects)
    {
        DotNetCoreRestore(project.ToString());
    }

    Information($"Building projects");
    foreach(var project in projects)
    {
        DotNetCoreBuild(project.ToString(), settings);
    }
});

Task("Nuget-Pack")
    .Description("Publish to nuget")
    .Does(() =>
    {
        var settings = new DotNetCorePackSettings
        {
            Configuration = "Release",
            OutputDirectory = "./artifacts/t-rext",
            WorkingDirectory = "src/t-rext"
        };

        DotNetCorePack("t-rext.csproj", settings);
    });


RunTarget(target);
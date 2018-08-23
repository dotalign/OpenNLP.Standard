/*
 * This is a build configuration file.  It uses Cake to do the building. http://cakebuild.net
 * Release builds are code-signed, so be sure to pass in code signing password on TeamCity
 * To run this call:
 *    > powershell .\build.ps1
 */

var target = Argument("target", "Default");

var runnerFactory = new RunnerFactory();
runner = runnerFactory.NewCoreRunner(Context);

// *****************************************************************************
// Startup
// *****************************************************************************
#addin nuget:?package=Cake.Git&version=0.16.1
#load nuget:?package=Dot.Cake.Recipe&version=1.0.135

Warnings.DowngradePackagingWarnings();


Task("Default")
    .IsDependentOn("Information")
    .IsDependentOn("Restore")
    .IsDependentOn("Clean")
    .IsDependentOn("Build")
    .IsDependentOn("CodeSign")
    .IsDependentOn("Test")
    .IsDependentOn("Package")
    .IsDependentOn("Publish")
    .Does(() =>
{
    Information("Build successfully completed");
});

// Run Tasks
RunTarget(target);

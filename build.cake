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

// *****************************************************************************
// BlankDbEnsurer
// copy blank Db to temp folder
// *****************************************************************************
Task("BlankDbEnsurer")
    .Does(() => 
    {

      var destLocation = new System.IO.FileInfo(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Blank.dotdb3"));
      if (!System.IO.File.Exists(destLocation.FullName)) 
      {
        var sqliteAssemblyPath = @".\Sqlite\Dot.Rdbms.Sqlite\bin\Release\net40\Dot.Rdbms.Sqlite.dll";
        var dbResourceName = "Dot.Rdbms.Sqlite.Resources.Blank.dotdb3";
        var assembly = System.Reflection.Assembly.LoadFrom(sqliteAssemblyPath);
        
        using (var resource = assembly.GetManifestResourceStream(dbResourceName))
        {
          using (var output = System.IO.File.OpenWrite(destLocation.FullName))
          {
            Information("Copying Blank db to temp folder.");
            resource.CopyTo(output);
          }
        }
      }

    });


Task("Default")
    .IsDependentOn("Information")
    .IsDependentOn("Restore")
    .IsDependentOn("Clean")
    .IsDependentOn("Build")
    .IsDependentOn("CodeSign")
    .IsDependentOn("BlankDbEnsurer")
    .IsDependentOn("Test")
    .IsDependentOn("Package")
    .IsDependentOn("Publish")
    .Does(() =>
{
    Information("Build successfully completed");
});

// Run Tasks
RunTarget(target);

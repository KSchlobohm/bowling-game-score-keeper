# .NET 8.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that an .NET 8.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 8.0 upgrade.
3. Upgrade BowlingGame\BowlingGame.csproj
4. Upgrade BowlingGame.Specs\BowlingGame.Specs.csproj
5. Run unit tests to validate upgrade in the projects listed below:
  - BowlingGame.Specs\BowlingGame.Specs.csproj

## Settings

This section contains settings and data used by execution steps.

### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update in projects that reference them.

| Package Name                        | Current Version | New Version | Description                         |
|:------------------------------------|:---------------:|:-----------:|:------------------------------------|
| SpecFlow                            | 3.9.8           | 3.9.74      | Deprecated                          |
| SpecFlow.Tools.MsBuild.Generation   | 3.9.8           | 3.9.74      | Deprecated                          |
| SpecFlow.xUnit                      | 3.9.8           | 3.9.74      | Deprecated                          |

### Project upgrade details
This section contains details about each project upgrade and modifications that need to be done in the project.

#### BowlingGame\BowlingGame.csproj modifications

Project properties changes:
  - Target framework should be changed from `net6.0` to `net8.0`

#### BowlingGame.Specs\BowlingGame.Specs.csproj modifications

Project properties changes:
  - Target framework should be changed from `net6.0` to `net8.0`

NuGet packages changes:
  - SpecFlow should be updated from `3.9.8` to `3.9.74` (*deprecated*)
  - SpecFlow.Tools.MsBuild.Generation should be updated from `3.9.8` to `3.9.74` (*deprecated*)
  - SpecFlow.xUnit should be updated from `3.9.8` to `3.9.74` (*deprecated*)

# .NET 8.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that an .NET 8.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 8.0 upgrade.
3. Upgrade BowlingGame\BowlingGame.csproj
4. Migrate BowlingGame.Specs\BowlingGame.Specs.csproj from SpecFlow to Reqnroll (using custom migration prompt)
5. Run unit tests to validate upgrade in the projects listed below:
  - BowlingGame.Specs\BowlingGame.Specs.csproj

## Settings

This section contains settings and data used by execution steps.

### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update in projects that reference them.

| Package Name                        | Current Version | New Version | Description                         |
|:------------------------------------|:---------------:|:-----------:|:------------------------------------|
| SpecFlow                            | 3.9.8           | Remove      | Migrate to Reqnroll.xUnit          |
| SpecFlow.Tools.MsBuild.Generation   | 3.9.8           | Remove      | Replaced by Reqnroll MSBuild       |
| SpecFlow.xUnit                      | 3.9.8           | Remove      | Migrate to Reqnroll.xUnit          |

### Project upgrade details
This section contains details about each project upgrade and modifications that need to be done in the project.

#### BowlingGame\BowlingGame.csproj modifications

Project properties changes:
  - Target framework should be changed from `net6.0` to `net8.0`

#### BowlingGame.Specs\BowlingGame.Specs.csproj modifications

Project properties changes:
  - Target framework should be changed from `net6.0` to `net8.0`

**Strategic Migration: SpecFlow to Reqnroll**
Following the custom migration prompt found in `.github\upgrades\prompts\custom_prompt_migrate_specflow_to_reqnroll.md`:

Migration approach: **Compatibility Package path** (fastest, minimal code changes)
  - Remove all SpecFlow packages: SpecFlow, SpecFlow.Tools.MsBuild.Generation, SpecFlow.xUnit
  - Add Reqnroll.xUnit package (matches current xUnit test framework)
  - Add Reqnroll.SpecFlowCompatibility package (maintains TechTalk.SpecFlow usings via shim)
  - Keep existing `TechTalk.SpecFlow` namespace usings (compatibility layer handles this)
  - Validate configuration compatibility (specflow.json continues to work)

Benefits of this approach:
  - Minimal code changes required
  - Faster migration path
  - Maintains existing step definitions and bindings
  - Future-proof migration to actively maintained Reqnroll framework
  - Better .NET 8.0 compatibility than deprecated SpecFlow v3.9.x

Feature upgrades:
  - SpecFlow to Reqnroll migration using compatibility package approach
  - Maintains existing test framework (xUnit) integration
  - Configuration compatibility maintained
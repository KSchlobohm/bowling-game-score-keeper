# .NET 8.0 Upgrade Report

## Project target framework modifications

| Project name                                   | Old Target Framework    | New Target Framework         | Commits                   |
|:-----------------------------------------------|:-----------------------:|:----------------------------:|---------------------------|
| BowlingGame\BowlingGame.csproj                 |   net6.0                | net8.0                       | c7520fb7, ef6bd49d        |
| BowlingGame.Specs\BowlingGame.Specs.csproj     |   net6.0                | net8.0                       | c7520fb7, ef6bd49d        |

## NuGet Packages

| Package Name                        | Old Version | New Version | Commit Id                                 |
|:------------------------------------|:-----------:|:-----------:|-------------------------------------------|
| SpecFlow                            |   3.9.8     |  Removed    | ef6bd49d                                  |
| SpecFlow.Tools.MsBuild.Generation   |   3.9.8     |  Removed    | ef6bd49d                                  |
| SpecFlow.xUnit                      |   3.9.8     |  Removed    | ef6bd49d                                  |
| Reqnroll.xUnit                      |   -         |  2.1.0      | ef6bd49d                                  |
| Reqnroll.SpecFlowCompatibility      |   -         |  2.1.0      | ef6bd49d                                  |

## All commits

| Commit ID              | Description                                |
|:-----------------------|:-------------------------------------------|
| c7520fb7               | Commit upgrade plan                        |
| ef6bd49d               | SpecFlow to Reqnroll migration completed successfully using compatibility package approach. All packages updated, build succeeded, feature files working correctly. |

## Project feature upgrades

Contains summary of modifications made to the project assets during different upgrade stages.

### BowlingGame\BowlingGame.csproj

Here is what changed for the project during upgrade:

- Target framework upgraded from net6.0 to net8.0
- No additional features required upgrade for this project

### BowlingGame.Specs\BowlingGame.Specs.csproj

Here is what changed for the project during upgrade:

- Target framework upgraded from net6.0 to net8.0
- **SpecFlow to Reqnroll Strategic Migration Completed**: Successfully migrated from deprecated SpecFlow v3.9.8 to actively maintained Reqnroll framework using compatibility package approach
  - Removed all deprecated SpecFlow packages (SpecFlow, SpecFlow.Tools.MsBuild.Generation, SpecFlow.xUnit)
  - Added Reqnroll.xUnit v2.1.0 for xUnit test framework integration
  - Added Reqnroll.SpecFlowCompatibility v2.1.0 to maintain existing TechTalk.SpecFlow namespace usings
  - Maintained all existing step definitions and bindings without code changes
  - Reqnroll MSBuild generator successfully handles .feature file code generation
  - All 6 tests (3 BDD scenarios + 3 unit tests) pass successfully

## Test Results

- **Total Tests Run**: 6
- **Passed**: 6 ✅
- **Failed**: 0 ✅  
- **Skipped**: 0 ✅
- **Test Success Rate**: 100%

## Next steps

- **Migration Complete**: The solution has been successfully upgraded to .NET 8.0 with modern, actively maintained dependencies
- **Future Considerations**: Consider migrating from compatibility package to full Reqnroll namespace approach in the future for a cleaner codebase (optional)
- **Verify CI/CD**: Ensure build agents can restore Reqnroll packages and run tests successfully
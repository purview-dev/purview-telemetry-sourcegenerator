# Variables
ROOT_FOLDER = src/
SOLUTION_FILE = $(ROOT_FOLDER)Purview.Telemetry.SourceGenerator.sln
TEST_PROJECT = $(ROOT_FOLDER)Purview.Telemetry.SourceGenerator.sln
CONFIGURATION = Release

PACK_VERSION = 0.0.13-prerelease
ARTIFACT_FOLDER = p:/sync-projects/.local-nuget/

# Targets
build:
	dotnet build $(SOLUTION_FILE) --configuration $(CONFIGURATION)

test:
	dotnet test $(TEST_PROJECT) --configuration $(CONFIGURATION)

pack:
	dotnet pack -c $(CONFIGURATION) -o $(ARTIFACT_FOLDER) $(ROOT_FOLDER)Purview.Telemetry.SourceGenerator/Purview.Telemetry.SourceGenerator.csproj --property:Version=$(PACK_VERSION) --include-symbols

format:
	dotnet format $(ROOT_FOLDER)

act:
	act -P ubuntu-latest=-self-hosted

.PHONY: build test

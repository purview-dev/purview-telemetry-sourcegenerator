include .build/common.mk

# Variables
ROOT_FOLDER = src/
SOLUTION_FILE = $(ROOT_FOLDER)Purview.Telemetry.SourceGenerator.sln
TEST_PROJECT = $(ROOT_FOLDER)Purview.Telemetry.SourceGenerator.sln
CONFIGURATION = Release

PACK_VERSION = 1.1.0
ARTIFACT_FOLDER = p:/sync-projects/.local-nuget/

# Targets
build: ## Builds the project.
	dotnet build $(SOLUTION_FILE) --configuration $(CONFIGURATION)

test: ## Runs the tests for the project.
	dotnet test $(TEST_PROJECT) --configuration $(CONFIGURATION)

pack: ## Packs the project into a nuget package using PACK_VERSION argument.
	dotnet pack -c $(CONFIGURATION) -o $(ARTIFACT_FOLDER) $(ROOT_FOLDER)Purview.Telemetry.SourceGenerator/Purview.Telemetry.SourceGenerator.csproj --property:Version=$(PACK_VERSION) --include-symbols

format: ## Formats the code according to the rules of the src/.editorconfig file.
	dotnet format $(ROOT_FOLDER)

act:
	act -P ubuntu-latest=-self-hosted

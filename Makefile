include .build/common.mk

# Variables
ROOT_FOLDER = ./src/
SOLUTION_FILE = $(ROOT_FOLDER)Purview.Telemetry.SourceGenerator.slnx
TEST_PROJECT = $(ROOT_FOLDER)Purview.Telemetry.SourceGenerator.slnx
CONFIGURATION = Release

SAMPLE_SOLUTION_FILE = ./samples/SampleApp/SampleApp.slnx

PACK_VERSION := $(shell bun -e 'console.log(require("./package.json").version)')
ARTIFACT_FOLDER = p:/sync-projects/.local-nuget/

# Common variables
COLOUR_RESET  = \033[0m

COLOUR_BLACK  = \033[30m
COLOUR_RED    = \033[31m
COLOUR_GREEN  = \033[32m
COLOUR_ORANGE = \033[33m
COLOUR_BLUE   = \033[34m
COLOUR_PURPLE = \033[35m
COLOUR_CYAN   = \033[36m
COLOUR_WHITE  = \033[37m

COLOUR_GREY   = \033[90m

# Targets
build: ## Builds the project.
	@echo -e "Building $(COLOUR_BLUE)$(SOLUTION_FILE)$(COLOUR_RESET) with $(COLOUR_ORANGE)$(CONFIGURATION)$(COLOUR_RESET)..."
	@dotnet build $(SOLUTION_FILE) --configuration $(CONFIGURATION)

test: ## Runs the tests for the project.
	@echo -e "Running tests for $(COLOUR_BLUE)$(TEST_PROJECT)$(COLOUR_RESET) with $(COLOUR_ORANGE)$(CONFIGURATION)$(COLOUR_RESET)..."
	@dotnet test $(TEST_PROJECT) --configuration $(CONFIGURATION)

pack: update-version build-pack ## Packs the project into a nuget package using PACK_VERSION argument.
	
build-pack:
	@echo -e "Packing $(COLOUR_BLUE)Source Generator$(COLOUR_RESET) with $(COLOUR_ORANGE)$(PACK_VERSION)$(COLOUR_RESET)..."
	@dotnet pack -c $(CONFIGURATION) -o $(ARTIFACT_FOLDER) $(ROOT_FOLDER)Purview.Telemetry.SourceGenerator/Purview.Telemetry.SourceGenerator.csproj --property:Version=$(PACK_VERSION) --include-symbols

format: ## Formats the code according to the rules of the src/.editorconfig file.
	@echo -e "Formatting $(COLOUR_BLUE)$(SOLUTION_FILE)$(COLOUR_RESET)..."
	@dotnet format $(ROOT_FOLDER)

act:
	@echo -e "Running $(COLOUR_BLUE)act$(COLOUR_RESET)..."
	act -P ubuntu-latest=-self-hosted

vs: ## Opens the project in Visual Studio.
	@echo -e "Opening $(COLOUR_BLUE)$(SOLUTION_FILE)$(COLOUR_RESET) in $(COLOUR_ORANGE)Visual Studio$(COLOUR_RESET)..."
	@start "$(SOLUTION_FILE)"

code: ## Opens the project in Visual Studio Code.
	@echo -e "Opening $(COLOUR_BLUE)Visual Studio Code$(COLOUR_RESET)..."
	@code .

vs-s: ## Opens the sample project in Visual Studio.
	@echo -e "Opening $(COLOUR_BLUE)$(SAMPLE_SOLUTION_FILE)$(COLOUR_RESET) in $(COLOUR_ORANGE)Visual Studio$(COLOUR_RESET)..."
	@start "$(SAMPLE_SOLUTION_FILE)"

version: ## Displays the current version of the project.
	@echo -e "Current Version: $(COLOUR_GREEN)$(PACK_VERSION)$(COLOUR_RESET)"
	
update-version: ## Update related samples and docs to new version.
	@echo -e "Update related samples and docs to new version: $(COLOUR_GREEN)$(PACK_VERSION)$(COLOUR_RESET)"
	@bun .build/update-version.js

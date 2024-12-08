# Define the default target
.DEFAULT_GOAL := help

# Include this in your Makefile using 'include common.mk'

# ANSI colour codes for formatting (used for coloured output in help)
COLOUR_TARGET = \033[1;34m   # Blue, bold
COLOUR_RESET = \033[0m       # Reset colour

# Automatically calculate the maximum target width (without colour)
TARGET_WIDTH := $(shell grep -Eh '^[a-zA-Z_-]+:.*?##' $(MAKEFILE_LIST) | awk -F':' '{print length($$1)}' | sort -nr | head -1)

# Collect all targets from the Makefile and included files
PHONY_TARGETS := $(shell grep -Eh '^[a-zA-Z_-]+:.*?##' $(MAKEFILE_LIST) | awk -F':' '{print $$1}') # OR ... '{print $$1}' | sort -u)

# Declare all the targets as phony
.PHONY: $(PHONY_TARGETS)

# Help target that reads targets from all Makefiles listed in MAKEFILE_LIST
help:
	@echo "Available targets:"
	@grep -Eh '^[a-zA-Z_-]+:.*?##' $(MAKEFILE_LIST) | \
	#sort -u | \
	awk -v COLOUR_target="$(COLOUR_TARGET)" -v COLOUR_reset="$(COLOUR_RESET)" -v width=$(TARGET_WIDTH) '\
	BEGIN {FS = ":.*## "}; \
	{ \
		target = $$1; \
		description = $$2; \
		printf "%s%-*s%s %s\n", COLOUR_target, width, target, COLOUR_reset, description \
	}'

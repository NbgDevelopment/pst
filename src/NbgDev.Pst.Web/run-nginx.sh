#!/bin/sh

# Try to copy the stage-specific appsettings file if it exists
# Otherwise, create an empty JSON object (settings from environment variables)

# Sanitize stage variable to prevent path traversal attacks
# Only allow alphanumeric characters, hyphens, and underscores
# Ensure it doesn't start with a hyphen or period to prevent path issues
if [ -n "${stage}" ]; then
    sanitized_stage=$(echo "${stage}" | sed 's/[^a-zA-Z0-9_-]//g' | sed 's/^[-.]*//')
    if [ -z "${sanitized_stage}" ]; then
        echo "Warning: stage variable contained only invalid characters"
        unset stage
    elif [ "${sanitized_stage}" != "${stage}" ]; then
        echo "Warning: stage variable contained invalid characters and was sanitized"
    fi
    stage="${sanitized_stage}"
fi

if [ -n "${stage}" ] && [ -f "/var/www/web/appsettings.${stage}.json" ]; then
    echo "Using stage-specific configuration: appsettings.${stage}.json"
    if ! cp "/var/www/web/appsettings.${stage}.json" "/var/www/web/appsettings.backend.json"; then
        echo "ERROR: Failed to copy stage-specific configuration file"
        exit 1
    fi
else
    echo "No stage-specific configuration found, creating config with API URL from environment variables"
    # Create appsettings.backend.json with API URL from environment variable
    if [ -n "${API_URL}" ]; then
        if ! echo "{\"PstApi:ApiUrl\":\"${API_URL}\"}" > "/var/www/web/appsettings.backend.json"; then
            echo "ERROR: Failed to create backend configuration file"
            exit 1
        fi
    else
        echo "Warning: API_URL environment variable not set, creating empty config"
        if ! echo '{}' > "/var/www/web/appsettings.backend.json"; then
            echo "ERROR: Failed to create backend configuration file"
            exit 1
        fi
    fi
fi

nginx -g "daemon off;"
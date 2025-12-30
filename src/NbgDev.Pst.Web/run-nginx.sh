#!/bin/sh

# Try to copy the stage-specific appsettings file if it exists
# Otherwise, fall back to the default appsettings.json
# If no config files exist, create an empty JSON object (settings from environment variables)
if [ -n "${stage}" ] && [ -f "/var/www/web/appsettings.${stage}.json" ]; then
    echo "Using stage-specific configuration: appsettings.${stage}.json"
    cp "/var/www/web/appsettings.${stage}.json" "/var/www/web/appsettings.backend.json"
elif [ -f "/var/www/web/appsettings.json" ]; then
    echo "Stage-specific configuration not found, using default: appsettings.json"
    cp "/var/www/web/appsettings.json" "/var/www/web/appsettings.backend.json"
else
    echo "No configuration file found, creating empty config (settings from environment variables)"
    echo '{}' > "/var/www/web/appsettings.backend.json"
fi

nginx -g "daemon off;"
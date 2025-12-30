#!/bin/sh

# Try to copy the stage-specific appsettings file if it exists
# Otherwise, fall back to the default appsettings.json
if [ -n "${stage}" ] && [ -f "/var/www/web/appsettings.${stage}.json" ]; then
    echo "Using stage-specific configuration: appsettings.${stage}.json"
    cp "/var/www/web/appsettings.${stage}.json" "/var/www/web/appsettings.backend.json"
elif [ -f "/var/www/web/appsettings.json" ]; then
    echo "Stage-specific configuration not found, using default: appsettings.json"
    cp "/var/www/web/appsettings.json" "/var/www/web/appsettings.backend.json"
else
    echo "ERROR: No configuration file found. Cannot start application."
    exit 1
fi

nginx -g "daemon off;"
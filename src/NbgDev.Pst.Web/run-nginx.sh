#!/bin/sh

# Try to copy the stage-specific appsettings file if it exists
# Otherwise, create an empty JSON object (settings from environment variables)
if [ -n "${stage}" ] && [ -f "/var/www/web/appsettings.${stage}.json" ]; then
    echo "Using stage-specific configuration: appsettings.${stage}.json"
    cp "/var/www/web/appsettings.${stage}.json" "/var/www/web/appsettings.backend.json"
else
    echo "No stage-specific configuration found, creating empty config (settings from environment variables)"
    echo '{}' > "/var/www/web/appsettings.backend.json"
fi

nginx -g "daemon off;"
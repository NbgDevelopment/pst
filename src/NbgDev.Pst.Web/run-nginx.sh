cp /var/www/web/appsettings.$stage.json /var/www/web/appsettings.backend.json

nginx -g "daemon off;"
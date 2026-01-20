# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NbgDev.Pst.Web/NbgDev.Pst.Web.csproj", "NbgDev.Pst.Web/"]
RUN dotnet restore "./NbgDev.Pst.Web/NbgDev.Pst.Web.csproj"
COPY . .
WORKDIR "/src/NbgDev.Pst.Web"
RUN dotnet build "./NbgDev.Pst.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NbgDev.Pst.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM nginx:alpine AS final
WORKDIR /var/www/web
COPY --from=publish /app/publish/wwwroot .
COPY NbgDev.Pst.Web/nginx.conf /etc/nginx/nginx.conf

COPY ["NbgDev.Pst.Web/run-nginx.sh", "/usr/share/nginx/run-nginx.sh"]
RUN chmod +x /usr/share/nginx/run-nginx.sh

EXPOSE 80

CMD ["/usr/share/nginx/run-nginx.sh"]
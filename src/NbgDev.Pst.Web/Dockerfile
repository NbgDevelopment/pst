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

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NbgDev.Pst.Web.dll"]
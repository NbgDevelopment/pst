using System.Text.Json;

namespace NbgDev.Pst.AppHost;

public static class BlazorWebAssemblyProjectExtensions
{
    public static IResourceBuilder<ProjectResource> AddWebAssemblyProject(
        this IResourceBuilder<ProjectResource> projectBuilder, string name,
        IResourceBuilder<ProjectResource> api,
        string apiUrlConfigurationKey = "ApiUrl")
    {
        var project = projectBuilder.Resource.GetProjectMetadata();
        var dir = Path.GetDirectoryName(project.ProjectPath);
        ArgumentNullException.ThrowIfNull(dir);

        var wwwroot = Path.Combine(dir, "wwwroot");
        if (!Directory.Exists(wwwroot))
        {
            Directory.CreateDirectory(wwwroot);
        }
        var file = Path.Combine(wwwroot, "appsettings.json");

        projectBuilder = projectBuilder.WithEnvironment(ctx =>
        {
            if (api.Resource.TryGetEndpoints(out var end))
            {
                var endpoints = end
                .Where(e => e.AllocatedEndpoint is not null)
                .Select(e => e.AllocatedEndpoint!)
                .ToList();

                if (endpoints.Count > 0)
                {
                    var apiUrl = endpoints.First().UriString;

                    var fileContent = File.Exists(file)
                        ? File.ReadAllText(file)
                        : "{}";

                    Dictionary<string, object>? dict = string.IsNullOrWhiteSpace(fileContent)
                        ? new Dictionary<string, object>()
                        : JsonSerializer.Deserialize<Dictionary<string, object>>(fileContent!);
                    ArgumentNullException.ThrowIfNull(dict);

                    dict[apiUrlConfigurationKey] = apiUrl;

                    JsonSerializerOptions opt = new JsonSerializerOptions(JsonSerializerOptions.Default)
                    {
                        WriteIndented = true
                    };
                    File.WriteAllText(file, JsonSerializer.Serialize(dict, opt));

                    ctx.EnvironmentVariables[apiUrlConfigurationKey] = apiUrl;
                }
            }
        });

        return projectBuilder;

    }
}

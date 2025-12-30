# GitHub Copilot Instructions for PST (Project Setup Tool)

## Project Overview

PST (Project Setup Tool) is a .NET 9.0 application that helps developers set up and manage project configurations. The application is built using .NET Aspire for cloud-native development and uses Azure services for storage and messaging.

## Architecture

### Project Structure

The solution follows a **modular, multi-project architecture** organized into functional domains:

- **NbgDev.Pst.AppHost** - .NET Aspire orchestrator (entry point for development)
- **NbgDev.Pst.ServiceDefaults** - Shared Aspire service defaults
- **Frontend** domain:
  - **NbgDev.Pst.Web** - Blazor web application
  - **NbgDev.Pst.App** - Client application
- **Backend** domain:
  - **NbgDev.Pst.Api** - Main ASP.NET Core Web API application
  - **NbgDev.Pst.Api.Client** - API client library
  - **NbgDev.Pst.Processing** - Background processing service
  - **NbgDev.Pst.Events.Contract** - Event contracts and messaging
- **Projects** domain:
  - **NbgDev.Pst.Projects.Contract** - Contracts/interfaces for project management
  - **NbgDev.Pst.Projects.AzureTable** - Azure Table Storage implementation

### Design Patterns

1. **CQRS with MediatR**
   - Use MediatR `IRequest<TResponse>` for queries and commands
   - Place request classes in `Requests/` folders within Contract projects
   - Implement handlers in implementation projects
   - Example: `GetProjectsRequest` → `GetProjectsRequestHandler`

2. **Dependency Injection**
   - Use constructor injection with primary constructors (C# 12)
   - Register services via extension methods in `Bootstrap.cs` or similar files
   - Use `IOptions<T>` pattern for configuration
   - Prefer interface-based abstractions over concrete implementations

3. **Contract-Implementation Separation**
   - Define interfaces and models in `.Contract` projects
   - Implement functionality in separate implementation projects
   - Internal implementations, public contracts

4. **Event-Driven Architecture**
   - Use Azure Queue Storage for asynchronous messaging
   - Define events in `Events.Contract` project
   - Implement event handlers in relevant services
   - Use `IEventPublisher` for publishing events

5. **.NET Aspire Patterns**
   - Use `AddServiceDefaults()` in all services
   - Configure Azure resources through Aspire
   - Use `MapDefaultEndpoints()` for health checks and telemetry

## Coding Conventions

### C# Style

- **Target Framework**: .NET 9.0
- **Language Version**: C# 12 with latest features
- **Nullable Reference Types**: Enabled (`<Nullable>enable</Nullable>`)
- **Implicit Usings**: Enabled (`<ImplicitUsings>enable</ImplicitUsings>`)

### Code Patterns

1. **Primary Constructors**
   ```csharp
   internal class ProjectService(IProjectRepository repository, ILogger<ProjectService> logger) : IProjectService
   ```

2. **Required Properties**
   ```csharp
   public required string Id { get; set; }
   public required string Name { get; set; }
   ```

3. **Init-Only Properties**
   ```csharp
   public string Type { get; init; }
   ```

4. **File-Scoped Namespaces**
   ```csharp
   namespace NbgDev.Pst.Projects.Contract;
   
   public class Project { }
   ```

5. **Pattern Matching and Null-Coalescing**
   ```csharp
   if (project?.Id is null) { return; }
   return result ?? NotFound();
   ```

6. **Collection Expressions and LINQ**
   ```csharp
   return projects.Select(p => MapToDto(p)).ToArray();
   ```

### Naming Conventions

- **Projects**: `NbgDev.Pst.<Domain>.<Technology>` (e.g., `NbgDev.Pst.Projects.AzureTable`)
- **Contracts**: `NbgDev.Pst.<Domain>.Contract`
- **Interfaces**: Prefix with `I` (e.g., `IProjectRepository`, `IEventPublisher`)
- **Internal Classes**: Mark implementation classes as `internal` when they implement public interfaces
- **Request/Response**: Suffix with `Request` or `Response` (e.g., `GetProjectsRequest`)
- **Handlers**: Suffix with `Handler` (e.g., `GetProjectsRequestHandler`)
- **Options**: Suffix configuration classes with `Options`
- **DTOs**: Place in `Dtos/` folders, suffix with `Dto` when needed

### Dependency Registration

Use extension methods for service registration:

```csharp
public static class BootstrapProjectsAzureTable
{
    public static IServiceCollection AddProjectsAzureTable(this IServiceCollection services)
    {
        services.AddSingleton<IProjectRepository, ProjectRepository>();
        services.AddTransient<IRequestHandler<GetProjectsRequest, IReadOnlyList<Project>>, GetProjectsRequestHandler>();
        return services;
    }
}
```

### Configuration

- Use `IOptions<T>` pattern for strongly-typed configuration
- Configuration sections match the domain or service name
- Store sensitive data externally (use user secrets for development, environment variables for production)
- Leverage .NET Aspire configuration patterns

### Logging

- Use **structured logging**
- Inject `ILogger<T>` via constructor
- Use structured logging with message templates
- .NET Aspire provides built-in telemetry and logging

### Package Management

- Package versions defined directly in project files
- Key dependencies:
  - **MediatR** - CQRS pattern
  - **Aspire.Azure.Data.Tables** - Azure Table Storage integration
  - **Aspire.Azure.Storage.Queues** - Azure Queue Storage integration
  - **Microsoft.Identity.Web** - Authentication and authorization
  - **NSwag.AspNetCore** - OpenAPI/Swagger generation

## Best Practices

1. **Async/Await**
   - Use `async`/`await` consistently
   - Cancel operations with `CancellationToken`
   - For fire-and-forget tasks, use appropriate patterns with error handling

2. **Error Handling**
   - Log exceptions with structured logging
   - Use `try-catch` for expected exceptions
   - Return appropriate HTTP status codes in controllers

3. **Hosted Services**
   - Implement `IHostedService` for background tasks (e.g., `EventProcessor`)
   - Register as singleton or scoped as appropriate
   - Handle startup and shutdown gracefully

4. **Event Processing**
   - Implement `IEventHandler` for handling specific events
   - Use `IEventPublisher` to publish events to Azure Queue Storage
   - Process events asynchronously in background services

5. **Controllers**
   - Use `[Route("api/[controller]")]` attribute routing
   - Inject `IMediator` for CQRS operations
   - Return `ActionResult<T>` for type-safe responses
   - Use `RequireAuthorization()` for secured endpoints

6. **Authentication**
   - Use Microsoft Identity Web with JWT Bearer authentication
   - Configure Azure AD in `appsettings.json` under `AzureAd` section
   - Apply authorization policies as needed

7. **CORS**
   - Configure CORS policy for frontend origins
   - Use configuration to specify allowed origins
   - Apply CORS policy in middleware pipeline

## Development Workflow

1. **Build**: `dotnet build NbgDev.Pst.sln` from the `src` directory
2. **Run**: Use .NET Aspire AppHost - `dotnet run --project NbgDev.Pst.AppHost`
3. **Docker**: Use the provided `Dockerfile` for containerization
4. **OpenAPI**: Available in development mode for API exploration (NSwag)

## Comments

- **Minimal Comments**: Code should be self-documenting through clear naming
- **When to Comment**:
  - Complex business logic
  - Non-obvious workarounds
  - Public API documentation (if needed)
- **XML Documentation**: Not extensively used; prefer clear code over comments

## Language

- **Code and Comments**: English
- **User-Facing Messages**: English

---

## Personal GitHub Copilot Instructions

This section contains personal preferences and instructions for working with GitHub Copilot on this project.

### Code Generation Preferences

- **Always use the latest C# features**: Leverage C# 12 features including primary constructors, collection expressions, and pattern matching
- **Prefer immutability**: Use `init` over `set` when possible, use readonly fields
- **Be explicit with nullability**: Always handle null cases explicitly, avoid null-forgiving operator unless absolutely necessary
- **Keep it DRY**: Don't repeat yourself - extract common patterns into reusable methods or classes
- **Single Responsibility**: Each class/method should have one clear purpose
- **.NET Aspire first**: Use Aspire patterns and integrations for cloud services
- **Cloud-native patterns**: Design with cloud deployment in mind

### Testing Preferences

- **Test naming**: Use descriptive test method names that explain the scenario and expected outcome
- **Arrange-Act-Assert**: Follow AAA pattern in unit tests
- **Test one thing**: Each test should verify a single behavior
- **Use meaningful test data**: Avoid magic numbers/strings, use descriptive constants or variables

### Documentation Preferences

- **Self-documenting code first**: Write clear code that doesn't need comments
- **Update documentation**: When changing functionality, update related documentation
- **Keep it current**: Remove outdated comments rather than leaving them in

### Review and Quality

- **Security first**: Always consider security implications (input validation, sanitization, authentication)
- **Performance awareness**: Be mindful of performance implications (N+1 queries, unnecessary allocations)
- **Error handling**: Don't swallow exceptions, log appropriately, fail fast when appropriate
- **Backwards compatibility**: Consider impact on existing functionality when making changes
- **Azure best practices**: Follow Azure service best practices for Table Storage, Queue Storage, etc.

### Communication Style

- **Clear commit messages**: Use descriptive commit messages following conventional commits format
- **Explain why, not what**: In comments and documentation, explain the reasoning, not just the implementation
- **Ask when uncertain**: If requirements are unclear, ask for clarification rather than making assumptions

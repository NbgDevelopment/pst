using MediatR;
using Microsoft.AspNetCore.Mvc;
using NbgDev.Pst.Api.Dtos;
using NbgDev.Pst.Api.Services;
using NbgDev.Pst.Events.Contract.Models;
using NbgDev.Pst.Projects.Contract.Models;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController(IMediator mediator, IEventPublisher eventPublisher) : ControllerBase
{
    [HttpGet]
    public async Task<IReadOnlyList<ProjectDto>> Get()
    {
        var projects = await mediator.Send(new GetProjectsRequest());
        return projects
            .Select(Map)
            .ToArray();
    }

    [HttpGet("/{id}")]
    public async Task<ActionResult<ProjectDto>> Get(Guid id)
    {
        var project = await mediator.Send(new GetProjectRequest(id));

        if (project is null)
        {
            return NotFound();
        }

        return Ok(Map(project));
    }

    [HttpPut]
    public async Task<ActionResult<ProjectDto>> Create([FromBody]CreateProjectRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return ValidationProblem("Project name may not be null or empty");
        }

        if (string.IsNullOrWhiteSpace(request.ShortName))
        {
            return ValidationProblem("Project short name may not be null or empty");
        }

        var project = await mediator.Send(request);

        await eventPublisher.PublishAsync(new ProjectCreatedEvent
        {
            EventType = nameof(ProjectCreatedEvent),
            ProjectId = project.Id,
            ProjectName = project.Name,
            ShortName = project.ShortName
        });

        return Ok(Map(project));
    }

    private static ProjectDto Map(Project project)
    {
        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            ShortName = project.ShortName
        };
    }
}

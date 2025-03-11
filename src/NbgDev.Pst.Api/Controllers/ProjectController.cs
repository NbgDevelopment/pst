using MediatR;
using Microsoft.AspNetCore.Mvc;
using NbgDev.Pst.Api.Dtos;
using NbgDev.Pst.Projects.Contract.Models;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IReadOnlyList<ProjectDto>> Get()
    {
        var projects = await mediator.Send(new GetProjectsRequest());
        return projects
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name
            })
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

        return Ok(new ProjectDto
        {
            Id = project.Id,
            Name = project.Name
        });
    }

    [HttpPut("/{name}")]
    public async Task<ProjectDto> Create(string name)
    {
        var project = await mediator.Send(new CreateProjectRequest(name));

        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name
        };
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using NbgDev.Pst.Api.Dtos;
using NbgDev.Pst.Api.Services;
using NbgDev.Pst.Events.Contract.Models;
using NbgDev.Pst.Projects.Contract.Models;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Api.Controllers;

[Route("api/project/{projectId}/roles")]
[ApiController]
public class ProjectRoleController(IMediator mediator, IEventPublisher eventPublisher) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RoleDto>>> GetRoles(Guid projectId)
    {
        var roles = await mediator.Send(new GetRolesRequest(projectId));
        return Ok(roles.Select(Map).ToArray());
    }

    [HttpPost]
    public async Task<ActionResult<RoleDto>> CreateRole(Guid projectId, [FromBody] CreateRoleDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return ValidationProblem("Role name may not be null or empty");
        }

        if (string.IsNullOrWhiteSpace(dto.Description))
        {
            return ValidationProblem("Role description may not be null or empty");
        }

        var role = await mediator.Send(new CreateRoleRequest(projectId, dto.Name, dto.Description));

        await eventPublisher.PublishAsync(new ProjectRoleCreatedEvent
        {
            EventType = nameof(ProjectRoleCreatedEvent),
            RoleId = role.Id,
            ProjectId = role.ProjectId,
            Name = role.Name,
            Description = role.Description
        });

        return Ok(Map(role));
    }

    [HttpPut("{roleId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoleDto>> UpdateRole(Guid projectId, Guid roleId, [FromBody] UpdateRoleDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return ValidationProblem("Role name may not be null or empty");
        }

        if (string.IsNullOrWhiteSpace(dto.Description))
        {
            return ValidationProblem("Role description may not be null or empty");
        }

        var role = await mediator.Send(new UpdateRoleRequest(roleId, dto.Name, dto.Description));

        if (role == null)
        {
            return NotFound();
        }

        return Ok(Map(role));
    }

    [HttpDelete("{roleId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteRole(Guid projectId, Guid roleId)
    {
        var deleted = await mediator.Send(new DeleteRoleRequest(roleId));

        if (!deleted)
        {
            return NotFound();
        }

        await eventPublisher.PublishAsync(new ProjectRoleDeletedEvent
        {
            EventType = nameof(ProjectRoleDeletedEvent),
            RoleId = roleId,
            ProjectId = projectId
        });

        return NoContent();
    }

    private static RoleDto Map(Role role)
    {
        return new RoleDto
        {
            Id = role.Id,
            ProjectId = role.ProjectId,
            Name = role.Name,
            Description = role.Description
        };
    }
}

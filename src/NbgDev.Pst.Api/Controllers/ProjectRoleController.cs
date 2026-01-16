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

        var role = await mediator.Send(new CreateRoleRequest(projectId, dto.Name));

        await eventPublisher.PublishAsync(new ProjectRoleCreatedEvent
        {
            EventType = nameof(ProjectRoleCreatedEvent),
            RoleId = role.Id,
            ProjectId = role.ProjectId,
            Name = role.Name
        });

        return Ok(Map(role));
    }

    [HttpDelete("{roleId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteRole(Guid projectId, Guid roleId)
    {
        await mediator.Send(new DeleteRoleRequest(roleId));

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
            Name = role.Name
        };
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using NbgDev.Pst.Api.Dtos;
using NbgDev.Pst.Api.Services;
using NbgDev.Pst.Events.Contract.Models;
using NbgDev.Pst.Projects.Contract.Models;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Api.Controllers;

[Route("api/project/role/{roleId}/members")]
[ApiController]
public class ProjectRoleMemberController(IMediator mediator, IEventPublisher eventPublisher) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RoleMemberDto>>> GetMembers(Guid roleId)
    {
        var members = await mediator.Send(new GetRoleMembersRequest(roleId));
        return Ok(members.Select(Map).ToArray());
    }

    [HttpPost]
    public async Task<ActionResult<RoleMemberDto>> AddMember(Guid roleId, [FromBody] AddRoleMemberDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.UserId))
        {
            return ValidationProblem("User ID may not be null or empty");
        }

        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            return ValidationProblem("Email may not be null or empty");
        }

        var member = await mediator.Send(new AddRoleMemberRequest(
            roleId,
            dto.UserId,
            dto.FirstName,
            dto.LastName,
            dto.Email
        ));

        await eventPublisher.PublishAsync(new ProjectMemberAddedToRoleEvent
        {
            EventType = nameof(ProjectMemberAddedToRoleEvent),
            RoleId = member.RoleId,
            UserId = member.UserId,
            FirstName = member.FirstName,
            LastName = member.LastName,
            Email = member.Email
        });

        return Ok(Map(member));
    }

    [HttpDelete("{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RemoveMember(Guid roleId, string userId)
    {
        await mediator.Send(new RemoveRoleMemberRequest(roleId, userId));

        await eventPublisher.PublishAsync(new ProjectMemberRemovedFromRoleEvent
        {
            EventType = nameof(ProjectMemberRemovedFromRoleEvent),
            RoleId = roleId,
            UserId = userId
        });

        return NoContent();
    }

    private static RoleMemberDto Map(RoleMember member)
    {
        return new RoleMemberDto
        {
            RoleId = member.RoleId,
            UserId = member.UserId,
            FirstName = member.FirstName,
            LastName = member.LastName,
            Email = member.Email
        };
    }
}

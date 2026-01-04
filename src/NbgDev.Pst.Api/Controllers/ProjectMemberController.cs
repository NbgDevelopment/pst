using MediatR;
using Microsoft.AspNetCore.Mvc;
using NbgDev.Pst.Api.Dtos;
using NbgDev.Pst.Api.Services;
using NbgDev.Pst.Events.Contract.Models;
using NbgDev.Pst.Projects.Contract.Models;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Api.Controllers;

[Route("api/project/{projectId}/members")]
[ApiController]
public class ProjectMemberController(IMediator mediator, IEventPublisher eventPublisher) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProjectMemberDto>>> GetMembers(Guid projectId)
    {
        var members = await mediator.Send(new GetProjectMembersRequest(projectId));
        return Ok(members.Select(Map).ToArray());
    }

    [HttpPost]
    public async Task<ActionResult<ProjectMemberDto>> AddMember(Guid projectId, [FromBody] AddProjectMemberDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.UserId))
        {
            return ValidationProblem("User ID may not be null or empty");
        }

        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            return ValidationProblem("Email may not be null or empty");
        }

        var member = await mediator.Send(new AddProjectMemberRequest(
            projectId,
            dto.UserId,
            dto.FirstName,
            dto.LastName,
            dto.Email
        ));

        // Get project to retrieve GroupId
        var project = await mediator.Send(new GetProjectRequest(projectId));

        await eventPublisher.PublishAsync(new ProjectMemberAddedEvent
        {
            EventType = nameof(ProjectMemberAddedEvent),
            ProjectId = member.ProjectId,
            UserId = member.UserId,
            FirstName = member.FirstName,
            LastName = member.LastName,
            Email = member.Email,
            GroupId = project?.Group?.Id
        });

        return Ok(Map(member));
    }

    [HttpDelete("{userId}")]
    public async Task<ActionResult> RemoveMember(Guid projectId, string userId)
    {
        var removed = await mediator.Send(new RemoveProjectMemberRequest(projectId, userId));

        if (!removed)
        {
            return NotFound();
        }

        // Get project to retrieve GroupId
        var project = await mediator.Send(new GetProjectRequest(projectId));

        await eventPublisher.PublishAsync(new ProjectMemberRemovedEvent
        {
            EventType = nameof(ProjectMemberRemovedEvent),
            ProjectId = projectId,
            UserId = userId,
            GroupId = project?.Group?.Id
        });

        return NoContent();
    }

    private static ProjectMemberDto Map(ProjectMember member)
    {
        return new ProjectMemberDto
        {
            ProjectId = member.ProjectId,
            UserId = member.UserId,
            FirstName = member.FirstName,
            LastName = member.LastName,
            Email = member.Email
        };
    }
}

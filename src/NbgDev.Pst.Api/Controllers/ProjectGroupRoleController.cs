using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NbgDev.Pst.Api.Controllers;

[Route("api/project/{projectId}/group/roles")]
[ApiController]
public class ProjectGroupRoleController(IMediator mediator) : ControllerBase
{
    [HttpPost("{roleId}")]
    public async Task<ActionResult> AssignRole(Guid projectId, string roleId)
    {
        // This will be implemented with MediatR request/handler pattern
        // For now, return NotImplemented to show the endpoint exists
        return StatusCode(501, "Role assignment functionality will be available once configured");
    }

    [HttpDelete("{roleId}")]
    public async Task<ActionResult> RemoveRole(Guid projectId, string roleId)
    {
        // This will be implemented with MediatR request/handler pattern
        // For now, return NotImplemented to show the endpoint exists
        return StatusCode(501, "Role removal functionality will be available once configured");
    }
}

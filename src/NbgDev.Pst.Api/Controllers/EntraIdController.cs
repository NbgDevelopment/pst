using MediatR;
using Microsoft.AspNetCore.Mvc;
using NbgDev.Pst.Api.Dtos;
using NbgDev.Pst.Projects.Contract.Models;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Api.Controllers;

[Route("api/entraid")]
[ApiController]
public class EntraIdController(IMediator mediator) : ControllerBase
{
    [HttpGet("users/search")]
    public async Task<ActionResult<IReadOnlyList<EntraIdUserDto>>> SearchUsers([FromQuery] string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return Ok(Array.Empty<EntraIdUserDto>());
        }

        var users = await mediator.Send(new SearchEntraIdUsersRequest(searchTerm));
        return Ok(users.Select(Map).ToArray());
    }

    private static EntraIdUserDto Map(EntraIdUser user)
    {
        return new EntraIdUserDto
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            GivenName = user.GivenName,
            Surname = user.Surname,
            Mail = user.Mail
        };
    }
}

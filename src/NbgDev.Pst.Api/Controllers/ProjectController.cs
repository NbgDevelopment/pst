using Microsoft.AspNetCore.Mvc;
using NbgDev.Pst.Api.Dtos;

namespace NbgDev.Pst.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly List<ProjectDto> _projects = [
        new ProjectDto{ Id = Guid.Parse("9a522978-7615-452d-af57-aad52aa36f36"), Name = "Project 1" },
        new ProjectDto{ Id = Guid.Parse("00661810-a3bc-456c-bc89-18abfe540de2"), Name = "Project 2" }
    ];

    [HttpGet]
    public IReadOnlyList<ProjectDto> Get()
    {
        return _projects;
    }
}

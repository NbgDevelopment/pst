using NbgDev.Pst.App.Models;

namespace NbgDev.Pst.App.Services;

public class ProjectService : IProjectService
{
    private readonly List<Project> _projects = [
        new Project{ Id = Guid.Parse("9a522978-7615-452d-af57-aad52aa36f36"), Name = "Project 1" },
        new Project{ Id = Guid.Parse("00661810-a3bc-456c-bc89-18abfe540de2"), Name = "Project 2" }
    ];

    public IReadOnlyList<Project> GetProjects()
    {
        return _projects;
    }
}

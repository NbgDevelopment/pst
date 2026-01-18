using NbgDev.Pst.Projects.Contract.Mediator;
using NbgDev.Pst.Projects.Contract.Models;

namespace NbgDev.Pst.Projects.Contract.Requests;

public record AddProjectMemberRequest(
    Guid ProjectId,
    string UserId,
    string FirstName,
    string LastName,
    string? Email
) : IRequest<ProjectMember>;

using NbgDev.Pst.Projects.Contract.Mediator;
using NbgDev.Pst.Projects.Contract.Models;

namespace NbgDev.Pst.Projects.Contract.Requests;

public record GetProjectMembersRequest(Guid ProjectId) : IRequest<IReadOnlyList<ProjectMember>>;

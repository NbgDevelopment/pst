using NbgDev.Pst.Projects.Contract.Mediator;
using NbgDev.Pst.Projects.Contract.Models;

namespace NbgDev.Pst.Projects.Contract.Requests;

public record GetRoleMembersRequest(Guid RoleId) : IRequest<IReadOnlyList<RoleMember>>;

using NbgDev.Pst.Projects.Contract.Mediator;

namespace NbgDev.Pst.Projects.Contract.Requests;

public record RemoveRoleMemberRequest(Guid RoleId, string UserId) : IRequest<bool>;

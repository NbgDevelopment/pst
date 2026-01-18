using NbgDev.Pst.Projects.Contract.Mediator;

namespace NbgDev.Pst.Projects.Contract.Requests;

public record DeleteRoleRequest(Guid RoleId) : IRequest<bool>;

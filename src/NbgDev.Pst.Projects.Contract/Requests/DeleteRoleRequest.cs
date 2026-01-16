using MediatR;

namespace NbgDev.Pst.Projects.Contract.Requests;

public record DeleteRoleRequest(Guid RoleId) : IRequest<bool>;

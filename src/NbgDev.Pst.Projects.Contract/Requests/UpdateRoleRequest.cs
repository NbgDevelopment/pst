using MediatR;
using NbgDev.Pst.Projects.Contract.Models;

namespace NbgDev.Pst.Projects.Contract.Requests;

public record UpdateRoleRequest(
    Guid RoleId,
    string Name,
    string Description
) : IRequest<Role?>;

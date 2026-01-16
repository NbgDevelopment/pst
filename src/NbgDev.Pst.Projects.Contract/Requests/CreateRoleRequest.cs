using MediatR;
using NbgDev.Pst.Projects.Contract.Models;

namespace NbgDev.Pst.Projects.Contract.Requests;

public record CreateRoleRequest(
    Guid ProjectId,
    string Name,
    string Description
) : IRequest<Role>;

using MediatR;
using NbgDev.Pst.Projects.AzureTable.Services;
using NbgDev.Pst.Projects.Contract.Models;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Projects.AzureTable.Handlers;

internal class UpdateRoleHandler(IProjectService projectService) : IRequestHandler<UpdateRoleRequest, Role?>
{
    public async Task<Role?> Handle(UpdateRoleRequest request, CancellationToken cancellationToken)
    {
        return await projectService.UpdateRole(request.RoleId, request.Name, request.Description);
    }
}

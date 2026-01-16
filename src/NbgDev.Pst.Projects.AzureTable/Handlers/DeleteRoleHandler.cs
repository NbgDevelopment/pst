using MediatR;
using NbgDev.Pst.Projects.AzureTable.Services;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Projects.AzureTable.Handlers;

internal class DeleteRoleHandler(IProjectService projectService) : IRequestHandler<DeleteRoleRequest, bool>
{
    public async Task<bool> Handle(DeleteRoleRequest request, CancellationToken cancellationToken)
    {
        return await projectService.DeleteRole(request.RoleId);
    }
}

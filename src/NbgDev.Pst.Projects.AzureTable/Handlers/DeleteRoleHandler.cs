using MediatR;
using NbgDev.Pst.Projects.AzureTable.Services;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Projects.AzureTable.Handlers;

internal class DeleteRoleHandler(IProjectService projectService) : IRequestHandler<DeleteRoleRequest>
{
    public async Task Handle(DeleteRoleRequest request, CancellationToken cancellationToken)
    {
        await projectService.DeleteRole(request.RoleId);
    }
}

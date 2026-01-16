using MediatR;
using NbgDev.Pst.Projects.AzureTable.Services;
using NbgDev.Pst.Projects.Contract.Models;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Projects.AzureTable.Handlers;

internal class CreateRoleHandler(IProjectService projectService) : IRequestHandler<CreateRoleRequest, Role>
{
    public async Task<Role> Handle(CreateRoleRequest request, CancellationToken cancellationToken)
    {
        return await projectService.CreateRole(request.ProjectId, request.Name, request.Description);
    }
}

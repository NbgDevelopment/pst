using NbgDev.Pst.Projects.Contract.Mediator;
using NbgDev.Pst.Projects.AzureTable.Services;
using NbgDev.Pst.Projects.Contract.Models;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Projects.AzureTable.Handlers;

internal class GetRolesHandler(IProjectService projectService) : IRequestHandler<GetRolesRequest, IReadOnlyList<Role>>
{
    public async Task<IReadOnlyList<Role>> Handle(GetRolesRequest request, CancellationToken cancellationToken)
    {
        return await projectService.GetRoles(request.ProjectId);
    }
}

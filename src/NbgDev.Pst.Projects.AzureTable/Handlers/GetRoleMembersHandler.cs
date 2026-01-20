using NbgDev.Pst.Projects.Contract.Mediator;
using NbgDev.Pst.Projects.AzureTable.Services;
using NbgDev.Pst.Projects.Contract.Models;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Projects.AzureTable.Handlers;

internal class GetRoleMembersHandler(IProjectService projectService) : IRequestHandler<GetRoleMembersRequest, IReadOnlyList<RoleMember>>
{
    public async Task<IReadOnlyList<RoleMember>> Handle(GetRoleMembersRequest request, CancellationToken cancellationToken)
    {
        return await projectService.GetRoleMembers(request.RoleId);
    }
}

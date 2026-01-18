using NbgDev.Pst.Projects.Contract.Mediator;
using NbgDev.Pst.Projects.AzureTable.Services;
using NbgDev.Pst.Projects.Contract.Models;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Projects.AzureTable.Handlers;

internal class AddRoleMemberHandler(IProjectService projectService) : IRequestHandler<AddRoleMemberRequest, RoleMember>
{
    public async Task<RoleMember> Handle(AddRoleMemberRequest request, CancellationToken cancellationToken)
    {
        return await projectService.AddRoleMember(
            request.RoleId,
            request.UserId,
            request.FirstName,
            request.LastName,
            request.Email
        );
    }
}

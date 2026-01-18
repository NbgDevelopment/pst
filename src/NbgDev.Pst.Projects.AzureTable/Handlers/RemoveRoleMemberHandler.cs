using NbgDev.Pst.Projects.Contract.Mediator;
using NbgDev.Pst.Projects.AzureTable.Services;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Projects.AzureTable.Handlers;

internal class RemoveRoleMemberHandler(IProjectService projectService) : IRequestHandler<RemoveRoleMemberRequest, bool>
{
    public async Task<bool> Handle(RemoveRoleMemberRequest request, CancellationToken cancellationToken)
    {
        return await projectService.RemoveRoleMember(request.RoleId, request.UserId);
    }
}

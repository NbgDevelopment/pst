using MediatR;
using NbgDev.Pst.Projects.AzureTable.Services;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Projects.AzureTable.Handlers;

internal class RemoveRoleMemberHandler(IProjectService projectService) : IRequestHandler<RemoveRoleMemberRequest>
{
    public async Task Handle(RemoveRoleMemberRequest request, CancellationToken cancellationToken)
    {
        await projectService.RemoveRoleMember(request.RoleId, request.UserId);
    }
}

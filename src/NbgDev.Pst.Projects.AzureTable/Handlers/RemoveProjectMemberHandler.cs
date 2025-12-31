using MediatR;
using NbgDev.Pst.Projects.AzureTable.Services;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Projects.AzureTable.Handlers;

internal class RemoveProjectMemberHandler(IProjectService projectService) : IRequestHandler<RemoveProjectMemberRequest, bool>
{
    public async Task<bool> Handle(RemoveProjectMemberRequest request, CancellationToken cancellationToken)
    {
        return await projectService.RemoveProjectMember(request.ProjectId, request.UserId);
    }
}

using NbgDev.Pst.Projects.Contract.Mediator;
using NbgDev.Pst.Projects.AzureTable.Services;
using NbgDev.Pst.Projects.Contract.Models;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Projects.AzureTable.Handlers;

internal class GetProjectMembersHandler(IProjectService projectService) : IRequestHandler<GetProjectMembersRequest, IReadOnlyList<ProjectMember>>
{
    public async Task<IReadOnlyList<ProjectMember>> Handle(GetProjectMembersRequest request, CancellationToken cancellationToken)
    {
        return await projectService.GetProjectMembers(request.ProjectId);
    }
}

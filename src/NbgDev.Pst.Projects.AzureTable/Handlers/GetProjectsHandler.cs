using NbgDev.Pst.Projects.Contract.Mediator;
using NbgDev.Pst.Projects.AzureTable.Services;
using NbgDev.Pst.Projects.Contract.Models;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Projects.AzureTable.Handlers;

internal class GetProjectsHandler(IProjectService projectService) : IRequestHandler<GetProjectsRequest, IReadOnlyList<Project>>
{
    public async Task<IReadOnlyList<Project>> Handle(GetProjectsRequest request, CancellationToken cancellationToken)
    {
        return await projectService.GetProjects();
    }
}

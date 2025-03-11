using MediatR;
using NbgDev.Pst.Projects.AzureTable.Services;
using NbgDev.Pst.Projects.Contract.Models;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Projects.AzureTable.Handlers;

internal class GetProjectHandler(IProjectService projectService) : IRequestHandler<GetProjectRequest, Project?>
{
    public async Task<Project?> Handle(GetProjectRequest request, CancellationToken cancellationToken)
    {
        return await projectService.GetProject(request.Id);
    }
}

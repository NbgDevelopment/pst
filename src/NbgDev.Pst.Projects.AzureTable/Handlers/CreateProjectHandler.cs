using MediatR;
using NbgDev.Pst.Projects.AzureTable.Services;
using NbgDev.Pst.Projects.Contract.Models;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Projects.AzureTable.Handlers;

internal class CreateProjectHandler(IProjectService projectService) : IRequestHandler<CreateProjectRequest, Project>
{
    public async Task<Project> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        return await projectService.CreateProject(request.Name, request.ShortName);
    }
}

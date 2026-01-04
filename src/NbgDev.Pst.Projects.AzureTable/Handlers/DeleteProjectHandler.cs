using MediatR;
using NbgDev.Pst.Projects.AzureTable.Services;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Projects.AzureTable.Handlers;

internal class DeleteProjectHandler(IProjectService projectService) : IRequestHandler<DeleteProjectRequest, bool>
{
    public async Task<bool> Handle(DeleteProjectRequest request, CancellationToken cancellationToken)
    {
        return await projectService.DeleteProject(request.ProjectId);
    }
}

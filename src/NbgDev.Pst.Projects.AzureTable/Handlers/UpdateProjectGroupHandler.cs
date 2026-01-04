using MediatR;
using NbgDev.Pst.Projects.AzureTable.Services;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Projects.AzureTable.Handlers;

internal class UpdateProjectGroupHandler(IProjectService projectService) : IRequestHandler<UpdateProjectGroupRequest>
{
    public async Task Handle(UpdateProjectGroupRequest request, CancellationToken cancellationToken)
    {
        await projectService.UpdateProjectGroup(request.ProjectId, request.Group);
    }
}

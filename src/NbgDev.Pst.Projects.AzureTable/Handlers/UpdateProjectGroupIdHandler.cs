using MediatR;
using NbgDev.Pst.Projects.AzureTable.Services;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Projects.AzureTable.Handlers;

internal class UpdateProjectGroupIdHandler(IProjectService projectService) : IRequestHandler<UpdateProjectGroupIdRequest>
{
    public async Task Handle(UpdateProjectGroupIdRequest request, CancellationToken cancellationToken)
    {
        await projectService.UpdateProjectGroupId(request.ProjectId, request.GroupId);
    }
}

using NbgDev.Pst.Projects.Contract.Mediator;
using NbgDev.Pst.Projects.AzureTable.Services;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Projects.AzureTable.Handlers;

internal class UpdateProjectGroupHandler(IProjectService projectService) : IRequestHandler<UpdateProjectGroupRequest, Unit>
{
    public async Task<Unit> Handle(UpdateProjectGroupRequest request, CancellationToken cancellationToken)
    {
        await projectService.UpdateProjectGroup(request.ProjectId, request.Group);
        return Unit.Value;
    }
}

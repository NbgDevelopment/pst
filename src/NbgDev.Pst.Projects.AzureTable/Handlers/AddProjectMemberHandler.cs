using MediatR;
using NbgDev.Pst.Projects.AzureTable.Services;
using NbgDev.Pst.Projects.Contract.Models;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Projects.AzureTable.Handlers;

internal class AddProjectMemberHandler(IProjectService projectService) : IRequestHandler<AddProjectMemberRequest, ProjectMember>
{
    public async Task<ProjectMember> Handle(AddProjectMemberRequest request, CancellationToken cancellationToken)
    {
        return await projectService.AddProjectMember(
            request.ProjectId,
            request.UserId,
            request.FirstName,
            request.LastName,
            request.Email
        );
    }
}

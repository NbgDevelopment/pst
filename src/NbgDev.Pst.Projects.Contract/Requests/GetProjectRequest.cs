using MediatR;
using NbgDev.Pst.Projects.Contract.Models;

namespace NbgDev.Pst.Projects.Contract.Requests;

public class GetProjectRequest(Guid id) : IRequest<Project?>
{
    public Guid Id => id;
}

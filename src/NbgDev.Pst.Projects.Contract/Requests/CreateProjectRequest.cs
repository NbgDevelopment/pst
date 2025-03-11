using MediatR;
using NbgDev.Pst.Projects.Contract.Models;

namespace NbgDev.Pst.Projects.Contract.Requests;

public class CreateProjectRequest(string name) : IRequest<Project>
{
    public string Name => name;
}

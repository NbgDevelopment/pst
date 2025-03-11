using MediatR;
using NbgDev.Pst.Projects.Contract.Models;

namespace NbgDev.Pst.Projects.Contract.Requests;

public class CreateProjectRequest(string name, string shortName) : IRequest<Project>
{
    public string Name => name;

    public string ShortName => shortName;
}

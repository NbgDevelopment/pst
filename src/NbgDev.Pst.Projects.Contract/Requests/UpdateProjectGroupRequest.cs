using MediatR;
using NbgDev.Pst.Projects.Contract.Models;

namespace NbgDev.Pst.Projects.Contract.Requests;

public record UpdateProjectGroupRequest(Guid ProjectId, GroupInfo Group) : IRequest;

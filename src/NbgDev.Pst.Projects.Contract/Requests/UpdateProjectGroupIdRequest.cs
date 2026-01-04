using MediatR;

namespace NbgDev.Pst.Projects.Contract.Requests;

public record UpdateProjectGroupIdRequest(Guid ProjectId, string GroupId) : IRequest;

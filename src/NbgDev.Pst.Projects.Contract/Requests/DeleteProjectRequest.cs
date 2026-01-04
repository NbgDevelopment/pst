using MediatR;

namespace NbgDev.Pst.Projects.Contract.Requests;

public record DeleteProjectRequest(Guid ProjectId) : IRequest<bool>;

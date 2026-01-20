using NbgDev.Pst.Projects.Contract.Mediator;

namespace NbgDev.Pst.Projects.Contract.Requests;

public record DeleteProjectRequest(Guid ProjectId) : IRequest<bool>;

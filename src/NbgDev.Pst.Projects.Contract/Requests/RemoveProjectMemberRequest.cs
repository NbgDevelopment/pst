using MediatR;

namespace NbgDev.Pst.Projects.Contract.Requests;

public record RemoveProjectMemberRequest(Guid ProjectId, string UserId) : IRequest<bool>;

using MediatR;
using NbgDev.Pst.Projects.Contract.Models;

namespace NbgDev.Pst.Projects.Contract.Requests;

public record SearchEntraIdUsersRequest(string SearchTerm) : IRequest<IReadOnlyList<EntraIdUser>>;

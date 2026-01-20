using NbgDev.Pst.Projects.Contract.Mediator;
using NbgDev.Pst.Api.Services;
using NbgDev.Pst.Projects.Contract.Models;
using NbgDev.Pst.Projects.Contract.Requests;

namespace NbgDev.Pst.Api.Handlers;

internal class SearchEntraIdUsersHandler(IEntraIdService entraIdService) : IRequestHandler<SearchEntraIdUsersRequest, IReadOnlyList<EntraIdUser>>
{
    public async Task<IReadOnlyList<EntraIdUser>> Handle(SearchEntraIdUsersRequest request, CancellationToken cancellationToken)
    {
        return await entraIdService.SearchUsers(request.SearchTerm);
    }
}

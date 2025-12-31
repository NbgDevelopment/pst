using NbgDev.Pst.Projects.Contract.Models;

namespace NbgDev.Pst.Api.Services;

public interface IEntraIdService
{
    Task<IReadOnlyList<EntraIdUser>> SearchUsers(string searchTerm);
}

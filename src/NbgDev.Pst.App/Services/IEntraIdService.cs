using NbgDev.Pst.App.Models;

namespace NbgDev.Pst.App.Services;

public interface IEntraIdService
{
    Task<IReadOnlyList<EntraIdUser>> SearchUsers(string searchTerm);
}

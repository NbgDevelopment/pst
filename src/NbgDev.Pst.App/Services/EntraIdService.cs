using NbgDev.Pst.Api.Client;
using NbgDev.Pst.App.Models;

namespace NbgDev.Pst.App.Services;

public class EntraIdService(IEntraIdClient entraIdClient) : IEntraIdService
{
    public async Task<IReadOnlyList<EntraIdUser>> SearchUsers(string searchTerm)
    {
        var users = await entraIdClient.SearchUsersAsync(searchTerm);
        return users.Select(Map).ToList();
    }

    private static EntraIdUser Map(EntraIdUserDto dto)
    {
        return new EntraIdUser
        {
            Id = dto.Id,
            DisplayName = dto.DisplayName,
            GivenName = dto.GivenName,
            Surname = dto.Surname,
            Mail = dto.Mail
        };
    }
}

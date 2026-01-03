using Microsoft.Graph;
using Microsoft.Graph.Models;
using NbgDev.Pst.Projects.Contract.Models;

namespace NbgDev.Pst.Api.Services;

public class EntraIdService(GraphServiceClient graphClient) : IEntraIdService
{
    public async Task<IReadOnlyList<EntraIdUser>> SearchUsers(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return Array.Empty<EntraIdUser>();
        }

        var users = await graphClient.Users
            .GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Filter = $"startsWith(givenName, '{searchTerm}') or startsWith(surname, '{searchTerm}') or startsWith(mail, '{searchTerm}')";
                requestConfiguration.QueryParameters.Select = new[] { "id", "displayName", "givenName", "surname", "mail" };
                requestConfiguration.QueryParameters.Top = 20;
            });

        return users?.Value?
            .Where(u => u.Mail != null && u.GivenName != null && u.Surname != null)
            .Select(Map)
            .ToList() ?? new List<EntraIdUser>();
    }

    private static EntraIdUser Map(User user)
    {
        return new EntraIdUser
        {
            Id = user.Id ?? string.Empty,
            DisplayName = user.DisplayName ?? string.Empty,
            GivenName = user.GivenName ?? string.Empty,
            Surname = user.Surname ?? string.Empty,
            Mail = user.Mail ?? string.Empty
        };
    }
}

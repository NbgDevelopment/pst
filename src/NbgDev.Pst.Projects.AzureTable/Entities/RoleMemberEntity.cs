using Azure;
using Azure.Data.Tables;

namespace NbgDev.Pst.Projects.AzureTable.Entities;

internal class RoleMemberEntity : ITableEntity
{
    public const string EntityPartitionKeyPrefix = "RoleMember-";

    public required Guid RoleId { get; set; }

    public required string UserId { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public string? Email { get; set; }

    public string PartitionKey
    {
        get => EntityPartitionKeyPrefix + RoleId;
        set { }
    }

    public string RowKey
    {
        get => UserId;
        set => UserId = value;
    }

    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}

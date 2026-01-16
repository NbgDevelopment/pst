using Azure;
using Azure.Data.Tables;

namespace NbgDev.Pst.Projects.AzureTable.Entities;

internal class RoleEntity : ITableEntity
{
    public const string EntityPartitionKeyPrefix = "Role-";

    public required Guid Id { get; set; }

    public required Guid ProjectId { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public string PartitionKey
    {
        get => EntityPartitionKeyPrefix + ProjectId;
        set { }
    }

    public string RowKey
    {
        get => Id.ToString();
        set => Id = Guid.Parse(value);
    }

    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}

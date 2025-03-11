using Azure;
using Azure.Data.Tables;

namespace NbgDev.Pst.Projects.AzureTable.Entities;

internal class ProjectEntity : ITableEntity
{
    public const string EntityPartitionKey = "Project";

    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required string ShortName { get; set; }

    public string PartitionKey { get; set; } = EntityPartitionKey;

    public string RowKey
    {
        get => Id.ToString();
        set => Guid.Parse(value);
    }

    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}

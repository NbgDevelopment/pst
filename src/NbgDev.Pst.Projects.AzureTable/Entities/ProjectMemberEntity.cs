using Azure;
using Azure.Data.Tables;

namespace NbgDev.Pst.Projects.AzureTable.Entities;

internal class ProjectMemberEntity : ITableEntity
{
    public const string EntityPartitionKeyPrefix = "ProjectMember-";

    public required Guid ProjectId { get; set; }

    public required string UserId { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Email { get; set; }

    public string PartitionKey
    {
        get => EntityPartitionKeyPrefix + ProjectId;
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

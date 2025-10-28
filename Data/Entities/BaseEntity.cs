using System.ComponentModel.DataAnnotations.Schema;

namespace ElderConnectApi.Data.Entities;

public abstract class BaseEntity : IAuditable
{
    [Column("created_at", TypeName = "timestamptz")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTimeOffset? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamptz")]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTimeOffset? UpdatedAt { get; set; }

    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}

public interface IAuditable
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace ElderConnectApi.Data.Entities;

public class UserAddress : BaseEntity
{
    public Guid AddressId { get; set; }
    public Guid UserId { get; set; }
    public string Country { get; set; } = null!;
    public string Province { get; set; } = null!;
    public string City { get; set; } = null!;
    public string? AddressLine { get; set; }

    [Column(TypeName = "geography (point)")]
    public Point Coordinates { get; set; } = null!;
    public User? User { get; set; } = null!;
}

public class NurseAddress : BaseEntity
{
    public Guid AddressId { get; set; }
    public Guid NurseId { get; set; }
    public string Country { get; set; } = null!;
    public string Province { get; set; } = null!;
    public string City { get; set; } = null!;
    public string? AddressLine { get; set; }

    [Column(TypeName = "geography (point)")]
    public Point Coordinates { get; set; } = null!;
    public Nurse? Nurse { get; set; } = null!;
}
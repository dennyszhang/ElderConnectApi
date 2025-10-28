using ElderConnectApi.Data.Common;

namespace ElderConnectApi.Data.Entities;

public class Patient : BaseEntity
{
    public Guid PatientId { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public Gender Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public double HeightInCm { get; set; }
    public double WeightInKg { get; set; }
    public string? MedicalConditions { get; set; }

    public User? User { get; set; }
}
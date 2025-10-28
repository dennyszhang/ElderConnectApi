using ElderConnectApi.Data.Common;

namespace ElderConnectApi.Controllers.Dto.Response;

public class PatientDto
{
    public Guid PatientId { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public double HeightInCm { get; set; }
    public double WeightInKg { get; set; }
    public Gender Gender { get; set; }
    public string? MedicalHistory { get; set; } = null!;
}
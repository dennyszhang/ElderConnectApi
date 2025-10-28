using System.ComponentModel.DataAnnotations;

namespace ElderConnectApi.Controllers.Dto.Request;

public class CreatePatientRequestDto
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
    public string Name { get; set; } = null!;

    [Required]
    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; }

    [Required]
    [Range(1, 250)]
    public double HeightInCm { get; set; }

    [Required]
    [Range(1, 500)]
    public double WeightInKg { get; set; }

    [MaxLength(500)]
    public string? MedicalConditions { get; set; }
}
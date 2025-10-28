using System.ComponentModel.DataAnnotations;
using ElderConnectApi.Data.Common;
using ElderConnectApi.Data.Entities;

namespace ElderConnectApi.Controllers.Dto.Response;

public class NurseResponseDto
{
    public Guid NurseId { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string? GraduatedFrom { get; set; } = null!;
    public string? LicenseNumber { get; set; } = null!;
    public string? ProfileImageUrl { get; set; } = null!;
}

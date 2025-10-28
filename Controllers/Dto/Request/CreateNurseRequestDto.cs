using System.ComponentModel.DataAnnotations;
using ElderConnectApi.Data.Common;

namespace ElderConnectApi.Controllers.Dto.Request;

public class CreateNurseRequestDto
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public DateOnly DateOfBirth { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public Gender Gender { get; set; }

    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = null!;
    
    public string? GraduatedFrom { get; set; } = null!;
    public string? LicenseNumber { get; set; } = null!;

    [Url]
    public string? ProfileImageUrl { get; set; } = null!;
}
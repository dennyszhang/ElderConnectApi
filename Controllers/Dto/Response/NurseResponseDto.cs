using ElderConnectApi.Data.Common;

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

public class NurseUnavailableDatesResponseDto
{
    public IEnumerable<DateOnly> UnavailableDates { get; set; } = [];
}
using ElderConnectApi.Data.Common;

namespace ElderConnectApi.Controllers.Dto.Response;

public class GetUserResponseDto
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public UserAccountStatus Status { get; set; }
    public DateTimeOffset JoinDate { get; set; }

    public IEnumerable<PatientDto> Patients { get; set; } = [];
    public IEnumerable<AddressResponseDto> Addresses { get; set; } = [];
}
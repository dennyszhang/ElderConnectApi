using ElderConnectApi.Data.Common;
using ElderConnectApi.Data.Entities;

namespace ElderConnectApi.Controllers.Dto.Response;

public class UserBookingDto
{
    public Guid BookingId { get; set; }
    public string? ReferenceCode { get; set; }
    public DateTime BookingDate { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public BookingStatus Status { get; set; }
    public NurseResponseDto Nurse { get; set; } = null!;
    public PatientDto Patient { get; set; } = null!;
    public AddressResponseDto Address { get; set; } = null!;
    public BookingTimeline? Timeline { get; set; }

}
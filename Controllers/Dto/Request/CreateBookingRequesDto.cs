using System.ComponentModel.DataAnnotations;

namespace ElderConnectApi.Controllers.Dto.Request;

public class CreateBookingRequestDto
{
    [Required]
    [IsFutureDate]
    public DateTimeOffset StartTime { get; set; }

    [Required]
    [IsFutureDate]
    [IsDateGreaterThan(nameof(StartTime))]
    public DateTimeOffset EndTime { get; set; }

    [Required]
    public Guid NurseId { get; set; }

    [Required]
    public Guid PatientId { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid AddressId { get; set; }
}

using System.ComponentModel.DataAnnotations.Schema;
using ElderConnectApi.Data.Common;

namespace ElderConnectApi.Data.Entities;

public class Booking : BaseEntity
{
    public Guid BookingId { get; set; }
    public string? ReferenceCode { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public BookingStatus Status { get; set; }

    [Column(TypeName = "jsonb")]
    public BookingTimeline? Timeline { get; set; }

    [Column(TypeName = "jsonb")]
    public BookingPayment? Payment { get; set; }


    public Guid NurseId { get; set; }
    public Nurse? Nurse { get; set; }

    [Column(TypeName = "jsonb")]
    public string? NurseSnapshot { get; set; }

    public Guid PatientId { get; set; }
    public Patient? Patient { get; set; }

    [Column(TypeName = "jsonb")]
    public string? PatientSnapshot { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    [Column(TypeName = "jsonb")]
    public string? UserSnapshot { get; set; }

    public Guid AddressId { get; set; }
    public UserAddress? Address { get; set; }
    [Column(TypeName = "jsonb")]
    public string? AddressSnapshot { get; set; }

}

// Complex Type for Booking Timeline JSON Mapping
public class BookingTimeline
{
    public DateTimeOffset? ConfirmedAt { get; set; }
    public DateTimeOffset? InProgressAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public DateTimeOffset? CancelledAt { get; set; }
}

// Complex Type for Booking Payment Information
public class BookingPayment
{
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = null!;
    public DateTimeOffset PaidAt { get; set; }
}
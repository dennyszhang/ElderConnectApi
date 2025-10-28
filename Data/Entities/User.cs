using ElderConnectApi.Data.Common;

namespace ElderConnectApi.Data.Entities;

public class User : BaseEntity
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public UserAccountStatus AccountStatus { get; set; }
    public DateTimeOffset JoinDate { get; set; }

    public List<UserAddress> Addresses { get; set; } = [];
    public List<Patient> Patients { get; set; } = [];
    public List<Booking> Bookings { get; set; } = [];
}

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

    public IQueryable<UserAddress> Addresses { get; set; } = Enumerable.Empty<UserAddress>().AsQueryable();
    public IQueryable<Patient> Patients { get; set; } = Enumerable.Empty<Patient>().AsQueryable();
    public IQueryable<Booking> Bookings { get; set; } = Enumerable.Empty<Booking>().AsQueryable();
}

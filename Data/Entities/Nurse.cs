using ElderConnectApi.Data.Common;

namespace ElderConnectApi.Data.Entities;

public class Nurse : BaseEntity
{
    public Guid NurseId { get; set; }
    public NurseAccountStatus AccountStatus { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public DateOnly? JoinDate { get; set; }
    public string Email { get; set; } = null!;
    public Gender Gender { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string? GraduatedFrom { get; set; }
    public string? LicenseNumber { get; set; }
    public string? ProfileImageUrl { get; set; }
    public NurseAddress? Address { get; set; }
    public NurseSchedule[] Schedules { get; set; } = [];
    public IQueryable<Booking> Bookings { get; set; } = Enumerable.Empty<Booking>().AsQueryable();
    public IQueryable<NurseLeave> Leaves { get; set; } = Enumerable.Empty<NurseLeave>().AsQueryable();
}

public class NurseSchedule : BaseEntity
{
    public Guid NurseId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsActive { get; set; }
    public Nurse? Nurse { get; set; }
}

public class NurseLeave : BaseEntity
{
    public Guid NurseId { get; set; }
    public DateOnly LeaveDate { get; set; }
    public string Reason { get; set; } = null!;
    public Nurse? Nurse { get; set; }
}
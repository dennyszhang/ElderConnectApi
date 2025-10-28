using ElderConnectApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ElderConnectApi.Data;

public class ElderConnectDbContext(DbContextOptions<ElderConnectDbContext> options) : DbContext(options)
{
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<UserAddress> Addresses { get; set; }
    public DbSet<Nurse> Nurses { get; set; }
    public DbSet<NurseAddress> NurseAddresses { get; set; }
    public DbSet<NurseSchedule> NurseSchedules { get; set; }
    public DbSet<NurseLeave> NurseLeaves { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(u =>
        {
            u.Property(e => e.UserId)
                .HasColumnType("uuid")
                .HasColumnName("user_id")
                .ValueGeneratedOnAdd();

            u.Property(e => e.Name)
                .HasColumnType("varchar(100)")
                .HasColumnName("name")
                .IsRequired();

            u.Property(e => e.Email)
                .HasColumnType("varchar(100)")
                .HasColumnName("email")
                .IsRequired();

            u.Property(e => e.PhoneNumber)
                .HasColumnType("varchar(15)")
                .HasColumnName("phone_number")
                .IsRequired();

            u.Property(e => e.PasswordHash)
                .HasColumnType("varchar(256)")
                .HasColumnName("password_hash")
                .IsRequired();

            u.Property(e => e.Salt)
                .HasColumnType("varchar(256)")
                .HasColumnName("salt")
                .IsRequired();

            u.Property(e => e.AccountStatus)
                .HasColumnType("user_account_status")
                .HasColumnName("account_status")
                .IsRequired();

            u.Property(e => e.JoinDate)
                .HasColumnType("timestamptz")
                .HasColumnName("join_date");

            u.HasKey(e => e.UserId);
            u.HasIndex(e => e.Email, "ix_user_email").IsUnique();
            u.HasIndex(e => e.PhoneNumber, "ix_user_phone_number").IsUnique();
            u.HasMany(e => e.Addresses).WithOne(a => a.User).HasForeignKey(a => a.UserId);
            u.HasMany(e => e.Patients).WithOne(p => p.User).HasForeignKey(p => p.UserId);
            u.HasMany(e => e.Bookings).WithOne(b => b.User).HasForeignKey(b => b.UserId);
        });

        modelBuilder.Entity<Patient>(p =>
        {
            p.Property(e => e.PatientId)
                .HasColumnType("uuid")
                .HasColumnName("patient_id")
                .ValueGeneratedOnAdd();

            p.Property(e => e.UserId)
                .HasColumnType("uuid")
                .HasColumnName("user_id")
                .IsRequired();

            p.Property(e => e.Name)
                .HasColumnType("varchar(100)")
                .HasColumnName("name")
                .IsRequired();

            p.Property(e => e.DateOfBirth)
                .HasColumnType("date")
                .HasColumnName("date_of_birth")
                .IsRequired();

            p.Property(e => e.HeightInCm)
                .HasColumnType("double precision")
                .HasColumnName("height_in_cm")
                .IsRequired();

            p.Property(e => e.WeightInKg)
                .HasColumnType("double precision")
                .HasColumnName("weight_in_kg")
                .IsRequired();

            p.Property(e => e.MedicalConditions)
                .HasColumnType("varchar(500)")
                .HasColumnName("medical_conditions");

            p.HasKey(e => e.PatientId);
            p.HasIndex(e => e.UserId, "ix_patient_user_id");
        });

        modelBuilder.Entity<UserAddress>(a =>
        {
            a.Property(e => e.AddressId)
                .HasColumnType("uuid")
                .HasColumnName("address_id")
                .ValueGeneratedOnAdd();

            a.Property(e => e.UserId)
                .HasColumnType("uuid")
                .HasColumnName("user_id")
                .IsRequired();

            a.Property(e => e.Country)
                .HasColumnType("varchar(100)")
                .HasColumnName("country")
                .IsRequired();

            a.Property(e => e.Province)
                .HasColumnType("varchar(100)")
                .HasColumnName("province")
                .IsRequired();

            a.Property(e => e.City)
                .HasColumnType("varchar(100)")
                .HasColumnName("city")
                .IsRequired();

            a.Property(e => e.AddressLine)
                .HasColumnType("varchar(200)")
                .HasColumnName("address_line");

            a.Property(e => e.Coordinates)
                .HasColumnType("geography (point)")
                .HasColumnName("coordinates")
                .IsRequired();

            a.HasKey(e => e.AddressId);
            a.HasIndex(e => e.UserId, "ix_address_user_id");
        });

        modelBuilder.Entity<Nurse>(n =>
        {
            n.Property(e => e.NurseId)
                .HasColumnType("uuid")
                .HasColumnName("nurse_id")
                .ValueGeneratedOnAdd();

            n.Property(e => e.AccountStatus)
                .HasColumnType("nurse_account_status")
                .HasColumnName("account_status")
                .IsRequired();

            n.Property(e => e.Name)
                .HasColumnType("varchar(100)")
                .HasColumnName("name")
                .IsRequired();

            n.Property(e => e.DateOfBirth)
                .HasColumnType("date")
                .HasColumnName("date_of_birth")
                .IsRequired();

            n.Property(e => e.JoinDate)
                .HasColumnType("date")
                .HasColumnName("join_date");

            n.Property(e => e.Email)
                .HasColumnType("varchar(100)")
                .HasColumnName("email")
                .IsRequired();

            n.Property(e => e.Gender)
                .HasColumnType("gender")
                .HasColumnName("gender")
                .IsRequired();

            n.Property(e => e.PhoneNumber)
                .HasColumnType("varchar(15)")
                .HasColumnName("phone_number")
                .IsRequired();

            n.Property(e => e.GraduatedFrom)
                .HasColumnType("varchar(200)")
                .HasColumnName("graduated_from");

            n.Property(e => e.LicenseNumber)
                .HasColumnType("varchar(50)")
                .HasColumnName("license_number");

            n.Property(e => e.ProfileImageUrl)
                .HasColumnType("text")
                .HasColumnName("profile_image_url");

            n.HasKey(e => e.NurseId);
            n.HasOne(e => e.Address).WithOne(a => a.Nurse).HasForeignKey<NurseAddress>(a => a.NurseId);
            n.HasMany(e => e.Schedules).WithOne(s => s.Nurse).HasForeignKey(s => s.NurseId);
            n.HasMany(e => e.Leaves).WithOne(e => e.Nurse).HasForeignKey(l => l.NurseId);
            n.HasMany(e => e.Bookings).WithOne(b => b.Nurse).HasForeignKey(b => b.NurseId);
            n.HasIndex(e => e.Email, "ix_nurse_email").IsUnique();
            n.HasIndex(e => e.PhoneNumber, "ix_nurse_phone_number").IsUnique();
        });
        
        modelBuilder.Entity<NurseAddress>(a =>
        {
            a.Property(e => e.AddressId)
                .HasColumnType("uuid")
                .HasColumnName("address_id")
                .ValueGeneratedOnAdd();

            a.Property(e => e.NurseId)
                .HasColumnType("uuid")
                .HasColumnName("nurse_id")
                .IsRequired();

            a.Property(e => e.Country)
                .HasColumnType("varchar(100)")
                .HasColumnName("country")
                .IsRequired();

            a.Property(e => e.Province)
                .HasColumnType("varchar(100)")
                .HasColumnName("province")
                .IsRequired();

            a.Property(e => e.City)
                .HasColumnType("varchar(100)")
                .HasColumnName("city")
                .IsRequired();

            a.Property(e => e.AddressLine)
                .HasColumnType("varchar(200)")
                .HasColumnName("address_line");

            a.Property(e => e.Coordinates)
                .HasColumnType("geography (point)")
                .HasColumnName("coordinates")
                .IsRequired();

            a.HasKey(e => e.AddressId);
            a.HasIndex(e => e.NurseId, "ix_nurse_address_nurse_id").IsUnique();
        });

        modelBuilder.Entity<NurseSchedule>(s =>
        {
            s.Property(e => e.NurseId)
                .HasColumnType("uuid")
                .HasColumnName("nurse_id")
                .IsRequired();

            s.Property(e => e.DayOfWeek)
                .HasColumnType("integer")
                .HasColumnName("day_of_week")
                .IsRequired();

            s.Property(e => e.StartTime)
                .HasColumnType("time")
                .HasColumnName("start_time")
                .IsRequired();

            s.Property(e => e.EndTime)
                .HasColumnType("time")
                .HasColumnName("end_time")
                .IsRequired();

            s.Property(e => e.IsActive)
                .HasColumnType("boolean")
                .HasColumnName("is_active")
                .IsRequired();

            s.HasKey(e => new { e.NurseId, e.DayOfWeek });
            s.HasIndex(e => e.NurseId, "ix_nurse_schedule_nurse_id");
            s.HasIndex(e => e.DayOfWeek, "ix_nurse_schedule_day_of_week");
        });

        modelBuilder.Entity<NurseLeave>(l =>
        {
            l.Property(e => e.NurseId)
                .HasColumnType("uuid")
                .HasColumnName("nurse_id")
                .IsRequired();

            l.Property(e => e.LeaveDate)
                .HasColumnType("date")
                .HasColumnName("leave_date")
                .IsRequired();

            l.Property(e => e.Reason)
                .HasColumnType("varchar(500)")
                .HasColumnName("reason")
                .IsRequired();

            l.HasKey(e => new { e.NurseId, e.LeaveDate });
            l.HasIndex(e => e.NurseId, "ix_nurse_leave_nurse_id");
        });

        modelBuilder.Entity<Booking>(b =>
        {
            b.Property(e => e.BookingId)
                .HasColumnType("uuid")
                .HasColumnName("booking_id")
                .ValueGeneratedOnAdd();

            b.Property(e => e.ReferenceCode)
                .HasColumnType("varchar(50)")
                .HasColumnName("reference_code");

            b.Property(e => e.StartTime)
                .HasColumnType("timestamptz")
                .HasColumnName("start_time")
                .IsRequired();

            b.Property(e => e.EndTime)
                .HasColumnType("timestamptz")
                .HasColumnName("end_time")
                .IsRequired();

            b.Property(e => e.Status)
                .HasColumnType("booking_status")
                .HasColumnName("status")
                .IsRequired();

            b.Property(e => e.Timeline)
                .HasColumnType("jsonb")
                .HasColumnName("timeline");
                
            b.Property(e => e.Payment)
                .HasColumnType("jsonb")
                .HasColumnName("payment");

            b.HasKey(e => e.BookingId);
            b.HasIndex(e => e.ReferenceCode, "ix_booking_reference_code").IsUnique();
            b.HasIndex(e => e.Status, "ix_booking_status");
            b.HasIndex(e => e.NurseId, "ix_booking_nurse_id");
            b.HasIndex(e => e.PatientId, "ix_booking_patient_id");
            b.HasIndex(e => e.UserId, "ix_booking_user_id");
            b.HasIndex(e => e.AddressId, "ix_booking_address_id");
        });

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IAuditable).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.Name).Property<DateTimeOffset?>("CreatedAt")
                    .HasColumnName("created_at")
                    .HasColumnType("timestamptz")
                    .ValueGeneratedOnAdd();

                modelBuilder.Entity(entityType.Name).Property<DateTimeOffset?>("UpdatedAt")
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamptz")
                    .ValueGeneratedOnUpdate();
            }
        }
    }
}
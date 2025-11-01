using ElderConnectApi.Controllers.Dto;
using ElderConnectApi.Controllers.Dto.Request;
using ElderConnectApi.Controllers.Dto.Response;
using ElderConnectApi.Data;
using ElderConnectApi.Data.Common;
using ElderConnectApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace ElderConnectApi.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class UserController(ElderConnectDbContext dbContext) : ControllerBase
{

    [ProducesResponseType(typeof(GetUserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<GetUserResponseDto>> GetUserById(Guid userId)
    {
        var user = await dbContext.Users
            .Include(u => u.Patients)
            .Include(u => u.Addresses)
            .Where(u => u.UserId == userId)
            .Select(u => new GetUserResponseDto
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Status = u.AccountStatus,
                JoinDate = u.JoinDate,
                Patients = u.Patients.Select(p => new PatientDto
                {
                    PatientId = p.PatientId,
                    Name = p.Name,
                    DateOfBirth = p.DateOfBirth,
                    HeightInCm = p.HeightInCm,
                    WeightInKg = p.WeightInKg,
                }),
                Addresses = u.Addresses.Select(ua => new AddressResponseDto
                {
                    AddressId = ua.AddressId,
                    Country = ua.Country,
                    Province = ua.Province,
                    City = ua.City,
                    AddressLine = ua.AddressLine,
                    Coordinates = new CoordinatesDto(ua.Coordinates),
                })
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound($"User with ID {userId} not found.");
        }

        return Ok(user);
    }

    [ProducesResponseType(typeof(GetUserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<GetUserResponseDto>> Register([FromBody] CreateUserRequestDto requestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var newUser = new User
        {
            Name = requestDto.Name,
            Email = requestDto.Email,
            PhoneNumber = requestDto.PhoneNumber,
            PasswordHash = "",
            Salt = "",
            AccountStatus = Data.Common.UserAccountStatus.Pending,
        };
        dbContext.Users.Add(newUser);
        await dbContext.SaveChangesAsync();

        return Ok(new GetUserResponseDto
        {
            UserId = newUser.UserId,
            Name = newUser.Name,
            Email = newUser.Email,
            PhoneNumber = newUser.PhoneNumber,
            Status = newUser.AccountStatus,
            Patients = [],
            Addresses = []
        });
    }

    [ProducesResponseType(typeof(AddressResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("{userId:guid}/address")]
    public async Task<ActionResult<AddressResponseDto>> AddAddress([FromRoute] Guid userId, [FromBody] CreateUserAddressRequestDto requestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userExists = await dbContext.Users.AnyAsync(u => u.UserId == userId);
        if (!userExists)
        {
            return NotFound($"User with ID {userId} not found.");
        }

        var newAddress = new UserAddress
        {
            UserId = userId,
            Country = requestDto.Country,
            Province = requestDto.Province,
            City = requestDto.City,
            AddressLine = requestDto.AddressLine,
            Coordinates = new Point(requestDto.Coordinates.Longitude, requestDto.Coordinates.Latitude),
        };
        dbContext.Addresses.Add(newAddress);
        await dbContext.SaveChangesAsync();

        return Ok(new AddressResponseDto
        {
            AddressId = newAddress.AddressId,
            Country = newAddress.Country,
            Province = newAddress.Province,
            City = newAddress.City,
            AddressLine = newAddress.AddressLine,
            Coordinates = new CoordinatesDto(newAddress.Coordinates),
        });
    }

    [ProducesResponseType(typeof(AddressResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{userId:guid}/address/{addressId:guid}")]
    public async Task<ActionResult<AddressResponseDto>> GetAddressById([FromRoute] Guid userId, [FromRoute] Guid addressId)
    {
        var userExists = await dbContext.Users.AnyAsync(u => u.UserId == userId);
        if (!userExists)
        {
            return NotFound($"User with ID {userId} not found.");
        }

        var address = await dbContext.Addresses
            .Where(a => a.UserId == userId && a.AddressId == addressId)
            .Select(a => new AddressResponseDto
            {
                AddressId = a.AddressId,
                Country = a.Country,
                Province = a.Province,
                City = a.City,
                AddressLine = a.AddressLine,
                Coordinates = new CoordinatesDto(a.Coordinates),
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (address == null)
        {
            return NotFound($"Address with ID {addressId} not found for user with ID {userId}.");
        }

        return Ok(address);
    }

    [ProducesResponseType(typeof(IEnumerable<AddressResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{userId:guid}/address")]
    public async Task<ActionResult<IEnumerable<AddressResponseDto>>> GetAddressesByUserId([FromRoute] Guid userId)
    {
        var userExists = await dbContext.Users.AnyAsync(u => u.UserId == userId);
        if (!userExists)
        {
            return NotFound($"User with ID {userId} not found.");
        }

        var addresses = await dbContext.Addresses
            .Where(a => a.UserId == userId)
            .Select(a => new AddressResponseDto
            {
                AddressId = a.AddressId,
                Country = a.Country,
                Province = a.Province,
                City = a.City,
                AddressLine = a.AddressLine,
                Coordinates = new CoordinatesDto(a.Coordinates),
            })
            .AsNoTracking()
            .ToListAsync();

        return Ok(addresses);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{userId:guid}/address/{addressId:guid}")]
    public async Task<ActionResult> DeleteAddress([FromRoute] Guid userId, [FromRoute] Guid addressId)
    {
        var userExists = await dbContext.Users.AnyAsync(u => u.UserId == userId);
        if (!userExists)
        {
            return NotFound($"User with ID {userId} not found.");
        }

        int n = await dbContext.Addresses.Where(addr => addr.AddressId == addressId).ExecuteDeleteAsync();
        if (n == 0)
        {
            return NotFound($"Address with ID {addressId} not found.");
        }

        return NoContent();
    }

    [ProducesResponseType(typeof(AddressResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{userId:guid}/address/{addressId:guid}")]
    public async Task<ActionResult<AddressResponseDto>> UpdateAddress(Guid userId, Guid addressId, [FromBody] UpdateUserAddressRequestDto requestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userExists = await dbContext.Users.AnyAsync(u => u.UserId == userId);
        if (!userExists)
        {
            return NotFound($"User with ID {userId} not found.");
        }

        var address = await dbContext.Addresses.FirstOrDefaultAsync(a => a.AddressId == addressId && a.UserId == userId);
        if (address == null)
        {
            return NotFound($"Address with ID {addressId} not found for user with ID {userId}.");
        }

        address.Country = requestDto.Country;
        address.Province = requestDto.Province;
        address.City = requestDto.City;
        address.AddressLine = requestDto.AddressLine;
        address.Coordinates = new Point(requestDto.Coordinates.Longitude, requestDto.Coordinates.Latitude);

        await dbContext.SaveChangesAsync();

        return Ok(new AddressResponseDto
        {
            AddressId = address.AddressId,
            Country = address.Country,
            Province = address.Province,
            City = address.City,
            AddressLine = address.AddressLine,
            Coordinates = new CoordinatesDto(address.Coordinates),
        });
    }

    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("{userId:guid}/patient")]
    public async Task<ActionResult<PatientDto>> AddPatient(Guid userId, CreatePatientRequestDto requestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userExists = await dbContext.Users.AnyAsync(u => u.UserId == userId);
        if (!userExists)
        {
            return NotFound($"User with ID {userId} not found.");
        }

        var newPatient = new Patient
        {
            UserId = userId,
            Name = requestDto.Name,
            DateOfBirth = requestDto.DateOfBirth,
            HeightInCm = requestDto.HeightInCm,
            WeightInKg = requestDto.WeightInKg,
            MedicalConditions = requestDto.MedicalConditions
        };
        dbContext.Patients.Add(newPatient);
        await dbContext.SaveChangesAsync();

        return Ok(new PatientDto
        {
            PatientId = newPatient.PatientId,
            Name = newPatient.Name,
            DateOfBirth = newPatient.DateOfBirth,
            HeightInCm = newPatient.HeightInCm,
            WeightInKg = newPatient.WeightInKg,
        });
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{userId:guid}/patient/{patientId:guid}")]
    public async Task<ActionResult> DeletePatient(Guid userId, Guid patientId)
    {
        var userExists = await dbContext.Users.AnyAsync(u => u.UserId == userId);
        if (!userExists)
        {
            return NotFound($"User with ID {userId} not found.");
        }

        int n = await dbContext.Patients.Where(p => p.PatientId == patientId && p.UserId == userId).ExecuteDeleteAsync();
        if (n == 0)
        {
            return NotFound($"Patient with ID {patientId} not found for user with ID {userId}.");
        }

        return NoContent();
    }

    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{userId:guid}/patient/{patientId:guid}")]
    public async Task<ActionResult<PatientDto>> GetPatientById(Guid userId, Guid patientId)
    {
        var userExists = await dbContext.Users.AnyAsync(u => u.UserId == userId);
        if (!userExists)
        {
            return NotFound($"User with ID {userId} not found.");
        }

        var patient = await dbContext.Patients
            .Where(p => p.PatientId == patientId && p.UserId == userId)
            .Select(p => new PatientDto
            {
                PatientId = p.PatientId,
                Name = p.Name,
                DateOfBirth = p.DateOfBirth,
                HeightInCm = p.HeightInCm,
                WeightInKg = p.WeightInKg,
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (patient == null)
        {
            return NotFound($"Patient with ID {patientId} not found for user with ID {userId}.");
        }

        return Ok(patient);
    }

    [ProducesResponseType(typeof(IEnumerable<PatientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{userId:guid}/patient")]
    public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatientsByUserId(Guid userId)
    {
        var userExists = await dbContext.Users.AnyAsync(u => u.UserId == userId);
        if (!userExists)
        {
            return NotFound($"User with ID {userId} not found.");
        }

        var patients = await dbContext.Patients
            .Where(p => p.UserId == userId)
            .Select(p => new PatientDto
            {
                PatientId = p.PatientId,
                Name = p.Name,
                DateOfBirth = p.DateOfBirth,
                HeightInCm = p.HeightInCm,
                WeightInKg = p.WeightInKg,
            })
            .AsNoTracking()
            .ToListAsync();

        return Ok(patients);
    }

    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{userId:guid}/patient/{patientId:guid}")]
    public async Task<ActionResult<PatientDto>> UpdatePatient(Guid userId, Guid patientId, [FromBody] UpdatePatientRequestDto requestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userExists = await dbContext.Users.AnyAsync(u => u.UserId == userId);
        if (!userExists)
        {
            return NotFound($"User with ID {userId} not found.");
        }

        var patient = await dbContext.Patients.FirstOrDefaultAsync(p => p.PatientId == patientId && p.UserId == userId);
        if (patient == null)
        {
            return NotFound($"Patient with ID {patientId} not found for user with ID {userId}.");
        }

        patient.Name = requestDto.Name;
        patient.DateOfBirth = requestDto.DateOfBirth;
        patient.HeightInCm = requestDto.HeightInCm;
        patient.WeightInKg = requestDto.WeightInKg;
        patient.MedicalConditions = requestDto.MedicalConditions;

        await dbContext.SaveChangesAsync();

        return Ok(new PatientDto
        {
            PatientId = patient.PatientId,
            Name = patient.Name,
            DateOfBirth = patient.DateOfBirth,
            HeightInCm = patient.HeightInCm,
            WeightInKg = patient.WeightInKg,
        });
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("{userId:guid}/booking")]
    public async Task<ActionResult> CreateBooking([FromRoute] Guid userId, [FromBody] CreateBookingRequestDto requestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var validationTask = Task.WhenAll([
            dbContext.Users.AnyAsync(u => u.UserId == userId),
            dbContext.Patients.AnyAsync(p => p.PatientId == requestDto.PatientId && p.UserId == userId),
            dbContext.Addresses.AnyAsync(a => a.AddressId == requestDto.AddressId && a.UserId == userId),
            dbContext.Nurses.AnyAsync(n => n.NurseId == requestDto.NurseId),
            dbContext.Bookings.HasOverlappingTimeRange(requestDto.NurseId, requestDto.StartTime, requestDto.EndTime),
            dbContext.NurseLeaves.HasOverlappingTimeRange(requestDto.NurseId, requestDto.StartTime, requestDto.EndTime)
        ]);

        var validationResult = await validationTask;
        if (!validationResult[0])
        {
            return NotFound($"User with ID {userId} not found.");
        }
        if (!validationResult[1])
        {
            return NotFound($"Patient with ID {requestDto.PatientId} not found for user with ID {userId}.");
        }
        if (!validationResult[2])
        {
            return NotFound($"Address with ID {requestDto.AddressId} not found for user with ID {userId}.");
        }
        if (!validationResult[3])
        {
            return NotFound($"Nurse with ID {requestDto.NurseId} not found.");
        }
        if (validationResult[4])
        {
            return BadRequest("The selected nurse has conflicting bookings in the specified time range.");
        }
        if (validationResult[5])
        {
            return BadRequest("The selected nurse is on leave during the specified time range.");
        }

        var newBooking = new Booking
        {
            UserId = userId,
            PatientId = requestDto.PatientId,
            NurseId = requestDto.NurseId,
            AddressId = requestDto.AddressId,
            StartTime = requestDto.StartTime,
            EndTime = requestDto.EndTime,
            Status = BookingStatus.Pending,
        };

        dbContext.Bookings.Add(newBooking);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [ProducesResponseType(typeof(UserBookingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{userId:guid}/booking/{bookingId:guid}")]
    public async Task<ActionResult<UserBookingDto>> GetBookingById([FromRoute] Guid userId, [FromRoute] Guid bookingId)
    {
        var userExists = await dbContext.Users.AnyAsync(u => u.UserId == userId);
        if (!userExists)
        {
            return NotFound($"User with ID {userId} not found.");
        }

        var booking = await dbContext.Bookings
            .Where(b => b.BookingId == bookingId && b.UserId == userId)
            .Include(b => b.Nurse)
            .Include(b => b.Patient)
            .Include(b => b.Address)
            .Select(b => new UserBookingDto
            {
                BookingId = b.BookingId,
                ReferenceCode = b.ReferenceCode,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                Status = b.Status,
                Nurse = new NurseResponseDto
                {
                    NurseId = b.Nurse!.NurseId,
                    Name = b.Nurse.Name,
                    DateOfBirth = b.Nurse.DateOfBirth,
                    Gender = b.Nurse.Gender,
                    PhoneNumber = b.Nurse.PhoneNumber,
                    GraduatedFrom = b.Nurse.GraduatedFrom,
                    LicenseNumber = b.Nurse.LicenseNumber,
                    ProfileImageUrl = b.Nurse.ProfileImageUrl,
                },
                Patient = new PatientDto
                {
                    PatientId = b.Patient!.PatientId,
                    Name = b.Patient.Name,
                    DateOfBirth = b.Patient.DateOfBirth,
                    HeightInCm = b.Patient.HeightInCm,
                    WeightInKg = b.Patient.WeightInKg,
                },
                Address = new AddressResponseDto
                {
                    AddressId = b.Address!.AddressId,
                    Country = b.Address.Country,
                    Province = b.Address.Province,
                    City = b.Address.City,
                    AddressLine = b.Address.AddressLine,
                    Coordinates = new CoordinatesDto(b.Address.Coordinates),
                },
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();
        if (booking == null)
        {
            return NotFound($"Booking with ID {bookingId} not found for user with ID {userId}.");
        }

        return Ok(booking);
    }


}
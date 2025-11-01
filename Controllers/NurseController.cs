using ElderConnectApi.Controllers.Dto.Request;
using ElderConnectApi.Controllers.Dto.Response;
using ElderConnectApi.Data;
using ElderConnectApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElderConnectApi.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class NurseController(ElderConnectDbContext dbContext) : ControllerBase
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<NurseResponseDto>> CreateNurse(CreateNurseRequestDto dto)
    {
        var nurse = new Nurse
        {
            NurseId = Guid.NewGuid(),
            Name = dto.Name,
            DateOfBirth = dto.DateOfBirth,
            Email = dto.Email,
            Gender = dto.Gender,
            PhoneNumber = dto.PhoneNumber,
            GraduatedFrom = dto.GraduatedFrom,
            LicenseNumber = dto.LicenseNumber,
            ProfileImageUrl = dto.ProfileImageUrl
        };
        dbContext.Nurses.Add(nurse);
        await dbContext.SaveChangesAsync();

        return Ok(new NurseResponseDto
        {
            NurseId = nurse.NurseId,
            Name = nurse.Name,
            DateOfBirth = nurse.DateOfBirth,
            Gender = nurse.Gender,
            PhoneNumber = nurse.PhoneNumber,
            GraduatedFrom = nurse.GraduatedFrom,
            LicenseNumber = nurse.LicenseNumber,
            ProfileImageUrl = nurse.ProfileImageUrl
        });
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("nurse/{nurseId:guid}")]
    public async Task<ActionResult<NurseResponseDto>> GetNurseById(Guid nurseId)
    {
        var nurse = await dbContext.Nurses.FindAsync(nurseId);
        if (nurse == null)
        {
            return NotFound($"Nurse with ID {nurseId} not found.");
        }

        return Ok(new NurseResponseDto
        {
            NurseId = nurse.NurseId,
            Name = nurse.Name,
            DateOfBirth = nurse.DateOfBirth,
            Gender = nurse.Gender,
            PhoneNumber = nurse.PhoneNumber,
            GraduatedFrom = nurse.GraduatedFrom,
            LicenseNumber = nurse.LicenseNumber,
            ProfileImageUrl = nurse.ProfileImageUrl
        });
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{nurseId:guid}/has-bookings-between")]
    public async Task<ActionResult> NurseHasBookingsBetween(Guid nurseId, DateTimeOffset start, DateTimeOffset end)
    {
        var nurseExists = await dbContext.Nurses.AnyAsync(n => n.NurseId == nurseId);
        if (!nurseExists)
        {
            return NotFound($"Nurse with ID {nurseId} not found.");
        }

        DateTimeOffset startUtc = start.ToUniversalTime();
        DateTimeOffset endUtc = end.ToUniversalTime();

        if (endUtc <= startUtc)
        {
            return BadRequest("End time must be after start time.");
        }

        var hasConflictingBookings = await dbContext.Bookings
            .Where(b => b.StartTime < endUtc && b.EndTime > startUtc)
            .AnyAsync();

        if (hasConflictingBookings)
        {
            return Ok();
        }
        else
        {
            return NoContent();
        }
    }


    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{nurseId:guid}/unavailable-dates")]
    public async Task<ActionResult> GetUnavailableDates(Guid nurseId)
    {
        var nurseExists = await dbContext.Nurses.AnyAsync(n => n.NurseId == nurseId);
        if (!nurseExists)
        {
            return NotFound($"Nurse with ID {nurseId} not found.");
        }

        var dateRangeList = await dbContext.Bookings
            .Where(b => b.NurseId == nurseId && b.StartTime > DateTimeOffset.UtcNow)
            .Select(b => new
            {
                StartTime = DateOnly.FromDateTime(b.StartTime.DateTime),
                EndTime = DateOnly.FromDateTime(b.EndTime.DateTime)
            })
            .ToListAsync();
        var booked = dateRangeList.SelectMany(b =>
        {
            var dates = new List<DateOnly>();
            for (var date = b.StartTime; date <= b.EndTime; date = date.AddDays(1))
            {
                dates.Add(date);
            }
            return dates;
        });
        var futureLeaves = await dbContext.NurseLeaves
            .Where(l => l.NurseId == nurseId && l.LeaveDate >= DateOnly.FromDateTime(DateTime.UtcNow))
            .Select(l => l.LeaveDate)
            .ToListAsync();

        return Ok(new NurseUnavailableDatesResponseDto
        {
            UnavailableDates = Enumerable.Concat(booked, futureLeaves)
        });
    }
}
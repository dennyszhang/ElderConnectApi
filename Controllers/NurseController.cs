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

        var hasBookings = await dbContext.Nurses
            .Where(n => n.NurseId == nurseId)
            .AnyAsync();

        if (hasBookings)
        {
            return Ok();
        }
        else
        {
            return NoContent();
        }
    }
}
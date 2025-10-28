using System.ComponentModel.DataAnnotations;

namespace ElderConnectApi.Controllers.Dto.Request;

public class CreateUserAddressRequestDto
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Country { get; set; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Province { get; set; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string City { get; set; } = null!;

    [Required]
    [StringLength(200, MinimumLength = 1)]    
    public string AddressLine { get; set; } = null!;

    [Required]
    public CoordinatesDto Coordinates { get; set; } = null!;
}
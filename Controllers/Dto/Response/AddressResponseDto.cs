using NetTopologySuite.Geometries;

namespace ElderConnectApi.Controllers.Dto.Response;

public class AddressResponseDto
{
    public Guid AddressId { get; set; }
    public string Country { get; set; } = null!;
    public string Province { get; set; } = null!;
    public string City { get; set; } = null!;
    public string? AddressLine { get; set; }
    public CoordinatesDto Coordinates { get; set; } = null!;
}
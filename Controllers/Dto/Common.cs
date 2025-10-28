using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace ElderConnectApi.Controllers.Dto;

public record CoordinatesDto
{
    [Required]
    [Range(-90.0, 90.0)]
    public double Latitude { get; set; }

    [Required]
    [Range(-180.0, 180.0)]
    public double Longitude { get; set; }

    public CoordinatesDto(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public CoordinatesDto() { }
    public CoordinatesDto(Point point)
    {
        Latitude = point.Y;
        Longitude = point.X;
    }
}

public class IsFutureDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateTimeOffset dateTimeOffset)
        {
            if (dateTimeOffset <= DateTimeOffset.Now)
            {
                return new ValidationResult("The date must be in the future.");
            }
            return ValidationResult.Success;
        }
        return new ValidationResult("Invalid date format.");
    }
}

public class IsDateGreaterThanAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public IsDateGreaterThanAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var currentValue = (DateTimeOffset?)value;

        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
        if (property == null)
            throw new ArgumentException("Property with this name not found");

        var comparisonValue = (DateTimeOffset?)property.GetValue(validationContext.ObjectInstance);

        if (currentValue != null && comparisonValue != null)
        {
            if (currentValue <= comparisonValue)
            {
                return new ValidationResult(ErrorMessage ?? $"{validationContext.MemberName} must be greater than {_comparisonProperty}.");
            }
        }

        return ValidationResult.Success;
    }
}
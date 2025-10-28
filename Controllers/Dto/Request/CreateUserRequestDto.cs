using System.ComponentModel.DataAnnotations;
using System.Security;

namespace ElderConnectApi.Controllers.Dto.Request;

public class CreateUserRequestDto
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
    public string Name { get; set; } = null!;

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(15)]
    [Phone]
    public string PhoneNumber { get; set; } = null!;

    [DataType(DataType.Password)]
    public SecureString? Password { get; set; }
}
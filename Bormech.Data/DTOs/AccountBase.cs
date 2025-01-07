using System.ComponentModel.DataAnnotations;

namespace Bormech.Data.DTOs;

public class AccountBase
{
    [DataType(DataType.EmailAddress, ErrorMessage = "Niepoprawny adres email")]
    [EmailAddress(ErrorMessage = "Niepoprawny adres email")]
    [Required(ErrorMessage = "Email jest wymagany")]
    public string? Email { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Hasło jest wymagane")]
    [MinLength(6, ErrorMessage = "Hasło musi mieć co najmniej 6 znaków")]
    public string? Password { get; set; }
}
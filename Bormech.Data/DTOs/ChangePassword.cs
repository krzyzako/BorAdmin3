using System.ComponentModel.DataAnnotations;

namespace Bormech.Data.DTOs;

public class ChangePassword:AccountBase
{
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Hasło jest wymagane")]
    [MinLength(6, ErrorMessage = "Hasło musi mieć co najmniej 6 znaków")]
    public string? Password { get; set; }
    
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Hasło jest wymagane")]
    [MinLength(6, ErrorMessage = "Hasło musi mieć co najmniej 6 znaków")]
    public string? NewPassword { get; set; } 
    
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword), ErrorMessage = "Hasła nie pasują do siebie")]
    [Required(ErrorMessage = "Potwierdzenie hasła jest wymagane")]
    public string? ConfirmNewPassword { get; set; }
}
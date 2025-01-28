using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Bormech.Data.Validate;

public class CertificationNumberValidationAttribute:ValidationAttribute
{
    private static readonly Regex Regex = new(@"^E20\s67R-0\d\s\d{4}$");

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string certificationNumber || !Regex.IsMatch(certificationNumber.Trim()))
        {
            return new ValidationResult("Certification number must be in format 'E20 67R-0X XXXX'.");
        }

        return ValidationResult.Success;
    }
    public static string GetPattern() => @"^E20\s67R-0\d\s\d{4}$";
}
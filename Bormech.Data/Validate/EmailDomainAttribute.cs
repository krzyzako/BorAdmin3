using System.ComponentModel.DataAnnotations;

namespace Bormech.Data.Validate;

public class EmailDomainAttribute : ValidationAttribute
{
    private readonly string _allowedDomain;

    public EmailDomainAttribute(string allowedDomain)
    {
        _allowedDomain = allowedDomain;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string email && !string.IsNullOrWhiteSpace(email))
        {
            var emailParts = email.Split('@');
            if (emailParts.Length == 2 && emailParts[1].Equals(_allowedDomain, StringComparison.OrdinalIgnoreCase))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"Adres e-mail musi pochodzić z domeny {_allowedDomain}.");
        }

        return new ValidationResult("Niepoprawny adres e-mail.");
    }
}
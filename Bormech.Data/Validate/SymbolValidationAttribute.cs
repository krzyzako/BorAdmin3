using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Bormech.Data.Entities.Approvals;

namespace Bormech.Data.Validate;

public class SymbolValidationAttribute:ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var symbol = value as string;
        var certificationTypeProperty = validationContext.ObjectType.GetProperty("CertificationType");

        if (certificationTypeProperty == null)
        {
            return new ValidationResult("Unknown CertificationType property.");
        }

        var certificationType = (TankCertificationType)(certificationTypeProperty.GetValue(validationContext.ObjectInstance) ?? throw new InvalidOperationException());

        // Definiujemy regexy dla różnych typów certyfikacji
        var regexToroidalInternal = new Regex(@"^TI-\d{4}$"); // np. "TI-1234"
        var regexToroidalExternal = new Regex(@"^TE-\d{4}$"); // np. "TE-5678"

        switch (certificationType)
        {
            case TankCertificationType.ToroidalInternal:
                if (string.IsNullOrEmpty(symbol) || !regexToroidalInternal.IsMatch(symbol))
                {
                    return new ValidationResult("Symbol must be in format 'TI-XXXX' for Toroidal Internal.");
                }
                break;

            case TankCertificationType.ToroidalExternal:
                if (string.IsNullOrEmpty(symbol) || !regexToroidalExternal.IsMatch(symbol))
                {
                    return new ValidationResult("Symbol must be in format 'TE-XXXX' for Toroidal External.");
                }
                break;

            case TankCertificationType.Cylindrical:
                // Dla Cylindrical brak wymagań, dowolny lub null
                break;

            default:
                return new ValidationResult("Invalid CertificationType.");
        }

        return ValidationResult.Success;
    }
}
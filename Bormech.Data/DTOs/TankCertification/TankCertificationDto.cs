using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Bormech.Data.Converter;
using Bormech.Data.Entities.Approvals;
using Bormech.Data.Validate;

namespace Bormech.Data.DTOs.TankCertification;

public class TankCertificationDto
{
    [CertificationNumberValidation]
    public string CertificationNumber { get; set; }
    
    [SymbolValidation]
    public string Symbol { get; set; }
    
    public int Height { get; set; }
    public int Width { get; set; }
    public int Capacity { get; set; }
    public int Dimeter { get; set; }
    /// <summary>
    /// Certification type (returns `Display(Name)`)
    /// </summary>
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))] 
    public TankCertificationType CertificationType { get; set; }
    
}



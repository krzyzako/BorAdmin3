using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bormech.Data.Entities.Approvals;

/// <summary>
/// Reprezentacja homologacji zbiornika LPG w bazie danych.
/// </summary>
[Table("TankCertifications")]
public class TankCertification
{

    public int Id { get; set; }
    
    [Required]
    public TankCertificationType TankCertificationType { get; set; } 
    
    [Required]
    [StringLength(50)]
    public string CertificationNumber { get; set; }

    [Required]
    [StringLength(50)]
    public string Symbol { get; set; }
    
    [Required]
    public int Height { get; set; }
    
    [Required]
    public int Width { get; set; }
    
    [Required]
    public int Capacity { get; set; }
    
    [Required]
    public int Diameter { get; set; }
    
}
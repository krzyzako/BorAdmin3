using System.ComponentModel.DataAnnotations;

namespace Bormech.Data.Entities.Approvals;

public enum TankCertificationType
{
    [Display(Name = "Teroidalny wewnętrzny")]
    ToroidalInternal = 1,
    [Display(Name = "Teroidalny zewnętrzny")]
    ToroidalExternal = 2,
    [Display(Name = "Toroidalny zewnętrzny pełny")]
    ToroidalExternalFull = 3,
    [Display(Name = "Walcowy")]
    Cylindrical = 4
}
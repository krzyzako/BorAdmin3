using Bormech.Data.Entities;
using Bormech.Data.Entities.Approvals;

using Microsoft.EntityFrameworkCore;

namespace Bormech.Server.Liblary.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Employe> Employes { get; set; }
    public DbSet<GeneralDepartment> GeneralDepartments { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Town> Towns { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<SystemRole> SystemRoles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RefreshTokenInfo> RefreshTokenInfos { get; set; }
    public DbSet<TankCertification> TankCertifications { get; set; }
}
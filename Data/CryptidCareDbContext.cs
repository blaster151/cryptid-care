using CryptidCare.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptidCare.Data;

public class CryptidCareDbContext : DbContext
{
    public CryptidCareDbContext(DbContextOptions<CryptidCareDbContext> options) : base(options) { }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Medicine> Medicines => Set<Medicine>();
    public DbSet<Claim> Claims => Set<Claim>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>(e =>
        {
            e.ToTable("Patients");
            e.HasKey(p => p.Id);
            e.Property(p => p.Species).HasConversion<string>().HasMaxLength(50);
            e.Property(p => p.Name).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<Medicine>(e =>
        {
            e.ToTable("Medicines");
            e.HasKey(m => m.Id);
            e.Property(m => m.Name).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<Claim>(e =>
        {
            e.ToTable("Claims");
            e.HasKey(c => c.Id);
            e.Property(c => c.Status).HasConversion<string>().HasMaxLength(20);
            e.Property(c => c.ExternalReferenceId).HasMaxLength(100);
            e.Property(c => c.RejectionReason).HasMaxLength(500);
            e.Property(c => c.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            e.HasOne(c => c.Patient).WithMany().HasForeignKey(c => c.PatientId);
            e.HasOne(c => c.Medicine).WithMany().HasForeignKey(c => c.MedicineId);
        });
    }
}

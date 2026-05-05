using CryptidCare.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptidCare.Data.Repositories;

public class ClaimRepository : IClaimRepository
{
    private readonly CryptidCareDbContext _db;
    public ClaimRepository(CryptidCareDbContext db) => _db = db;

    public async Task AddAsync(Claim claim)
    {
        _db.Claims.Add(claim);
        await _db.SaveChangesAsync();
    }

    public async Task<List<Claim>> GetRecentApprovedAsync(Guid patientId, Guid medicineId, int withinDays)
    {
        var cutoff = DateTime.UtcNow.AddDays(-withinDays);
        return await _db.Claims
            .Where(c => c.PatientId == patientId
                     && c.MedicineId == medicineId
                     && c.Status == ClaimStatus.Approved
                     && c.CreatedAt >= cutoff)
            .ToListAsync();
    }
}

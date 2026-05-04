using CryptidCare.Models;

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
}

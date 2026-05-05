using CryptidCare.Models;

namespace CryptidCare.Data.Repositories;

public interface IClaimRepository
{
    Task AddAsync(Claim claim);
    Task<List<Claim>> GetRecentApprovedAsync(Guid patientId, Guid medicineId, int withinDays);
}

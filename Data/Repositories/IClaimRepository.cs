using CryptidCare.Models;

namespace CryptidCare.Data.Repositories;

public interface IClaimRepository
{
    Task AddAsync(Claim claim);
}

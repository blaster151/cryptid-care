using CryptidCare.Models;

namespace CryptidCare.Data.Repositories;

public interface IMedicineRepository
{
    Task<Medicine?> GetByIdAsync(Guid id);
}

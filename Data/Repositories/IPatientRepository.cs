using CryptidCare.Models;

namespace CryptidCare.Data.Repositories;

public interface IPatientRepository
{
    Task<Patient?> GetByIdAsync(Guid id);
}

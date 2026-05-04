using CryptidCare.Models;

namespace CryptidCare.Data.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly CryptidCareDbContext _db;
    public PatientRepository(CryptidCareDbContext db) => _db = db;

    public async Task<Patient?> GetByIdAsync(Guid id) => await _db.Patients.FindAsync(id);
}

using CryptidCare.Models;

namespace CryptidCare.Data.Repositories;

public class MedicineRepository : IMedicineRepository
{
    private readonly CryptidCareDbContext _db;
    public MedicineRepository(CryptidCareDbContext db) => _db = db;

    public async Task<Medicine?> GetByIdAsync(Guid id) => await _db.Medicines.FindAsync(id);
}

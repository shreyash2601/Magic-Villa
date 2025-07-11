using System;
using MagicVillaAPI.Data;
using MagicVillaAPI.Models;

namespace MagicVillaAPI.Repository.IRepository;

public class VillaRepository : Repository<Villa>, IVillaRepository
{
    private readonly ApplicationDbContext db;
    public VillaRepository(ApplicationDbContext _db) : base(_db)
    {
        db = _db;
    }
    public async Task UpdateAsync(Villa entity)
    {
        db.Villas.Update(entity);
        await SaveAsync();
    }
}

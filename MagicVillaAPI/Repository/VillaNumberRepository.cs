using System;
using MagicVillaAPI.Data;
using MagicVillaAPI.Models;
using MagicVillaAPI.Repository.IRepository;

namespace MagicVillaAPI.Repository;

public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
{
    private readonly ApplicationDbContext db;
    public VillaNumberRepository(ApplicationDbContext _db) : base(_db)
    {
        db = _db;
    }
    public async Task UpdateAsync(VillaNumber entity)
    {
        db.VillaNumbers.Update(entity);
        await SaveAsync();
    }
}

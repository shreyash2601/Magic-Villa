using System;
using System.Linq.Expressions;
using MagicVillaAPI.Data;
using MagicVillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MagicVillaAPI.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext db;
    internal DbSet<T> DbSet;
    public Repository(ApplicationDbContext _db)
    {
        db = _db;
        DbSet = db.Set<T>();
    }
    public async Task CreateAsync(T entity)
    {
        await db.AddAsync(entity);
        await SaveAsync();
    }

    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
    {
        IQueryable<T> query = DbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }
        return await query.ToListAsync();
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null)
    {
        IQueryable<T> query = DbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }
        return await query.FirstOrDefaultAsync();
    }

    public async Task RemoveAsync(T entity)
    {
        DbSet.Remove(entity);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await db.SaveChangesAsync();
    }
}

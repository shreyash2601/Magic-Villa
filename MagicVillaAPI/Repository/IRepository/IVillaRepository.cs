using System;
using MagicVillaAPI.Models;

namespace MagicVillaAPI.Repository.IRepository;

public interface IVillaRepository : IRepository<Villa>
{
    Task UpdateAsync(Villa entity);
}

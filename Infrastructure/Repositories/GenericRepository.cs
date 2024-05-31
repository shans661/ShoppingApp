using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext Context;
        public GenericRepository(StoreContext context)
        {
            Context = context;
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await Context.Set<T>().FindAsync(id); 
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await Context.Set<T>().Tolistasync();
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetFileAPI.Repositories.Interfaces;

namespace NetFileAPI.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private DbContext Db { get; set; }

        protected Repository(DbContext db)
        {
            this.Db = db;
        }

        public async Task AddAsync(TEntity model)
        {
            await Db.Set<TEntity>().AddAsync(model);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Db.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            var entity = await Db.Set<TEntity>().FindAsync(id);
            return entity ?? new TEntity();
        }

        public void Modify(TEntity model)
        {
            Db.Entry<TEntity>(model).State = EntityState.Modified;
        }

        public void Delete(TEntity model)
        {
            Db.Set<TEntity>().Remove(model);
        }

        public async Task DeleteByIdAsync(object id)
        {
            var entity = await Db.Set<TEntity>().FindAsync(id);
            if (entity != null)
            {
                Db.Set<TEntity>().Remove(entity);
            }
        }

    
    }
}
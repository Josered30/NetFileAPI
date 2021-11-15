using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetFileAPI.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        Task AddAsync(TEntity model);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(object id);
        void Modify(TEntity model);
        void Delete(TEntity model);
        Task DeleteByIdAsync(object id);
    }
}
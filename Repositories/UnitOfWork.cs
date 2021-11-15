using System.Threading.Tasks;
using NetFileAPI.Database;
using NetFileAPI.Repositories.Interfaces;

namespace NetFileAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MySqlDbContext _context;

        public UnitOfWork(MySqlDbContext context)
        {
            _context = context;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
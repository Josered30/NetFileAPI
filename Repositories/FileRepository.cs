using NetFileAPI.Database;
using NetFileAPI.Models;
using NetFileAPI.Repositories.Interfaces;

namespace NetFileAPI.Repositories
{
    public class FileRepository : Repository<FileModel>, IFileRepository
    {
        public FileRepository(MySqlDbContext context) : base(context)
        {
        }
    }
};
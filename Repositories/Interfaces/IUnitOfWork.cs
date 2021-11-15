using System.Threading.Tasks;

namespace NetFileAPI.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
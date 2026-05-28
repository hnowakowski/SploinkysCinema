using SploinkyAPI.Models;

namespace SploinkyAPI.Controllers
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAll();
        Task<List<T>> Get(Guid id);
    }
}

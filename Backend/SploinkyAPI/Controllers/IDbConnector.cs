using SploinkyAPI.Models;
using System.Collections;

namespace SploinkyAPI.Controllers
{
    public interface IDbConnector
    {
        public Task<T> Query<T>(string query);
        public List<Reservation> TestQuery(); //temp
    }
}

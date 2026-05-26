using SploinkyAPI.Models;
using System.Collections;

namespace SploinkyAPI.Controllers
{
    public interface IDbConnector
    {
        public Task<Cassandra.RowSet> Query(Cassandra.IStatement statement);
    }
}

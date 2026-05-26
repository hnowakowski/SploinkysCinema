using Cassandra;
using Microsoft.AspNetCore.Mvc;
using SploinkyAPI.Models;
using System.Collections;
using System.ComponentModel;

namespace SploinkyAPI.Controllers
{
    public class DbConnector : IDbConnector
    {
        private Cassandra.ICluster _cluster;
        public readonly Cassandra.ISession session;

        public async Task<Cassandra.RowSet> Query(Cassandra.IStatement statement)
        {
            try
            {
                return await session.ExecuteAsync(statement);
            }
            catch (Exception e)
            {
                throw new Exception("Query failed", e);
            }
        }

        public DbConnector() // TODO: read db address and port from some config yaml
        {
            _cluster = Cluster.Builder().AddContactPoints("127.0.0.1").WithPort(9042).Build();
            session = _cluster.Connect("reservations");
            Console.WriteLine($"Connected to {session.Cluster}");
        }

    }
}

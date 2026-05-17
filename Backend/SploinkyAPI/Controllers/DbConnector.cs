using Cassandra;
using Microsoft.AspNetCore.Mvc;
using SploinkyAPI.Models;
using System.Collections;
using System.ComponentModel;

namespace SploinkyAPI.Controllers
{
    public class DbConnector : IDbConnector
    {
        private ICluster _cluster;
        private Cassandra.ISession _session;

        public async Task<T> Query<T>(string query)
        {
            throw new NotImplementedException();
        }
        
        public List<Reservation> TestQuery() //temp
        {
            Cassandra.RowSet response = _session.Execute("select * from reservation;");
            List<Reservation> res = new List<Reservation>();
            foreach (Cassandra.Row row in response)
            {
                res.Add(Reservation.FromDBRow(row));
            }
            return res;
        }

        public DbConnector() // temp
        {
            _cluster = Cluster.Builder().AddContactPoints("127.0.0.1").WithPort(9042).Build();
            _session = _cluster.Connect("reservations");
            Console.WriteLine($"Connected to {_session.Cluster}");
        }

    }
}

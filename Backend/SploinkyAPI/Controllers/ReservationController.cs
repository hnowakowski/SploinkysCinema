using Cassandra;
using Cassandra.Mapping;
using Microsoft.AspNetCore.Mvc;
using SploinkyAPI.Models;
using System.Net;
using SploinkyAPI.Controllers;

namespace SploinkyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ILogger<ReservationController> _logger;
        private readonly Cassandra.ISession _session;
        private readonly IRepository<Reservation> _connector;

        public ReservationController(Cassandra.ISession session, IRepository<Reservation> connector, ILogger<ReservationController> logger)
        {
            _logger = logger;
            _session = session;
            _connector = connector;
        }

        [HttpGet]
        [Route("api/reservations/getall")]
        public async Task<List<Reservation>> GetAll()
        {
            return await _connector.GetAll();
        }

        [HttpGet]
        [Route("api/reservations/get")]
        public async Task<List<Reservation>> Get(int Id)
        {
            throw new NotImplementedException();
            //Reservation user = mapper.Single<Reservation>("SELECT * FROM reservation WHERE id = ?", Id);
            Cassandra.RowSet response = await _session.ExecuteAsync(new Cassandra.SimpleStatement("SELECT * FROM reservation;"));
            List<Reservation> list = new List<Reservation>();
            foreach (var item in response)
            {
                list.Add(Reservation.FromDBRow(item));
            }
            Console.WriteLine(list.Count);
            return list;
        }

        [HttpPost]
        [Route("api/reservations/put")]
        public async Task<HttpStatusCode> Put(Reservation reservation)
        {
            throw new NotImplementedException();
        }


    }
}

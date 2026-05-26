using Cassandra;
using Cassandra.Mapping;
using Microsoft.AspNetCore.Mvc;
using SploinkyAPI.Models;
using System.Net;

namespace SploinkyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ILogger<ReservationController> _logger;
        private readonly IDbConnector _dbConnector;

        public ReservationController(ILogger<ReservationController> logger)
        {
            _logger = logger;
            _dbConnector = new DbConnector();
        }

        [HttpGet]
        [Route("api/reservations/getall")]
        public async Task<List<Reservation>> GetAll()
        {
            Cassandra.RowSet response = await _dbConnector.Query(new Cassandra.SimpleStatement("SELECT * FROM reservation;"));
            List<Reservation> list = new List<Reservation>();
            foreach (var item in response)
            {
                list.Add(Reservation.FromDBRow(item));
            }
            Console.WriteLine(list.Count);
            return list;
        }

        [HttpGet]
        [Route("api/reservations/get")]
        public async Task<List<Reservation>> Get(int Id)
        {
            throw new NotImplementedException();
            //Reservation user = mapper.Single<Reservation>("SELECT * FROM reservation WHERE id = ?", Id);
            Cassandra.RowSet response = await _dbConnector.Query(new Cassandra.SimpleStatement("SELECT * FROM reservation;"));
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

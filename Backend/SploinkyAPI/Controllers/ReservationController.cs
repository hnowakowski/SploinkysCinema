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

        public ReservationController(Cassandra.ISession session, ILogger<ReservationController> logger)
        {
            _logger = logger;
            _session = session;
        }

        [HttpGet]
        [Route("api/reservations/getall")]
        public async Task<List<Reservation>> GetAll()
        {
            IMapper mapper = new Mapper(_session);
            IEnumerable<Reservation> reservations = await mapper.FetchAsync<Reservation>("SELECT * FROM RESERVATION;");
            return reservations.ToList();
        }

        [HttpGet]
        [Route("api/reservations/get")]
        public async Task<List<Reservation>> Get(int Id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("api/reservations/put")]
        public async Task<HttpStatusCode> Put(Reservation reservation)
        {
            throw new NotImplementedException();
        }


    }
}

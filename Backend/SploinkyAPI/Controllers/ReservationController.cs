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
        public async Task<Reservation> Get(Guid Id)
        {
            IMapper mapper = new Mapper(_session);
            // fetch returns an ienumerable so i have to grab it like that even if i only care about one item
            IEnumerable<Reservation> reservations = await mapper.FetchAsync<Reservation>("SELECT * FROM RESERVATION WHERE id = ?;", Id);
            return reservations.First(); //guids *should* be unique so i assume this will always have one element
        }

        [HttpPost]
        [Route("api/reservations/insert")]
        public async Task Insert(Reservation reservation)
        {
            IMapper mapper = new Mapper(_session);
            await mapper.InsertAsync<Reservation>(reservation);
        }

        [HttpPost]
        [Route("api/reservations/update")]
        public async Task Update(Reservation reservation)
        {
            IMapper mapper = new Mapper(_session);
            await mapper.UpdateAsync<Reservation>(reservation);
        }

        [HttpPost]
        [Route("api/reservations/delete")]
        // the mapper takes entire object as input but deletes ONLY based on the id, other attribs dont have to match
        public async Task Delete(Reservation reservation)
        {
            IMapper mapper = new Mapper(_session);
            await mapper.DeleteAsync<Reservation>(reservation);
        }
    }
}

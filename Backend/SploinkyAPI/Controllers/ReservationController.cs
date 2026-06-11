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

        // dont actually use this in the app, this is just for testing purposes
        [HttpGet]
        [Route("api/reservations/getall")]
        public async Task<List<Reservation>> GetAll()
        {
            IMapper mapper = new Mapper(_session);
            IEnumerable<Reservation> reservations = await mapper.FetchAsync<Reservation>("SELECT * FROM RESERVATIONS;");
            return reservations.ToList();
        }

        // get all reservations for a given movie when opening the movie seat-picking window
        [HttpGet]
        [Route("api/reservations/get")]
        public async Task<List<Reservation>> Get(Guid movieId)
        {
            IMapper mapper = new Mapper(_session);
            IEnumerable<Reservation> reservations = await mapper.FetchAsync<Reservation>("SELECT * FROM RESERVATIONS WHERE movie_id = ?;", movieId);
            return reservations.ToList();
        }

        // making a single reservation
        [HttpPost]
        [Route("api/reservations/insert")]
        public async Task Insert(Reservation reservation) // TODO: add status returns for stress tests later for if a seat was already taken
        {
            IMapper mapper = new Mapper(_session);
            await mapper.InsertAsync<Reservation>(reservation);
        }

        // changing a reservation? it's a requirement but i dont really have a good idea for it, i guess you can give a reserved seat to someone else lol
        // yeah so basically for changing the username on a reservation
        [HttpPost]
        [Route("api/reservations/update")]
        public async Task Update(Reservation reservation) 
        {
            IMapper mapper = new Mapper(_session);
            await mapper.UpdateAsync<Reservation>(reservation);
        }

        // cancelling a single reservation
        [HttpPost]
        [Route("api/reservations/delete")]
        // the mapper takes entire object as input but deletes ONLY based on the id, other attribs dont have to match
        // added an explicit condition to the query to make sure this does not accidentally nuke all the reservations on a given movie_id
        public async Task Delete(Guid movieId, int seat, int row)
        {
            IMapper mapper = new Mapper(_session);
            await mapper.DeleteAsync<Reservation>("WHERE movie_id = ? AND seat = ? AND row = ?", movieId, seat, row);
        }

        // cancelling all reservations made by a user on a given movie (pair requirement and for a stress test)
        [HttpPost]
        [Route("api/reservations/deleteall")]
        public async Task DeleteAll(Guid movieId, string username)
        {
            // make it all seperate requests in a loop to keep them atomic?
            throw new NotImplementedException();
        }
    }
}

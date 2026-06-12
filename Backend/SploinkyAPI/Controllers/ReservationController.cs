using Cassandra;
using Cassandra.Mapping;
using Microsoft.AspNetCore.Mvc;
using SploinkyAPI.Models;
using System.Net;
using SploinkyAPI.Controllers;
using System.Runtime.CompilerServices;

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
        public async Task<ObjectResult> Insert(Reservation reservation)
        {
            IMapper mapper = new Mapper(_session);
            DateTime lastUpdate = DateTime.Now;
            // execute with a session instead of insert with a mapper cause i need to specify the full query to add a check if seat was already taken and to get the status code
            SimpleStatement statement = new SimpleStatement("INSERT INTO RESERVATIONS (movie_id, movie_name, username, seat, row, last_update) VALUES (?, ?, ?, ?, ?, ?) IF NOT EXISTS;",
                    reservation.MovieId, reservation.MovieName, reservation.Username, reservation.Seat, reservation.Row, lastUpdate);
            statement.SetSerialConsistencyLevel(ConsistencyLevel.Serial);
            try
            {
                RowSet res = await _session.ExecuteAsync(statement);
                if (res.First().GetValue<bool>("[applied]"))
                {
                    return StatusCode(200, new { message = "Reservation made successfully" });
                }
                else
                {
                    return StatusCode(409, new { message = "Reservation failed, seat was taken" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Reservation failed", detail = ex.Message });
            }

        }

        // changing a reservation? it's a requirement but i dont really have a good idea for it, i guess you can give a reserved seat to someone else lol
        // yeah so basically for changing the username on a reservation
        [HttpPost]
        [Route("api/reservations/update")]
        public async Task<ObjectResult> Update(Reservation reservation, string newUsername) 
        {
            IMapper mapper = new Mapper(_session);
            SimpleStatement statement = new SimpleStatement("UPDATE RESERVATIONS SET username = ? WHERE movie_id = ? AND username = ? AND seat = ? AND row = ? IF EXISTS;",
                    newUsername, reservation.MovieId, reservation.Username, reservation.Seat, reservation.Row);
            statement.SetSerialConsistencyLevel(ConsistencyLevel.Serial);
            try
            {
                RowSet res = await _session.ExecuteAsync(statement);
                if (res.First().GetValue<bool>("[applied]"))
                {
                    return StatusCode(200, new { message = "Reservation updated successfully" });
                }
                else
                {
                    return StatusCode(409, new { message = "Update failed, valid reservation not found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Update failed", detail = ex.Message });
            }
        }

        // cancelling a single reservation
        [HttpPost]
        [Route("api/reservations/delete")]
        // the mapper takes entire object as input but deletes ONLY based on the id, other attribs dont have to match
        // also i need the IF EXISTS clause to prevent race condition shenanigans
        public async Task<ObjectResult> Delete(Reservation reservation)
        {
            IMapper mapper = new Mapper(_session);
            SimpleStatement statement = new SimpleStatement("DELETE FROM RESERVATIONS WHERE movie_id = ? AND seat = ? AND row = ? IF EXISTS",
                reservation.MovieId, reservation.Seat, reservation.Row);
            statement.SetSerialConsistencyLevel(ConsistencyLevel.Serial);
            try
            {
                RowSet res = await _session.ExecuteAsync(statement);
                if (res.First().GetValue<bool>("[applied]"))
                {
                    return StatusCode(200, new { message = "Reservation deleted successfully" });
                }
                else
                {
                    return StatusCode(409, new { message = "Delete failed, valid reservation not found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Delete failed", detail = ex.Message });
            }
        }

        // cancelling all reservations made by a user on a given movie, button in some corner (pair requirement and for a stress test)
        [HttpPost]
        [Route("api/reservations/deleteall")]
        public async Task DeleteAll(Guid movieId, string username)
        {
            // make it all seperate requests in a loop to keep them atomic?
            throw new NotImplementedException();
        }
    }
}

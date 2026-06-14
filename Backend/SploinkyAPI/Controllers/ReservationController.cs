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
        public async Task<ActionResult<List<Reservation>>> GetAll()
        {
            IMapper mapper = new Mapper(_session);
            try
            {
                IEnumerable<Reservation> reservations = await mapper.FetchAsync<Reservation>("SELECT * FROM RESERVATIONS;");
                List<Reservation>? res = reservations.ToList();
                if (res == null)
                {
                    return StatusCode(404, new { message = "Select failed, no reservations found" });
                }
                return StatusCode(200, res);
                }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Select failed: {ex.Message}" });
            }
        }

        // get all reservations for a given movie when opening the movie seat-picking window
        [HttpGet]
        [Route("api/reservations/getmovieseats")]
        public async Task<ActionResult<List<Reservation>>> Get(Guid movieId)
        {
            IMapper mapper = new Mapper(_session);
            try
            {
                IEnumerable<Reservation> reservations = await mapper.FetchAsync<Reservation>("SELECT * FROM RESERVATIONS WHERE movie_id = ?;", movieId);
                List<Reservation>? res = reservations.ToList();
                if (res == null)
                {
                    return StatusCode(404, new { message = "Select failed, no reservations found" });
                }
                return StatusCode(200, res);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Select failed: {ex.Message}" });
            }
        }

        // get specific reservation with a specific seat/row and user
        // gives null if no match found
        [HttpGet]
        [Route("api/reservations/getreservation")]
        public async Task<ActionResult<Reservation>> Get(Guid movieId, int seat, int row, string username)
        {
            IMapper mapper = new Mapper(_session);
            try
            {
                IEnumerable<Reservation> reservations = await mapper.FetchAsync<Reservation>(
                    "SELECT * FROM RESERVATIONS WHERE movie_id = ? AND seat = ? AND row = ? AND username = ? ALLOW FILTERING;", movieId, seat, row, username);
                Reservation? res = reservations.FirstOrDefault();
                if (res == null)
                {
                    return StatusCode(404, new { message = "Select failed, reservation not found" });
                }
                return StatusCode(200, res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Select failed: {ex.Message}" });
            }
        }

        // making a single reservation
        [HttpPost]
        [Route("api/reservations/post")]
        public async Task<ActionResult> Insert(Reservation reservation)
        {
            // execute with a session instead of insert with a mapper cause i need to specify the full query
            // to add a check if seat was already taken and to get the status code
            SimpleStatement statement = new SimpleStatement(
                "INSERT INTO RESERVATIONS (movie_id, movie_name, username, seat, row) VALUES (?, ?, ?, ?, ?) IF NOT EXISTS;",
                reservation.MovieId, reservation.MovieName, reservation.Username, reservation.Seat, reservation.Row);
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

        // for changing the username on a reservation
        [HttpPut]
        [Route("api/reservations/put")]
        public async Task<ActionResult> Update(Reservation reservation, string newUsername) 
        {
            SimpleStatement statement = new SimpleStatement("UPDATE RESERVATIONS SET username = ? WHERE movie_id = ? AND seat = ? AND row = ? IF username = ?;",
                    newUsername, reservation.MovieId, reservation.Seat, reservation.Row, reservation.Username);
            try
            {
                RowSet res = await _session.ExecuteAsync(statement);
                if (res.First().GetValue<bool>("[applied]"))
                {
                    return StatusCode(200, new { message = "Reservation updated successfully" });
                }
                else
                {
                    return StatusCode(404, new { message = "Update failed, valid reservation not found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Update failed: {ex.Message}" });
            }
        }

        // cancelling a single reservation
        [HttpDelete]
        [Route("api/reservations/delete")]
        // the mapper takes entire object as input but deletes ONLY based on the id, other attribs dont have to match
        // also i need the IF EXISTS clause to prevent async shenanigans
        public async Task<ActionResult> Delete(Reservation reservation)
        {
            SimpleStatement statement = new SimpleStatement("DELETE FROM RESERVATIONS WHERE movie_id = ? AND seat = ? AND row = ? IF EXISTS",
                reservation.MovieId, reservation.Seat, reservation.Row);
            try
            {
                RowSet res = await _session.ExecuteAsync(statement);
                if (res.First().GetValue<bool>("[applied]"))
                {
                    return StatusCode(200, new { message = "Reservation deleted successfully" });
                }
                else
                {
                    return StatusCode(404, new { message = "Delete failed, valid reservation not found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Delete failed: {ex.Message}" });
            }
        }

        // cancelling all reservations made by a user on a given movie, button in some corner (pair requirement and for a stress test)
        [HttpDelete]
        [Route("api/reservations/deleteall")]
        public async Task<ActionResult> DeleteAll(Guid movieId, string username)
        {
            IMapper mapper = new Mapper(_session);
            try
            {
                IEnumerable<Reservation> reservations = await mapper.FetchAsync<Reservation>(
                    "SELECT * FROM RESERVATIONS WHERE movie_id = ? AND username = ? ALLOW FILTERING;", movieId, username);
                List<Reservation>? res = reservations.ToList();
                if (res == null)
                {
                    return StatusCode(404, new { message = "Select failed, no reservations on a movie found" });
                }
                try
                {
                    foreach (Reservation r in res)
                    {
                        SimpleStatement statement = new SimpleStatement("DELETE FROM RESERVATIONS WHERE movie_id = ? AND seat = ? AND row = ? IF username = ?;",
                                movieId, r.Seat, r.Row, username);
                        await _session.ExecuteAsync(statement);
                    }
                    return StatusCode(200, new { message = "Bulk delete success" });
                }
                catch(Exception ex)
                {
                    return StatusCode(500, new { message = $"Delete failed: {ex.Message}" });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Selecting occupied seats failed: {ex.Message}" });
            }
        }
    }
}

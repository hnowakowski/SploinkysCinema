using Microsoft.AspNetCore.Mvc;
using SploinkyAPI.Models;

namespace SploinkyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ILogger<ReservationController> _logger;
        private IDbConnector _dbConnector;

        public ReservationController(ILogger<ReservationController> logger)
        {
            _logger = logger;
            _dbConnector = new DbConnector();
        }

        [HttpGet]
        [Route("api/reservations")]
        public IEnumerable<Reservation> Get()
        {
            List<Reservation> list = _dbConnector.TestQuery().ToList<Reservation>();
            Console.WriteLine(list.Count);
            return list;
        }


    }
}

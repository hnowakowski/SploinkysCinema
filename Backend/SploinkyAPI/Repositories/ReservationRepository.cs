using SploinkyAPI.Models;

namespace SploinkyAPI.Controllers
{
    public class ReservationRepository : IRepository<Reservation>
    {
        private readonly Cassandra.ISession _session;
        public async Task<List<Reservation>> GetAll()
        {
            Cassandra.RowSet response = await _session.ExecuteAsync(new Cassandra.SimpleStatement("SELECT * FROM reservation;"));
            List<Reservation> list = new List<Reservation>();
            foreach (var item in response)
            {
                list.Add(Reservation.FromDBRow(item));
            }
            Console.WriteLine(list.Count);
            return list;
        }

        public async Task<List<Reservation>> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public ReservationRepository(Cassandra.ISession session)
        {
            _session = session;
        }
    }
}

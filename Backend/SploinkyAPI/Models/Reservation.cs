using Cassandra;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SploinkyAPI.Models
{
    public class Reservation
    {
        public Guid MovieId { get; set; }
        public string MovieName { get; set; } = String.Empty;
        public string Username { get; set; } = String.Empty;
        public int Seat { get; set; }
        public int Row { get; set; }
        public DateTimeOffset LastUpdate { get; set; }

        public Reservation() {}

        public Reservation(Guid movieId, string movieName, string username, int seat, int row, DateTimeOffset lastUpdate)
        {
            MovieId = movieId;
            MovieName = movieName;
            Username = username;
            Seat = seat;
            Row = row;
            LastUpdate = lastUpdate;
        }
    }
}

using Cassandra;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SploinkyAPI.Models
{
    public class Reservation
    {
        [NotNull]
        public Guid Id { get; set; }
        [NotNull]
        public string Name { get; set; } = String.Empty;
        [NotNull]
        public string Surname { get; set; } = String.Empty;
        [NotNull]
        public int PlayId { get; set; }
        [NotNull]
        public int Seat { get; set; }
        [NotNull]
        public int Row { get; set; }

        public Reservation() {}

        
        public Reservation(Guid id, string name, string surname, int playid, int seat, int row)
        {
            this.Id = id;
            this.Name = name;
            this.Surname = surname;
            this.PlayId = playid;
            this.Seat = seat;
            this.Row = row;
        }
        
    }
}

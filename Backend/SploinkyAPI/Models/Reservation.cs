using Cassandra;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SploinkyAPI.Models
{
    public class Reservation : IDbItem<Reservation>
    {
        [NotNull]
        public int Id { get; protected set; }
        [NotNull]
        public string Name { get; protected set; } = String.Empty;
        [NotNull]
        public string Surname { get; protected set; } = String.Empty;
        [NotNull]
        public int PlayId { get; protected set; }
        [NotNull]
        public int Seat { get; protected set; }
        [NotNull]
        public int Row { get; protected set; }

        public static Reservation FromDBRow(Row row)
        {
            return new Reservation(row.GetValue<int>("id"), row.GetValue<string>("name"), row.GetValue<string>("surname"), row.GetValue<int>("play_id"), row.GetValue<int>("row"), row.GetValue<int>("seat"));
        }

        public Reservation LoadFromDb(int Id)
        {
            throw new NotImplementedException();
        }
        
        public Reservation(int id, string name, string surname, int playid, int seat, int row)
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

using Cassandra.Mapping;

namespace SploinkyAPI.Models
{
    public class ModelMappings : Mappings
    {
        public ModelMappings()
        {
            For<Reservation>().TableName("Reservation").PartitionKey(u => u.Id)
                .Column(u => u.Name, cm => cm.WithName("name"))
                .Column(u => u.Surname, cm => cm.WithName("surname"))
                .Column(u => u.PlayId, cm => cm.WithName("play_id"))
                .Column(u => u.Seat, cm => cm.WithName("seat"))
                .Column(u => u.Row, cm => cm.WithName("row"));
        }
    }
}

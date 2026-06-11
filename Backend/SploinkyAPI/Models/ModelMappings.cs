using Cassandra.Mapping;

namespace SploinkyAPI.Models
{
    public class ModelMappings : Mappings
    {
        public ModelMappings()
        {
            For<Movie>().TableName("Movies").PartitionKey(u => u.MovieId)
                .Column(u => u.MovieId, cm => cm.WithName("movie_id"))
                .Column(u => u.MovieName, cm => cm.WithName("movie_name"))
                .Column(u => u.ImagePath, cm => cm.WithName("image_path"));

            For<Reservation>().TableName("Reservations").PartitionKey(u => u.MovieId)
                .ClusteringKey(u => u.Seat)
                .ClusteringKey(u => u.Row)
                .Column(u => u.MovieId, cm => cm.WithName("movie_id"))
                .Column(u => u.MovieName, cm => cm.WithName("movie_name"))
                .Column(u => u.Username, cm => cm.WithName("username"))
                .Column(u => u.Seat, cm => cm.WithName("seat"))
                .Column(u => u.Row, cm => cm.WithName("row"))
                .Column(u => u.LastUpdate, cm => cm.WithName("last_update"));
        }
    }
}

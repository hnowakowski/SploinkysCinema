namespace SploinkyAPI.Models
{
    public class Movie
    {
        public Guid MovieId { get; set; }
        public string MovieName { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;

        public Movie() { }

        public Movie(Guid movieId, string movieName, string imagePath)
        {
            MovieId = movieId;
            MovieName = movieName;
            ImagePath = imagePath;
        }
    }
}

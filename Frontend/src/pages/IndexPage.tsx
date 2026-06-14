import { useEffect, useState } from "react";
import { getMovies } from "../services/apiHandler";
import type { Movie } from "../types/movie";
import { useNavigate } from "react-router-dom";
import Navbar from "../components/Navbar";

export default function IndexPage() {
  const [movies, setMovies] = useState<Movie[]>([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchMovies = async () => {
      try {
        const res = await getMovies();
        setMovies(res.data);
      } catch (ex) {
        console.error("Failed to fetch movies", ex);
      } finally {
        setLoading(false);
      }
    };
    fetchMovies();
  }, []);

  if (loading) {
    return <div>Fetching movie list from db please wait :)</div>;
  }

  return(
    <main>
      <Navbar />
      <h1>GO WATCH THESE</h1>
      <ul style={{ listStyle: "none", padding: 0, margin: 0, display: "flex", flexDirection: "column", alignItems: "center" }}>
        {movies.map((movie) => (
          <li key={movie.movieId}>
              <button onClick={() => navigate(`/movie/${movie.movieId}`)}>
                <img src={movie.imagePath} alt={`Pretend this is an image for ${movie.movieName}`} width={400} />
                <h3>{movie.movieName}</h3>
              </button>
            </li>
          ))}
        </ul>
      </main>
  );
}
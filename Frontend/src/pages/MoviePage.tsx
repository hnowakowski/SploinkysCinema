import { useEffect, useState } from "react";
import { useParams, useLocation } from "react-router-dom";
import type { Movie } from "../types/movie";
import type { Reservation } from "../types/reservation";
import { getMovie, getMovieSeats, deleteAll } from "../services/apiHandler";
import { useUsername } from "../store/username";
import SeatSelect from "../components/SeatSelect";
import Navbar from "../components/Navbar";

export default function MoviePage() {
    const {movieId} = useParams<{ movieId: string }>();
    const location = useLocation();
    const username = useUsername();

    const [movie, setMovie] = useState<Movie | null>(location.state?.movie || null);
    const [reservations, setReservations] = useState<Reservation[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const fetchMovie = async () => {
        if (!movieId || movie) return;
        try {
            const res = await getMovie(movieId);
            setMovie(res.data);
        } catch (ex: any) {
            setError(ex.response?.data?.message || 'Failed to fetch movie details');
        }
    };
    fetchMovie();

    const fetchReservations = async () => {
        if (!movieId) return;
        setLoading(true);
        try {
            const res = await getMovieSeats(movieId);
            setReservations(res.data);
            setError(null);
        } catch (ex: any) {
            if (ex.response?.status === 404) {
                setReservations([]);
            } else {
                setError(ex.response?.data?.message || 'Failed to fetch seat allocation');
            }
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchReservations();
    }, [movieId]);

    const handleConfirmDeleteAll = async () => {
        if (!movieId || !username) return;
        if (!confirm(`Are you ABSOLUTELY sure you want to cancel all your reservations for ${movie?.movieName}?`)) return;
        try{
            await deleteAll(movieId, username);
            fetchReservations();
        } catch (ex: any) {
            alert(ex.response?.data?.message || 'Failed to delete all reservations');
            setError(ex.response?.data?.message || 'Failed to delete all reservations');
        }
    };

    return (
         <>
      <Navbar />
        <main style={{ maxWidth: '800px', margin: '0 auto', padding: '2rem 1rem', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
        <div style={{ display: 'flex', alignItems: 'center', textAlign: 'center', gap: '1.5rem', marginBottom: '2rem' }}>
          <img src={movie?.imagePath} alt={movie?.movieName || 'Image isnt showing :('} width={60} height={90} style={{ objectFit: 'cover', borderRadius: '4px' }} />
          <div>
            <h1>{movie?.movieName || 'Movie name not fetched properly :('}</h1>
            <p>Select a seat to make a reservation</p>
          </div>
        </div>

        <div style={{ position: 'relative', padding: '2rem' }}>
          {username && <button onClick={handleConfirmDeleteAll} style={{ position: 'absolute', top: '1rem', right: '1rem' }}>Cancel all mine</button>}
          {loading && <p>Loading seats...</p>}
          {error && <p style={{ color: 'red' }}>{error}</p>}
          {!loading && !error && movieId && (
            <span style={{ textAlign: 'center', alignItems: 'center', display: 'flex', flexDirection: 'column', gap: 16 }}>
            <p>The screen is here btw</p>
            <SeatSelect movieId={movieId} movieName={movie?.movieName ?? ''} reservations={reservations} onRefresh={fetchReservations} />
            </span>
          )}
        </div>
      </main>
    </>
    )
}
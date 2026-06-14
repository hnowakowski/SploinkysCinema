import axios from "axios";
import type { Reservation } from "../types/reservation";
import type { Movie } from "../types/movie";

const api = axios.create({baseURL: "/api/reservations"});

export const getMovies = () =>
   api.get<Movie[]>('/getmovies');

export const getMovie = (movieId: string) =>
   api.get<Movie>('/getmovie', { params: { movieId } });

export const getMovieSeats = (movieId: string) =>
   api.get<Reservation[]>('/getmovieseats', { params: { movieId } });

export const getReservation = (movieId: string, seat: number, row: number, username: string) =>
  api.get<Reservation>('/getreservation', { params: { movieId, seat, row, username } });

export const postReservation = (reservation: Reservation) =>
  api.post('/post', reservation);

export const putReservation = (reservation: Reservation, newUsername: string) =>
  api.put('/put', reservation, { params: { newUsername } });

export const deleteReservation = (reservation: Reservation) =>
  api.delete('/delete', { data: reservation });

export const deleteAll = (movieId: string, username: string) =>
  api.delete('/deleteall', { params: { movieId, username } });
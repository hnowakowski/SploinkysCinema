import axios from "axios";
import type { Reservation } from "../types/reservation";
import type { Movie } from "../types/movie";

const BASE_URL = "https://localhost:7117/Reservation/api/reservations";

export const getMovies = () =>
   axios.get<Movie[]>(`${BASE_URL}/getmovies`);

export const getMovieSeats = (movieId: string) =>
   axios.get<number[][]>(`${BASE_URL}/getmovieseats`, { params: { movieId } });

export const getReservation = (movieId: string, seat: number, row: number, username: string) =>
  axios.get<Reservation>(`${BASE_URL}/getreservation`, { params: { movieId, seat, row, username } });

export const postReservation = (reservation: Reservation) =>
  axios.post(`${BASE_URL}/post`, reservation);

export const putReservation = (reservation: Reservation, newUsername: string) =>
  axios.put(`${BASE_URL}/put`, reservation, { params: { newUsername } });

export const deleteReservation = (reservation: Reservation) =>
  axios.delete(`${BASE_URL}`, { data: reservation });

export const deleteAll = (movieId: string, username: string) =>
  axios.delete(`${BASE_URL}/deleteall`, { params: { movieId, username } });
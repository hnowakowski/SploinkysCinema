import { useState } from "react";
import { useUsername } from "../store/username";
import { postReservation, deleteReservation, putReservation} from "../services/apiHandler";
import type { Reservation } from "../types/reservation";

interface Props {
    movieId: string,
    movieName: string,
    reservations: Reservation[],
    onRefresh: () => void;
}

type PopupState = null | { type: 'confirm', row: number, seat: number } |
 { type: 'taken', by: string } | {type: 'own'; reservation: Reservation};

export default function SeatSelect({ reservations, movieId, movieName, onRefresh }: Props){
    const username = useUsername();
    const [popup, setPopup] = useState<PopupState>(null);
    const [updateInput, setUpdateInput] = useState('');
    const [error, setError] = useState<string | null>(null);

    const closePopup = () => {setPopup(null); setError(null);};

    const getRes = (row: number, seat: number) => reservations.find(r => r.row === row && r.seat === seat);
    const seatColor = (row: number, seat: number) => {
        const res = getRes(row, seat);
        if (!res) return 'green';
        if (res.username === username) return 'orange';
        return 'red';
    }

    const onSeatClick = (row: number, seat: number) => {
        setError(null);
        const res = getRes(row, seat);
        if (!res) {
            setPopup({ type: 'confirm', row, seat });
        } else if (res.username === username) {
            setPopup({ type: 'own', reservation: res });
        }
        else{
            setPopup({ type: 'taken', by: res.username });
        }
    };

    const onReservation = async (row: number, seat: number) => {
        if (!username) {
            return setError('Who even is this brochacho 💀💀');
        }
        try {
            await postReservation({movieId, movieName, row, seat, username});
            closePopup();
            onRefresh();
        } catch (ex: any) {
            setError(ex.response?.data?.message || 'Failed to reserve seat');
        }
    };

    const onCancel = async (reservation: Reservation) => {
        try {
            await deleteReservation(reservation);
            closePopup();
            onRefresh();
        } catch (ex: any) {
            setError(ex.response?.data?.message || 'Failed to cancel reservation');
        }
    };

    const onUpdate = async (reservation: Reservation) => {
        if (!updateInput.trim()){
            return;
        }
        try{
            await putReservation(reservation, updateInput.trim());
            setUpdateInput('');
            closePopup();
            onRefresh();
        } catch (ex: any) {
            setError(ex.response?.data?.message || 'Failed to update reservation');
        }
    };

    return (
        <div>
            {/* Seat grid */}
            {Array.from({ length: 10 }, (_, ri) => (
            <div key={ri} style={{ display: 'flex', gap: 4 }}>
            <span style={{ width: 20 }}>{ri + 1}</span>
            {Array.from({ length: 10 }, (_, si) => {
                const row = ri + 1, seat = si + 1;
                return (
                <button
                    className = "seat-button"
                    key={si}
                    onClick={() => onSeatClick(row, seat)}
                    style={{ width: 32, height: 24, background: seatColor(row, seat) }}
                />
                );
            })}
            </div>
        ))}

        {/* Popup */}
        {popup && (
            <div onClick={closePopup} style={{ position: 'fixed', inset: 0, background: 'rgba(0,0,0,0.5)', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
            <div onClick={e => e.stopPropagation()} style={{ background: '#fff', color: '#000', padding: '1.5rem', borderRadius: 8, minWidth: 240 }}>
                {error && <p style={{ color: 'red' }}>{error}</p>}

                {popup.type === 'confirm' && <>
                <p>Reserve row {popup.row}, seat {popup.seat}?</p>
                <button onClick={() => onReservation(popup.row, popup.seat)}>Confirm</button>
                <button onClick={closePopup}>Cancel</button>
                </>}

                {popup.type === 'taken' && <>
                <p>Reserved by <strong>{popup.by}</strong></p>
                <button onClick={closePopup}>Close</button>
                </>}

                {popup.type === 'own' && <>
                <p>Your seat — row {popup.reservation.row}, seat {popup.reservation.seat}</p>
                <input value={updateInput} onChange={e => setUpdateInput(e.target.value)} placeholder="Transfer reservation to..?" />
                <button onClick={() => onUpdate(popup.reservation)}>Transfer</button>
                <button onClick={() => onCancel(popup.reservation)}>Cancel reservation</button>
                </>}
            </div>
            </div>
        )}
        </div>
    );
};

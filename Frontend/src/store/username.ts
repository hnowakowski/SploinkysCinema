import { useState, useEffect } from "react";

let username = '';
const listeners = new Set<(username: string) => void>();

export function getUsername() { return username; }

export function setUsername(newUsername: string) {
    username = newUsername;
    listeners.forEach(listener => listener(username));
}

export function useUsername() {
    const [, rerender] = useState(0);
    useEffect(() => {
        const fn = () => rerender(prev => prev + 1);
        listeners.add(fn);
        return () => { listeners.delete(fn); };
    }, [])
    return username;
}
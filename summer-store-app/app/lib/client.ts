import { Api } from "~/Api";

export const TOKEN_KEY = "token";

export const getToken = () => localStorage.getItem(TOKEN_KEY);
export const setToken = (token: string | null) =>
    token ? localStorage.setItem(TOKEN_KEY, token) : localStorage.removeItem(TOKEN_KEY);

export const client = new Api({
    baseURL: import.meta.env.VITE_API_URL ?? "http://localhost:5000",
    secure: true,
    securityWorker: () => {
        const token = getToken();
        return token ? { headers: { Authorization: `Bearer ${token}` } } : {};
    },
});

import { redirect } from "react-router";
import type { AuthResponse, LoginRequest, RegisterRequest } from "~/Api";
import { client, getToken, setToken } from "~/lib/client";

const USER_NAME_KEY = "userName";

function saveSession({ token, userName }: AuthResponse) {
    if (!token) throw new Error("Сервер не вернул токен");
    setToken(token);
    if (userName) localStorage.setItem(USER_NAME_KEY, userName);
}

export const isAuthenticated = () => Boolean(getToken());
export const getUserName = () => localStorage.getItem(USER_NAME_KEY);

export async function login(data: LoginRequest) {
    const res = await client.api.authLoginCreate(data);
    saveSession(res.data);
}

export async function register(data: RegisterRequest) {
    const res = await client.api.authRegisterCreate(data);
    saveSession(res.data);
}

export function logout() {
    setToken(null);
    localStorage.removeItem(USER_NAME_KEY);
}

export function requireAuth(request: Request) {
    if (isAuthenticated()) return;
    const url = new URL(request.url);
    const from = encodeURIComponent(url.pathname + url.search);
    throw redirect(`/login?from=${from}`);
}

export function safeRedirectPath(path: FormDataEntryValue | null) {
    const value = typeof path === "string" ? path : "/";
    return value.startsWith("/") && !value.startsWith("//") ? value : "/";
}

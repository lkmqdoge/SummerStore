import axios from "axios";

export function getErrorMessage(error: unknown): string {
    if (axios.isAxiosError(error)) {
        const status = error.response?.status;
        const data = error.response?.data;

        if (!error.response) return "Нет связи с сервером";

        if (status === 401) return "Неверный email или пароль";

        if (typeof data === "string" && data) return data;

        if (data && typeof data === "object") {
            if (data.errors) {
                return Object.values<string[]>(data.errors).flat().join(". ");
            }
            const message = data.error ?? data.detail ?? data.title ?? data.message;
            if (typeof message === "string") return message;
        }
    }
    if (error instanceof Error) return error.message;
    return "Что-то пошло не так";
}

import { Form, Link, redirect, useNavigation } from "react-router";
import Field from "~/components/field";
import { isAuthenticated, register } from "~/lib/auth";
import type { Route } from "../+types/root";

export async function clientLoader() {
    if (isAuthenticated()) throw redirect("/");
    return null;
}

export async function clientAction({
    request,
}: Route.ClientActionArgs) {
    console.log("[register] clientAction вызван");

    const form = await request.formData();

    const userName = String(form.get("userName") ?? "");
    const email = String(form.get("email") ?? "");
    const password = String(form.get("password") ?? "");
    const repeat = String(form.get("repeat") ?? "");

    console.log("[register] Данные формы получены");

    if (password !== repeat) {
        return { error: "Пароли не совпадают" };
    }

    try {
        await register({ userName, email, password });
        console.log("[register] Регистрация успешна");
        return redirect("/");
    } catch (err) {
        console.error("[register] Ошибка регистрации:", err);

        return {
            error: err instanceof Error
                ? err.message
                : "Ошибка регистрации",
        };
    }
}

export default function RegisterPage({ actionData }: Route.ComponentProps) {
    const navigation = useNavigation();
    const pending = navigation.state !== "idle";

    return (
        <div className="mx-auto max-w-sm">
            <Form
                method="post"
                className="space-y-4 rounded-lg border border-gray-200 bg-white p-6"
            >
                <h1 className="text-xl font-bold">Регистрация</h1>

                <Field
                    label="Имя пользователя"
                    name="userName"
                    autoComplete="username"
                    minLength={3}
                    maxLength={64}
                    required
                />
                <Field label="Email" name="email" type="email" autoComplete="email" required />
                <Field
                    label="Пароль (минимум 8 символов)"
                    name="password"
                    type="password"
                    autoComplete="new-password"
                    minLength={8}
                    required
                />
                <Field
                    label="Повторите пароль"
                    name="repeat"
                    type="password"
                    autoComplete="new-password"
                    required
                />

                <button
                    disabled={pending}
                    className="w-full rounded bg-gray-900 py-2 text-white hover:bg-gray-700 disabled:opacity-50"
                >
                    {pending ? "Подождите..." : "Зарегистрироваться"}
                </button>
            </Form>

            <p className="mt-4 text-center text-sm text-gray-600">
                Уже есть аккаунт?{" "}
                <Link to="/login" className="underline">
                    Войти
                </Link>
            </p>
        </div>
    );
}

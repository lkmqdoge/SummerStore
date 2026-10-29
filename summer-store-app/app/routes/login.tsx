import { Form, Link, redirect, useNavigation, useSearchParams } from "react-router";
import Field from "~/components/field";
import { isAuthenticated, login, safeRedirectPath } from "~/lib/auth";
import { getErrorMessage } from "~/lib/error";
import type { Route } from "../+types/root";

export async function clientLoader() {
    if (isAuthenticated()) throw redirect("/");
    return null;
}

export async function clientAction({ request }: Route.ClientActionArgs) {
    const form = await request.formData();

    try {
        await login({
            email: String(form.get("email") ?? ""),
            password: String(form.get("password") ?? ""),
        });
    } catch (err) {
        return { error: getErrorMessage(err) };
    }

    return redirect(safeRedirectPath(form.get("from")));
}

export default function LoginPage({ actionData }: Route.ComponentProps) {
    const [searchParams] = useSearchParams();
    const navigation = useNavigation();
    const pending = navigation.state !== "idle";

    return (
        <div className="mx-auto max-w-sm">
            <Form
                method="post"
                className="space-y-4 rounded-lg border border-gray-200 bg-white p-6"
            >
                <h1 className="text-xl font-bold">Вход</h1>

                {actionData?.error && (
                    <p className="rounded border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">
                        {actionData.error}
                    </p>
                )}

                <input type="hidden" name="from" value={searchParams.get("from") ?? "/"} />
                <Field label="Email" name="email" type="email" autoComplete="email" required />
                <Field
                    label="Пароль"
                    name="password"
                    type="password"
                    autoComplete="current-password"
                    required
                />

                <button
                    disabled={pending}
                    className="w-full rounded bg-gray-900 py-2 text-white hover:bg-gray-700 disabled:opacity-50"
                >
                    {pending ? "Подождите..." : "Войти"}
                </button>
            </Form>

            <p className="mt-4 text-center text-sm text-gray-600">
                Нет аккаунта?{" "}
                <Link to="/register" className="underline">
                    Зарегистрироваться
                </Link>
            </p>
        </div>
    );
}

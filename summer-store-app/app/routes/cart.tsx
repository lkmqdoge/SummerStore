import { Link, useNavigate, useRevalidator, type LoaderFunctionArgs } from "react-router";
import { useState } from "react";

import { client } from "~/lib/client";
import { getUserName } from "~/lib/auth";
import type { Route } from "./+types/cart";

export async function clientLoader({ request }: Route.ClientLoaderArgs) {
    try {
        const res = await client.api.cartList();
        return { cart: res.data };
    } catch (err) {
        const status = (err as any)?.response?.status;
        if (status === 401) {
            const url = new URL(request.url);
            throw new Response("", {
                status: 302,
                headers: {
                    Location: `/login?from=${encodeURIComponent(url.pathname)}`,
                },
            });
        }
        if (status === 404) return { cart: null };
        throw new Response("Не удалось загрузить корзину", { status: 500 });
    }
}

export default function Cart({ loaderData }: Route.ComponentProps) {
    const cart = loaderData.cart;
    const navigate = useNavigate();
    const revalidator = useRevalidator();
    const userName = getUserName();

    const [paying, setPaying] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handlePayment = async () => {
        if (!cart?.items?.length) {
            setError("Корзина пуста");
            return;
        }

        setPaying(true);
        setError(null);
        try {
            await client.api.cartPaymentCreate();
            navigate("/thankyou", { replace: true });
        } catch (err) {
            const status = (err as any)?.response?.status;
            setError(
                status === 401 ? "Сессия истекла, войдите заново"
                : status === 404 ? "Корзина не найдена"
                : "Не удалось провести оплату",
            );
            setPaying(false);
            revalidator.revalidate();
        }
    };

    if (!cart || !cart.items?.length) {
        return (
            <div className="bg-secondary text-primary p-4 max-w-3xl mx-auto">
                <h1 className="text-2xl font-bold mb-4">{userName ?? "Гость"}'s Cart</h1>
                <p className="opacity-80">Корзина пуста.</p>
                <Link to="/" className="underline mt-2 inline-block">К товарам</Link>
            </div>
        );
    }

    return (
        <div className="bg-secondary text-primary p-4 max-w-5xl mx-auto">
            <h1 className="text-2xl font-bold mb-4">{userName ?? "Гость"}'s Cart</h1>

            <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
                {cart.items.map((p, i) =>
                    <Link to={`/products/${p.productId}`} key={i}>
                        <div className="flex h-full flex-col border-6 border-gray-200 bg-primary p-4 hover:border-b-brand">
                            <div className="mb-3 flex h-40 items-center justify-center bg-g text-sm text-gray-400">
                                Нет фото
                            </div>

                            <h3 className="font-semibold">{p.title ?? "Без названия"}</h3>
                            <div className="flex text-secondary justify-between">
                                <p className="mt-auto pt-3 font-bold">{p.price} ₽</p>
                                <h1>{p.quantity}</h1>
                            </div>
                        </div>
                    </Link>
                )}
            </div>

            {error && <p role="alert" className="text-red-600 mt-4">{error}</p>}

            <div className="flex items-center justify-between gap-4 mt-6 border-t border-primary pt-4">
                <p className="text-xl font-bold">Итого: {cart.total} ₽</p>
                <button
                    type="button"
                    onClick={handlePayment}
                    disabled={paying}
                    className="border-primary border-2 px-4 py-2 disabled:opacity-50"
                >
                    {paying ? "Оплата…" : "Перейти к оплате"}
                </button>
            </div>
        </div>
    );
}

import { client } from "~/lib/client";
import { getUserName } from "~/lib/auth";
import { Link, redirect } from "react-router";
import type { Route } from "../+types/root";

export async function clientLoader({}: Route.ClientLoaderArgs) {
    const products = await client.api.cartList();
    return products.data;
}

export default function Home({loaderData}: Route.ComponentProps) {
    const { items, total } = loaderData;
    const userName = getUserName();

    const handlePayment = async () => {
        try {
            await client.api.cartPaymentCreate();
            
            redirect("/thankyou");
        } catch {
            redirect("/");
        }
    }

    return (
        <div>
            <h1>{userName}'s Cart</h1>
            <div className="bg-secondary grid grid-cols-1 gap-4 sm:grid-cols-2 p-4 lg:grid-cols-3 xl:grid-cols-4">
                {items.map((p, i) =>
                    <Link to={`/products/${p.productId}`} key={i}>
                        <div className="flex h-full flex-col border-6 border-gray-200 bg-primary p-4 hover:border-b-brand">
                            <div className="mb-3 flex h-40 items-center justify-center bg-g text-sm text-gray-400">
                                Нет фото
                            </div>

                            <h3 className="font-semibold">{p.title ?? "Без названия"}</h3>
                            <div className="flex justify-between">
                                <p className="mt-auto pt-3 font-bold">{p.price} ₽</p>
                                <h1>{p.quantity}</h1>
                            </div>
                        </div>
                    </Link>
                )}
            </div>

            <div className="flex gap-4">
                <h1>Total: {total}</h1>
                <button onClick={handlePayment}>Continue to payment</button>
            </div>
        </div>
    )
}

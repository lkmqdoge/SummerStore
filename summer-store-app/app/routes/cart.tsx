import ProductPreview from "~/components/product-preview";
import type { Route } from "../+types/root";
import { client } from "~/lib/client";

export async function clientLoader({}: Route.ClientLoaderArgs) {
    const products = await client.api.cartList();
    return { products: products.data ?? [] };
}

export default function Home({loaderData}: Route.ComponentProps) {
    const { products } = loaderData;

    return (
        <div className="bg-secondary grid grid-cols-1 gap-4 sm:grid-cols-2 p-4 lg:grid-cols-3 xl:grid-cols-4">
            {products.map((p, i) => <ProductPreview product={p} key={i} />)}
        </div>
    )
}

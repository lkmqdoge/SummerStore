import type { ProductRequest } from "~/Api"
import ProductPreview from "~/components/product-preview";

export default function Home() {
    let products: ProductRequest[] = [
        {
            title: "prod1",
            price: 999
        },
    ];

    for (let _ = 0; _ <= 3; _++) {
        products = products.flatMap(i => [i, Object.assign({}, i)])
    }

    return (
        <div className="bg-secondary grid grid-cols-1 gap-4 sm:grid-cols-2 p-4 lg:grid-cols-3 xl:grid-cols-4">
            {products.map((p, i) => <ProductPreview product={p} key={i} />)}
        </div>
    )
}

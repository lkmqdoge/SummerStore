import { Link } from "react-router"
import type { ProductListItem, ProductRequest } from "~/Api"

interface Props {
    product: ProductListItem
}

export default function ProductPreview(props: Props) {
    return (
        <Link to={`/products/${props.product.id}`}>
            <div className="flex h-full flex-col border-6 border-gray-200 bg-primary p-4 hover:border-b-brand">
                <div className="mb-3 flex h-40 items-center justify-center bg-g text-sm text-gray-400">
                    Нет фото
                </div>

                <h3 className="font-semibold">{props.product.title ?? "Без названия"}</h3>

                <p className="mt-auto pt-3 font-bold">{props.product.price} ₽</p>
            </div>
        </Link>
    )
}


import { Link } from "react-router";
import type { ProductListItem } from "~/Api";
import { productImageUrl } from "~/lib/images";

interface Props {
    product: ProductListItem;
}

export default function ProductPreview({ product }: Props) {
    const hasImage = product.id && product.previewImageId;

    return (
        <Link to={`/products/${product.id}`}>
            <div className="flex h-full flex-col border-6 border-gray-200 bg-primary p-4 hover:border-b-brand">
                <div className="mb-3 flex h-40 items-center justify-center overflow-hidden bg-g text-sm text-gray-400">
                    {hasImage ? (
                        <img
                            src={productImageUrl(product.id!, product.previewImageId!)}
                            alt={product.title ?? ""}
                            loading="lazy"
                            className="h-full w-full object-cover"
                        />
                    ) : (
                        "Нет фото"
                    )}
                </div>

                <h3 className="font-semibold">{product.title ?? "Без названия"}</h3>

                <p className="mt-auto pt-3 font-bold">{product.price} ₽</p>
            </div>
        </Link>
    );
}

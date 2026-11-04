import {
    Link,
    isRouteErrorResponse,
    useNavigate,
    useRouteError,
} from "react-router";
import { useState } from "react";

import { client } from "~/lib/client";
import { getUserId, isAuthenticated } from "~/lib/auth";
import type { Route } from "../+types/root";

export async function clientLoader({ params }: Route.ClientLoaderArgs) {
    const id = params.id;
    if (!id) throw new Response("Товар не найден", { status: 404 });

    const res = await client.api
        .productsDetail(id)
        .catch((err) => {
            const status = (err as any)?.response?.status;
            throw new Response(
                status === 404 ? "Товар не найден" : "Не удалось загрузить товар",
                { status: status ?? 500 },
            );
        });

    if (!res.data?.id) throw new Response("Товар не найден", { status: 404 });
    return { product: res.data };
}

const priceFormatter = new Intl.NumberFormat("ru-RU", {
    style: "currency",
    currency: "RUB",
    maximumFractionDigits: 2,
});

export default function Product({loaderData}: Route.ComponentProps) {
    const product = loaderData.product;

    const navigate = useNavigate();
    const [downloading, setDownloading] = useState(false);
    const [deleting, setDeleting] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const isAuthor = product.creatorId === getUserId();

    const [adding, setAdding] = useState(false);
    const [added, setAdded] = useState(false);

    const handleAddToCart = async () => {
        if (!isAuthenticated()) {
            navigate(`/login?from=${encodeURIComponent(`/products/${product.id}`)}`);
            return;
        }
        setAdding(true);
        setError(null);
        try {
            await client.api.cartItemsCreate({
                productId: product.id!,
                quantity: 1,
            });
            setAdded(true);
            // через пару секунд вернуть кнопку в исходное состояние
            window.setTimeout(() => setAdded(false), 2000);
        } catch (err) {
            const status = (err as any)?.response?.status;
            setError(
                status === 401 ? "Требуется войти"
                    : status === 404 ? "Товар или корзина не найдены"
                        : "Не удалось добавить в корзину",
            );
        } finally {
            setAdding(false);
        }
    };

    const handleDownload = async () => {
        setDownloading(true);
        setError(null);
        try {
            const res = await client.api.productsAttachmentsList(
                { id: product.id! },
                { format: "blob" },
            );

            const url = URL.createObjectURL(res.data as unknown as Blob);
            const a = document.createElement("a");
            a.href = url;
            a.download = `${product.title || product.id}.zip`;
            document.body.appendChild(a);
            a.click();
            a.remove();
            URL.revokeObjectURL(url);
        } catch (err) {
            const status = (err as any)?.response?.status;
            setError(
                status === 404
                    ? "У товара нет вложений"
                    : "Не удалось скачать вложения",
            );
        } finally {
            setDownloading(false);
        }
    };

    const handleDelete = async () => {
        if (!window.confirm(`Удалить «${product.title}»?`)) return;
        setDeleting(true);
        setError(null);
        try {
            await client.api.productsDelete(product.id!);
            navigate("/", { replace: true });
        } catch (err) {
            const status = (err as any)?.response?.status;
            setError(
                status === 401
                    ? "Требуется войти"
                    : status === 403
                      ? "Нет прав на удаление"
                      : "Не удалось удалить товар",
            );
            setDeleting(false);
        }
    };

    return (
        <div className="bg-secondary text-primary p-4 max-w-3xl mx-auto">
            <nav className="mb-4 text-sm opacity-70">
                <Link to="/" className="hover:underline">← К товарам</Link>
            </nav>

            <article className="flex flex-col gap-4">
                <header className="flex items-start justify-between gap-4">
                    <div>
                        <h1 className="text-2xl font-bold">{product.title}</h1>
                        {product.summary && (
                            <p className="opacity-80 mt-1">{product.summary}</p>
                        )}
                    </div>
                    <div className="text-xl font-semibold whitespace-nowrap">
                        {priceFormatter.format(product.price ?? 0)}
                    </div>
                </header>

                {product.description && (
                    <section>
                        <h2 className="font-semibold mb-1">Description</h2>
                        <p className="whitespace-pre-line leading-relaxed">
                            {product.description}
                        </p>
                    </section>
                )}

                {(product.imageIds?.length ?? 0) > 0 && (
                    <section>
                        <h2 className="font-semibold mb-1">Rewards</h2>
                        <p className="text-sm opacity-70">
                            {product.imageIds!.length} шт. — скачиваются в общем zip
                        </p>
                    </section>
                )}

                {error && <p role="alert" className="text-red-600">{error}</p>}

                <div className="flex flex-wrap gap-2">
                    <button
                        type="button"
                        onClick={handleDownload}
                        disabled={downloading}
                        className="border-primary border-2 px-3 py-1 disabled:opacity-50"
                    >
                        {downloading ? "Downloading…" : "Download rewards"}
                    </button>

                    {isAuthor && (
                        <>
                            <Link
                                to={`/products/${product.id}/edit`}
                                className="border-primary border-2 px-3 py-1"
                            >
                                Редактировать
                            </Link>
                            <button
                                type="button"
                                onClick={handleDelete}
                                disabled={deleting}
                                className="border-primary border-2 px-3 py-1 text-red-600 disabled:opacity-50"
                            >
                                {deleting ? "Deleting…" : "Delete"}
                            </button>
                        </>
                    )}
                </div>
                <button
                    type="button"
                    onClick={handleAddToCart}
                    disabled={adding}
                    className="border-primary border-2 px-3 py-1 disabled:opacity-50"
                >
                    {adding ? "Добавление…" : added ? "Добавлено ✓" : "В корзину"}
                </button>
            </article>
        </div>
    );
}

export function ErrorBoundary() {
    const err = useRouteError();
    const message = isRouteErrorResponse(err)
        ? err.status === 404
            ? "Товар не найден"
            : err.statusText || "Ошибка"
        : "Что-то пошло не так";

    return (
        <div className="bg-secondary text-primary p-4 max-w-3xl mx-auto">
            <h1 className="text-xl font-bold mb-2">{message}</h1>
            <Link to="/products" className="underline">К списку товаров</Link>
        </div>
    );
}

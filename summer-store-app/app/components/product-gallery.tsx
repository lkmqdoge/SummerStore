import { useRef, useState } from "react";
import { productImageUrl } from "~/lib/images";

interface Props {
    productId: string;
    imageIds: string[];
}

export default function ProductGallery({ productId, imageIds }: Props) {
    const trackRef = useRef<HTMLDivElement>(null);
    const [index, setIndex] = useState(0);

    if (imageIds.length === 0) return null;

    const goTo = (i: number) => {
        const el = trackRef.current;
        if (!el) return;
        const next = Math.max(0, Math.min(imageIds.length - 1, i));
        el.scrollTo({ left: next * el.clientWidth, behavior: "smooth" });
    };

    const handleScroll = () => {
        const el = trackRef.current;
        if (!el) return;
        setIndex(Math.round(el.scrollLeft / el.clientWidth));
    };

    const handleKeyDown = (e: React.KeyboardEvent) => {
        if (e.key === "ArrowLeft") goTo(index - 1);
        if (e.key === "ArrowRight") goTo(index + 1);
    };

    const multiple = imageIds.length > 1;

    return (
        <section aria-label="Изображения товара" className="flex flex-col gap-2">
            <div className="relative" tabIndex={0} onKeyDown={handleKeyDown}>
                <div
                    ref={trackRef}
                    onScroll={handleScroll}
                    className="flex snap-x snap-mandatory overflow-x-auto scroll-smooth [scrollbar-width:none] [&::-webkit-scrollbar]:hidden"
                >
                    {imageIds.map((imageId, i) => (
                        <div
                            key={imageId}
                            className="flex aspect-video min-w-full snap-center items-center justify-center bg-black/5"
                        >
                            <img
                                src={productImageUrl(productId, imageId)}
                                alt={`Изображение ${i + 1} из ${imageIds.length}`}
                                loading={i === 0 ? "eager" : "lazy"}
                                className="h-full w-full object-contain"
                            />
                        </div>
                    ))}
                </div>

                {multiple && (
                    <>
                        <button
                            type="button"
                            aria-label="Предыдущее изображение"
                            onClick={() => goTo(index - 1)}
                            disabled={index === 0}
                            className="absolute left-2 top-1/2 -translate-y-1/2 bg-black/50 px-3 py-2 text-white disabled:opacity-30"
                        >
                            ‹
                        </button>
                        <button
                            type="button"
                            aria-label="Следующее изображение"
                            onClick={() => goTo(index + 1)}
                            disabled={index === imageIds.length - 1}
                            className="absolute right-2 top-1/2 -translate-y-1/2 bg-black/50 px-3 py-2 text-white disabled:opacity-30"
                        >
                            ›
                        </button>
                        <span className="absolute bottom-2 right-2 bg-black/50 px-2 py-0.5 text-xs text-white">
                            {index + 1} / {imageIds.length}
                        </span>
                    </>
                )}
            </div>

            {multiple && (
                <div className="flex gap-2 overflow-x-auto">
                    {imageIds.map((imageId, i) => (
                        <button
                            key={imageId}
                            type="button"
                            onClick={() => goTo(i)}
                            aria-label={`Показать изображение ${i + 1}`}
                            className={`h-14 w-20 shrink-0 border-2 ${
                                i === index ? "border-primary" : "border-transparent opacity-60"
                            }`}
                        >
                            <img
                                src={productImageUrl(productId, imageId)}
                                alt=""
                                loading="lazy"
                                className="h-full w-full object-cover"
                            />
                        </button>
                    ))}
                </div>
            )}
        </section>
    );
}

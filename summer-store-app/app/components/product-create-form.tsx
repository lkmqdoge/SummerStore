import { useRef, useState, type FormEvent } from "react";
import { ContentType, DocumentType } from "~/Api";
import { client } from "~/lib/client";


function getErrorMessage(err: unknown): string {
  const e = err as any;
  const data = e?.response?.data;
  if (data?.errors && typeof data.errors === "object") {
    return Object.values(data.errors as Record<string, string[]>)
      .flat()
      .join("; ");
  }
  return data?.detail || data?.title || e?.message || "Не удалось создать товар";
}

export default function ProductCreateForm() {
  const formRef = useRef<HTMLFormElement>(null);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (submitting) return;

    const form = event.currentTarget;
    const data = new FormData(form);

    const title = String(data.get("title") ?? "").trim();
    const summary = String(data.get("summary") ?? "").trim();
    const description = String(data.get("description") ?? "").trim();
    const price = Number(data.get("price"));

    const previewFiles = data.getAll("preview-files")
      .filter((f): f is File => f instanceof File && f.size > 0);

    const files = data
      .getAll("files")
      .filter((f): f is File => f instanceof File && f.size > 0);

    if (!title) {
      setError("Укажите название товара");
      return;
    }
    if (!Number.isFinite(price) || price < 0) {
      setError("Укажите корректную цену");
      return;
    }

    setError(null);
    setSuccess(false);
    setSubmitting(true);

    try {
      const response = await client.api.productsCreate({
        title,
        summary: summary || undefined,
        description: description || undefined,
        price,
      });

      const created = response.data as unknown as { id?: string } | null;
      const productId = created?.id;

      if (files.length > 0) {
        if (!productId) {
          throw new Error(
            "Товар создан, но сервер не вернул id — файлы загрузить не удалось",
          );
        }

        for (const preview of previewFiles) {
          await client.api.productsAttachmentsCreate(
            productId,
            { productId, file: preview, type: DocumentType.Value1 },
            { type: ContentType.FormData },
          );
        }

        for (const file of files) {
          await client.api.productsAttachmentsCreate(
            productId,
            { productId, file, type: DocumentType.Value1 },
            { type: ContentType.FormData },
          );
        }
      }

      form.reset();
      setSuccess(true);
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="bg-secondary text-primary p-4">
      <form
        ref={formRef}
        className="flex flex-col gap-2"
        onSubmit={handleSubmit}
        noValidate
      >
        <h1>Новый товар</h1>

        <label className="flex flex-col">
          Название
          <input
            id="title"
            name="title"
            className="border-primary border-2"
            required
          />
        </label>

        <label className="flex flex-col">
          Краткое описание
          <input id="summary" name="summary" className="border-primary border-2" />
        </label>

        <label className="flex flex-col">
          Описание
          <textarea
            id="description"
            name="description"
            className="border-primary border-2"
            rows={4}
          />
        </label>

        <label className="flex flex-col">
          Цена
          <input
            id="price"
            name="price"
            type="number"
            min="0"
            step="0.01"
            className="border-primary border-2"
            required
          />
        </label>

        <label className="flex flex-col">
            Превью
          <input
            id="preview-files"
            name="preview-files"
            accept="image/*"
            type="file"
            className="border-primary border-2"
            multiple
          />
        </label>

        <label className="flex flex-col">
          Файлы товара
          <input
            id="files"
            name="files"
            type="file"
            className="border-primary border-2"
            multiple
          />
        </label>

        {error && (
          <p role="alert" className="text-red-600">
            {error}
          </p>
        )}
        {success && (
          <p role="status" className="text-green-600">
            Товар создан
          </p>
        )}

        <button id="submit" type="submit" disabled={submitting}>
          {submitting ? "Отправка…" : "Создать"}
        </button>
      </form>
    </div>
  );
}

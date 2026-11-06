import { client } from "~/lib/client";

export const productImageUrl = (productId: string, imageId: string) =>
    `${client.instance.defaults.baseURL}/api/products/${productId}/images/${imageId}`;

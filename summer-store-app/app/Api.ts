/* eslint-disable */
/* tslint:disable */
// @ts-nocheck
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export interface AddCartItemRequest {
  /** @format uuid */
  productId?: string;
  /** @format int32 */
  quantity?: number;
}

export interface AuthResponse {
  token?: string | null;
  /** @format uuid */
  userId?: string;
  userName?: string | null;
}

export interface HttpValidationProblemDetails {
  type?: string | null;
  title?: string | null;
  /** @format int32 */
  status?: number | null;
  detail?: string | null;
  instance?: string | null;
  errors?: Record<string, string[]> | null;
  [key: string]: any;
}

export interface LoginRequest {
  email?: string | null;
  password?: string | null;
}

export interface ProductRequest {
  title?: string | null;
  summary?: string | null;
  description?: string | null;
  /** @format double */
  price?: number;
}

export interface RegisterRequest {
  userName?: string | null;
  email?: string | null;
  password?: string | null;
}

export interface UpdateCartItemRequest {
  /** @format int32 */
  quantity?: number;
}

import type {
  AxiosInstance,
  AxiosRequestConfig,
  AxiosResponse,
  HeadersDefaults,
  ResponseType,
} from "axios";
import axios from "axios";

export type QueryParamsType = Record<string | number, any>;

export interface FullRequestParams
  extends Omit<AxiosRequestConfig, "data" | "params" | "url" | "responseType"> {
  /** set parameter to `true` for call `securityWorker` for this request */
  secure?: boolean;
  /** request path */
  path: string;
  /** content type of request body */
  type?: ContentType;
  /** query params */
  query?: QueryParamsType;
  /** format of response (i.e. response.json() -> format: "json") */
  format?: ResponseType;
  /** request body */
  body?: unknown;
}

export type RequestParams = Omit<
  FullRequestParams,
  "body" | "method" | "query" | "path"
>;

export interface ApiConfig<SecurityDataType = unknown>
  extends Omit<AxiosRequestConfig, "data" | "cancelToken"> {
  securityWorker?: (
    securityData: SecurityDataType | null,
  ) => Promise<AxiosRequestConfig | void> | AxiosRequestConfig | void;
  secure?: boolean;
  format?: ResponseType;
}

export enum ContentType {
  Json = "application/json",
  JsonApi = "application/vnd.api+json",
  FormData = "multipart/form-data",
  UrlEncoded = "application/x-www-form-urlencoded",
  Text = "text/plain",
}

export class HttpClient<SecurityDataType = unknown> {
  public instance: AxiosInstance;
  private securityData: SecurityDataType | null = null;
  private securityWorker?: ApiConfig<SecurityDataType>["securityWorker"];
  private secure?: boolean;
  private format?: ResponseType;

  constructor({
    securityWorker,
    secure,
    format,
    ...axiosConfig
  }: ApiConfig<SecurityDataType> = {}) {
    this.instance = axios.create({
      ...axiosConfig,
      baseURL: axiosConfig.baseURL || "",
    });
    this.secure = secure;
    this.format = format;
    this.securityWorker = securityWorker;
  }

  public setSecurityData = (data: SecurityDataType | null) => {
    this.securityData = data;
  };

  protected mergeRequestParams(
    params1: AxiosRequestConfig,
    params2?: AxiosRequestConfig,
  ): AxiosRequestConfig {
    const method = params1.method || (params2 && params2.method);

    return {
      ...this.instance.defaults,
      ...params1,
      ...(params2 || {}),
      headers: {
        ...((method &&
          this.instance.defaults.headers[
            method.toLowerCase() as keyof HeadersDefaults
          ]) ||
          {}),
        ...(params1.headers || {}),
        ...((params2 && params2.headers) || {}),
      },
    };
  }

  protected stringifyFormItem(formItem: unknown) {
    if (typeof formItem === "object" && formItem !== null) {
      return JSON.stringify(formItem);
    } else {
      return `${formItem}`;
    }
  }

  protected createFormData(input: Record<string, unknown>): FormData {
    if (input instanceof FormData) {
      return input;
    }
    return Object.keys(input || {}).reduce((formData, key) => {
      const property = input[key];
      const propertyContent: any[] =
        property instanceof Array ? property : [property];

      for (const formItem of propertyContent) {
        const isFileType = formItem instanceof Blob || formItem instanceof File;
        formData.append(
          key,
          isFileType ? formItem : this.stringifyFormItem(formItem),
        );
      }

      return formData;
    }, new FormData());
  }

  public request = async <T = any, _E = any>({
    secure,
    path,
    type,
    query,
    format,
    body,
    ...params
  }: FullRequestParams): Promise<AxiosResponse<T>> => {
    const secureParams =
      ((typeof secure === "boolean" ? secure : this.secure) &&
        this.securityWorker &&
        (await this.securityWorker(this.securityData))) ||
      {};
    const requestParams = this.mergeRequestParams(params, secureParams);
    const responseFormat = format || this.format || undefined;

    if (
      type === ContentType.FormData &&
      body &&
      body !== null &&
      typeof body === "object"
    ) {
      body = this.createFormData(body as Record<string, unknown>);
    }

    if (
      type === ContentType.Text &&
      body &&
      body !== null &&
      typeof body !== "string"
    ) {
      body = JSON.stringify(body);
    }

    return this.instance.request({
      ...requestParams,
      headers: {
        ...(requestParams.headers || {}),
        ...(type ? { "Content-Type": type } : {}),
      },
      params: query,
      responseType: responseFormat,
      data: body,
      url: path,
    });
  };
}

/**
 * @title Product service
 * @version v1
 * @contact lkmqdoge <lkmqdoge@proton.me>
 *
 * Project for study
 */
export class Api<
  SecurityDataType extends unknown,
> extends HttpClient<SecurityDataType> {
  api = {
    /**
     * No description
     *
     * @tags CartEndpoints
     * @name CartList
     * @request GET:/api/cart
     */
    cartList: (params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/cart`,
        method: "GET",
        ...params,
      }),

    /**
     * No description
     *
     * @tags CartEndpoints
     * @name CartDelete
     * @request DELETE:/api/cart
     */
    cartDelete: (params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/cart`,
        method: "DELETE",
        ...params,
      }),

    /**
     * No description
     *
     * @tags CartEndpoints
     * @name CartItemsCreate
     * @request POST:/api/cart/items
     */
    cartItemsCreate: (data: AddCartItemRequest, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/cart/items`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags CartEndpoints
     * @name CartItemsUpdate
     * @request PUT:/api/cart/items/{productId}
     */
    cartItemsUpdate: (
      productId: string,
      data: UpdateCartItemRequest,
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/api/cart/items/${productId}`,
        method: "PUT",
        body: data,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags CartEndpoints
     * @name CartItemsDelete
     * @request DELETE:/api/cart/items/{productId}
     */
    cartItemsDelete: (productId: string, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/cart/items/${productId}`,
        method: "DELETE",
        ...params,
      }),

    /**
     * No description
     *
     * @tags ProductEndpoints
     * @name ProductsList
     * @request GET:/api/products
     */
    productsList: (params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/products`,
        method: "GET",
        ...params,
      }),

    /**
     * No description
     *
     * @tags ProductEndpoints
     * @name ProductsCreate
     * @request POST:/api/products
     */
    productsCreate: (data: ProductRequest, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/products`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags ProductEndpoints
     * @name ProductsDetail
     * @request GET:/api/products/{id}
     */
    productsDetail: (id: string, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/products/${id}`,
        method: "GET",
        ...params,
      }),

    /**
     * No description
     *
     * @tags ProductEndpoints
     * @name ProductsUpdate
     * @request PUT:/api/products/{id}
     */
    productsUpdate: (
      id: string,
      data: ProductRequest,
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/api/products/${id}`,
        method: "PUT",
        body: data,
        type: ContentType.Json,
        ...params,
      }),

    /**
     * No description
     *
     * @tags ProductEndpoints
     * @name ProductsDelete
     * @request DELETE:/api/products/{id}
     */
    productsDelete: (id: string, params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/products/${id}`,
        method: "DELETE",
        ...params,
      }),

    /**
     * No description
     *
     * @tags ProductEndpoints
     * @name ProductsImagesDetail
     * @request GET:/api/products/{id}/images/{imageId}
     */
    productsImagesDetail: (
      id: string,
      imageId: string,
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/api/products/${id}/images/${imageId}`,
        method: "GET",
        ...params,
      }),

    /**
     * No description
     *
     * @tags ProductEndpoints
     * @name ProductsImagesDelete
     * @request DELETE:/api/products/{id}/images/{imageId}
     */
    productsImagesDelete: (
      id: string,
      imageId: string,
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/api/products/${id}/images/${imageId}`,
        method: "DELETE",
        ...params,
      }),

    /**
     * No description
     *
     * @tags ProductEndpoints
     * @name ProductsImagesCreate
     * @request POST:/api/products/{id}/images
     */
    productsImagesCreate: (
      id: string,
      data: {
        /** @format binary */
        file: File;
      },
      params: RequestParams = {},
    ) =>
      this.request<void, any>({
        path: `/api/products/${id}/images`,
        method: "POST",
        body: data,
        type: ContentType.FormData,
        ...params,
      }),

    /**
     * No description
     *
     * @tags UserEndpoints
     * @name AuthRegisterCreate
     * @request POST:/api/auth/register
     */
    authRegisterCreate: (data: RegisterRequest, params: RequestParams = {}) =>
      this.request<AuthResponse, HttpValidationProblemDetails | void>({
        path: `/api/auth/register`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags UserEndpoints
     * @name AuthLoginCreate
     * @request POST:/api/auth/login
     */
    authLoginCreate: (data: LoginRequest, params: RequestParams = {}) =>
      this.request<AuthResponse, void>({
        path: `/api/auth/login`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),
  };
}

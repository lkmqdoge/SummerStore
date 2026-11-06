import { type RouteConfig, index, layout, route } from "@react-router/dev/routes";

export default [
    layout("layouts/main-layout.tsx", [
        index("routes/home.tsx"),
        route("register", "routes/register.tsx"),
        route("login", "routes/login.tsx"),
        route("logout", "routes/logout.tsx"),
        route("products/create", "routes/product-create.tsx"),
        route("products/:id", "routes/product.tsx"),
        route("cart", "routes/cart.tsx"),
        route("library", "routes/library.tsx"),
        route("thankyou", "routes/thankyou.tsx")
    ])
] satisfies RouteConfig;

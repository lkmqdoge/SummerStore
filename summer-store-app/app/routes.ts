import { type RouteConfig, index, layout, route } from "@react-router/dev/routes";

export default [
    layout("layouts/main-layout.tsx", [
        index("routes/home.tsx"),
        route("register", "routes/register.tsx"),
        route("login", "routes/login.tsx"),
        route("logout", "routes/logout.tsx")
    ])
] satisfies RouteConfig;

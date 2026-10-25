import { Link, Outlet } from "react-router";

export default function MainLayout() {
    return (
        <>
            <nav>
                <Link to="/">Home</Link> |
                <Link to="/about">About</Link> |
                <Link to="/cart">cart</Link>
            </nav>
            <main>
                <Outlet />
            </main>
        </>
    )
}

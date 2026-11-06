import { Link } from "react-router";
import { isAuthenticated } from "~/lib/auth";

export default function Header() {
 return (
    <div className="flex h-12 px-4 justify-between bg-secondary items-center">
        <div>
            <Link to="/">
                <img src="/summer-shop-logo.svg"/>
            </Link>
        </div>

        <div className="text-primary">
            {isAuthenticated() ? (
                <>
                    <Link to={"/products/create"}>Create</Link>|
                    <Link to={"/cart"}>Cart</Link>|
                    <Link to={"/library"}>Library</Link>|
                    <Link to={"/logout"}>Logout</Link>
                </>
            ) : (
                <>
                    <Link to={"/register"}>Register</Link> |
                    <Link to={"/login"}>Login</Link>
                </>
            )}
        </div>
    </div>
 )
}

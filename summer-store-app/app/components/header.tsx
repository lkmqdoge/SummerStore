import { Link } from "react-router";
import { isAuthenticated } from "~/lib/auth";

export default function Header() {
 return (
    <div className="flex h-12 px-4 justify-between bg-secondary items-center">
        <div>
            <img src="/summer-shop-logo.svg"/>
        </div>

        <div>
            {isAuthenticated() ? (
                <Link to={"/logout"}>Logout</Link>
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

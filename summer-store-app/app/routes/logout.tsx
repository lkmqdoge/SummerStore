import { redirect } from "react-router";
import { logout } from "~/lib/auth";

export async function clientLoader() {
    logout();
    return redirect("/");
}

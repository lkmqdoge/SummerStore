import { Link, Outlet } from "react-router";
import Header from "~/components/header";

export default function MainLayout() {
    return (
        <>
            <div className="h-full  min-h-screen flex-col flex bg-primary items-center"> 
                {/* Main column */}
                <div className="w-5xl gap-y-4 flex flex-col">
                    <Header />
                    <div>
                        <Outlet />
                    </div>
                </div>
            </div>
        </>
    )
}

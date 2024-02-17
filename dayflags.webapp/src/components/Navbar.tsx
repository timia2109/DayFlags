import { FC } from "react";
import { NavLink } from "react-router-dom";

export const Navbar: FC = () => (
    <div className="navbar bg-base-100">
        <NavLink to={"/welcome"} className="btn btn-ghost text-xl">
            DayFlags
        </NavLink>
    </div>
);

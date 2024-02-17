import { FC, PropsWithChildren } from "react";
import { NavLink } from "react-router-dom";

export const Navbar: FC<PropsWithChildren<unknown>> = ({ children }) => (
    <div className="navbar bg-base-100">
        <NavLink to={"/welcome"} className="btn btn-ghost text-xl">
            DayFlags
        </NavLink>
        <div className="navbar-end">{children}</div>
    </div>
);

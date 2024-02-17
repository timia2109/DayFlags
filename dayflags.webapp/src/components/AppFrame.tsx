import { FC, PropsWithChildren } from "react";
import { Navbar } from "./Navbar";
import { Sidemenu } from "./Sidemenu";

export const AppFrame: FC<PropsWithChildren<unknown>> = ({ children }) => (
    <>
        <Navbar>
            <label
                htmlFor="my-drawer-2"
                className="btn btn-primary drawer-button lg:hidden"
            >
                Open drawer
            </label>
        </Navbar>

        <div className="drawer lg:drawer-open">
            <input id="my-drawer-2" type="checkbox" className="drawer-toggle" />
            <div className="drawer-content flex flex-col items-center justify-center">
                {children}
            </div>
            <div className="drawer-side">
                <label
                    htmlFor="my-drawer-2"
                    aria-label="close sidebar"
                    className="drawer-overlay"
                ></label>
                <Sidemenu />
            </div>
        </div>
    </>
);

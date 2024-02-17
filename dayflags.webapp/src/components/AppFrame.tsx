import { FC, PropsWithChildren } from "react";
import { Navbar } from "./Navbar";

export const AppFrame: FC<PropsWithChildren<unknown>> = ({ children }) => (
    <>
        <Navbar />
        <div className="container">{children}</div>
    </>
);

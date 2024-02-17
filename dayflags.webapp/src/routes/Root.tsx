import { Outlet } from "react-router-dom";
import { AppFrame } from "../components/AppFrame";

export const Root: React.FC = () => (
    <AppFrame>
        <button className="btn btn-primary">Button</button>

        <Outlet />
    </AppFrame>
);

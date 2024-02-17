import { Outlet } from "react-router-dom";
import { AppFrame } from "../components/AppFrame";

export const Root: React.FC = () => (
    <AppFrame>
        <Outlet />
    </AppFrame>
);

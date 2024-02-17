import { createBrowserRouter } from "react-router-dom";
import { Root } from "./routes/Root";

export const router = createBrowserRouter([
    {
        path: "",
        element: <Root />,
        children: [
            {
                path: "/welcome",
                element: <h1>Welcome to DayFlags</h1>,
            },
        ],
    },
]);

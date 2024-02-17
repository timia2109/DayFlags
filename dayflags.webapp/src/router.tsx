import { createBrowserRouter } from "react-router-dom";
import { Root } from "./routes/Root";
import { Welcome } from "./routes/Welcome";

export const router = createBrowserRouter([
    {
        path: "",
        element: <Root />,
        children: [
            {
                index: true,
                element: <Welcome />,
            },
        ],
    },
]);

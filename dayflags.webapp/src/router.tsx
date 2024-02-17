import { createBrowserRouter } from "react-router-dom";
import { Root } from "./routes/Root";
import { rootLoader } from "./routes/Root.actions";
import { Welcome } from "./routes/Welcome";

export const router = createBrowserRouter([
    {
        path: "",
        element: <Root />,
        loader: rootLoader,
        children: [
            {
                index: true,
                element: <Welcome />,
            },
        ],
    },
]);

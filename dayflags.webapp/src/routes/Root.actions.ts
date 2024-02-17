import createClient from "openapi-fetch";
import { LoaderFunction } from "react-router-dom";
import { paths } from "../api/schema";

const { GET } = createClient<paths>();

export const rootLoader: LoaderFunction = async () => {
    const { data } = await GET("/api/Realm");
    return data;
};

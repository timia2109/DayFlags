import { FC, PropsWithChildren } from "react";
import {
    NavLink,
    useLoaderData,
    useNavigate,
    useParams,
} from "react-router-dom";
import { components } from "../api/schema";

const RealmSelector: FC = () => {
    const realms =
        useLoaderData() as components["schemas"]["RealmResponsePagingResponse"];
    const routeOptions = useParams();
    const nav = useNavigate();

    const selectedRealm = routeOptions["realmId"];

    return (
        <label className="form-control w-full max-w-xs">
            <div className="label">
                <span className="label-text">Select Realm</span>
            </div>

            <select
                value={selectedRealm ?? 1}
                className="select select-bordered"
                onChange={(e) => nav(`/realms/${e.currentTarget.value}`)}
            >
                <option disabled value={1}>
                    Select Realm
                </option>
                {realms.items!.map((r) => (
                    <option key={r.realmId} value={r.realmId}>
                        {r.label ?? r.realmId}
                    </option>
                ))}
            </select>
        </label>
    );
};

const RealmLink: FC<
    PropsWithChildren<{
        target: string;
    }>
> = ({ target, children }) => {
    const { realmId } = useParams();
    const disabled = realmId == undefined;

    return (
        <li>
            {!disabled && (
                <NavLink to={`/realms/${realmId}/${target}`}>
                    {children}
                </NavLink>
            )}
            {disabled && <a className="text-slate-500">{children}</a>}
        </li>
    );
};

export const Sidemenu: FC = () => {
    return (
        <div className="p-4 min-h-full bg-base-400 text-base-content">
            <RealmSelector />
            <ul className="menu w-56 rounded-box">
                <li>
                    <NavLink to="/realms">Manage Realms</NavLink>
                </li>
                <RealmLink target="flagGroups">FlagGroups</RealmLink>
                <RealmLink target="flagTypes">FlagTypes</RealmLink>
                <RealmLink target="dayFlags">DayFlags</RealmLink>
            </ul>
        </div>
    );
};

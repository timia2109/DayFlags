import { FC } from "react";

export const Welcome: FC = () => (
    <div className="hero min-h-screen bg-base-200">
        <div className="hero-content text-center">
            <div className="max-w-md">
                <h1 className="text-5xl font-bold">Welcome to DayFlags</h1>
                <p className="py-6">A simple app for flagging days</p>
                <button className="btn btn-primary">Get Started</button>
            </div>
        </div>
    </div>
);

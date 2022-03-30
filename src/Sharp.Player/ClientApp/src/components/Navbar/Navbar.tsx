import * as React from "react";
import {NavLink} from "react-router-dom";
import clsx from "clsx";

type NavigationItem = {
    name: string;
    to: string;
};

const NavigationButtons = () => {
    const navigation = [
        {name: 'Home', to: '.'},
        {name: 'Games', to: './games'},
        {name: 'Map', to: './map'},
    ] as NavigationItem[];

    return (
        <>
            {navigation.map((item, index) => (
                <NavLink
                    end={index === 0}
                    key={item.name}
                    to={item.to}
                    className={({isActive}) => clsx('px-3 py-2 rounded-md text-sm font-medium', isActive ? "bg-gray-900 text-white" : "text-gray-300 hover:bg-gray-700 hover:text-white")}
                >
                    {item.name}
                </NavLink>
            ))}
        </>
    );
};

export const Navbar = () => {
    return (
        <nav className="bg-gray-800">
            <div className="max-w-7xl mx-auto px-2 sm:px-6 lg:px-8">
                <div className="relative flex items-center justify-between h-16">
                    <div className="flex-1 flex items-stretch justify-start">
                        <div className="block ml-6 flex space-x-4">
                            <NavigationButtons/>
                        </div>
                    </div>
                </div>
            </div>
        </nav>
    );
};

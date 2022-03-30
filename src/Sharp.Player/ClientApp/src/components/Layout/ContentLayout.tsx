import * as React from 'react';

import {Head} from '../Head';
import {Navbar} from "../Navbar";

type TitleProps = {
    title?: string;
}

const Title = ({title}: TitleProps) => {
    if (title) {
        return (
            <div className="max-w-7xl mx-auto px-4 sm:px-6 md:px-8">
                <h1 className="text-2xl font-semibold text-gray-900">{title}</h1>
            </div>
        );
    }
    return (null);
}

type ContentLayoutProps = {
    children: React.ReactNode;
    title?: string;
};

export const ContentLayout = ({children, title}: ContentLayoutProps) => {
    return (
        <>
            <Head title={title}/>
            <Navbar/>
            <div className="py-6">
                <Title title={title}/>
                <div className="max-w-7xl mx-auto px-4 sm:px-6 md:px-8">{children}</div>
            </div>
        </>
    );
};

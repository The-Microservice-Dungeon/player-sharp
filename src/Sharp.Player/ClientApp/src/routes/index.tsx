import { useRoutes } from 'react-router-dom';
import {Landing} from "../features/misc";
import {MapOutlet} from "../features/map";
import {ContentLayout} from "../components/Layout";

export const AppRoutes = () => {
    const routes = [
        {
            path: '/',
            element: <Landing />
        },
        {
            path: '/map',
            element: <MapOutlet />
        }
    ];

    const element = useRoutes(routes);
    return <>
        <ContentLayout>
            {element}
        </ContentLayout>
    </>
}

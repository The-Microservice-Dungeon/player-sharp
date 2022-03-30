import {useRoutes} from 'react-router-dom';
import {Landing} from "../features/misc";
import {MapOutlet} from "../features/map";
import {ContentLayout} from "../components/Layout";
import {GameOverview} from "../features/game";

export const AppRoutes = () => {
    const routes = [
        {
            path: '/',
            element: <Landing/>
        },
        {
            path: '/map',
            element: <MapOutlet/>
        },
        {
            path: '/games',
            element: <GameOverview/>
        }
    ];

    const element = useRoutes(routes);
    return <>
        <ContentLayout>
            {element}
        </ContentLayout>
    </>
}

import { useRoutes } from 'react-router-dom';
import {Landing} from "../features/misc";

export const AppRoutes = () => {
    const routes = [
        {
            path: '/',
            element: <Landing />
        }  
    ];
    
    const element = useRoutes(routes);
    return <>{element}</>
}
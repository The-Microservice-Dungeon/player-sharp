import {getMap, Map, mapConnection} from "../../client";
import {useQuery} from "react-query";
import {useEffect} from "react";

export type MapProps = {};

export const MapOutlet = (props: MapProps) => {
    const { isLoading, error, data } = useQuery<Map, Error>('mapData', () => getMap())

    useEffect((): () => void => {
        mapConnection.start()
            .then(result => {
                console.log("Connected");

                mapConnection.on('FieldUpdated', message => {
                    console.log(message);
                });
            })
            .catch(err => console.error(err));

        return () => mapConnection.stop();
    }, [mapConnection]);

    if (isLoading) return (
        <span>{'Loading...'}</span>
    );

    if (error) return (<span>{'An error has occurred: ' + error.message}</span>);
    if (!data) return (<span>{'No Map could be fetched'}</span>);

    return (<div>
        <span>{'Map ID: ' + data.id}</span>
    </div>);
}

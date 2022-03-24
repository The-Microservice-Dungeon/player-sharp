import {getMap, Map} from "../../client";
import {useQuery} from "react-query";
import {CSSProperties, useEffect, useLayoutEffect, useRef, useState} from "react";
import { Edge, Network, Node} from "vis-network";

const buildNodesFromMap = (map: Map): Node[] => {
    return Object.entries(map.fields)
        .map(([key, value]) => ({
            id: key,
            label: !!value.spacestation ? 'Spacestation' : '' + !!value.planet ? 'Planet' : ''
        }));
}

const buildEdgesFromMap = (map: Map): Edge[] => {
    return Object.entries(map.fields)
        .map(([key, value]) =>
            value.connections.map(connection => ({
                    from: key,
                    to: connection
                }))).flat();
}

export type MapGraphProps = {
    map: Map;
    className?: string;
    style?: CSSProperties;
};

// Percentages dont work
export const MapGraph = ({ style = { width: "100%", height: "80vh", border: "1px solid red" }, map, ...props}: MapGraphProps) => {
    const container = useRef<HTMLDivElement | null>(null);
    const [dimensions, setDimensions] = useState({});
    const [ nodes, setNodes ] = useState<Node[]>(buildNodesFromMap(map));
    const [ edges, setEdges ] = useState<Edge[]>(buildEdgesFromMap(map));
    const [ network, setNetwork ] = useState<Network | null>(null);

    useEffect(() => {
        if(container.current) {
            setNetwork(new Network(container.current, {nodes: nodes, edges: edges}, {}));
        }
    }, []);

    return (
        <>
          <div
              ref={container}
              className={props.className}
              style={style}
          >
          </div>
        </>);
};

export type MapOutletProps = {};

export const MapOutlet = (props: MapOutletProps) => {
    const { isLoading, error, data } = useQuery<Map, Error>('mapData', () => getMap());

    // TODO: Later
    /*const [ connection, setConnection ] = useState<HubConnection>();

    useEffect(() => {
        setConnection(mapConnection);
    }, []);

    useEffect((): () => void => {
        if(connection) {
            connection.start()
                .then(result => {
                    console.log("Connected");

                    connection.on('FieldUpdated', message => {
                        console.log(message);
                    });
                })
                .catch(err => console.error(err));
            return () => connection.stop();
        }
        return () => {};
    }, [connection]);*/

    if (isLoading) return (
        <span>{'Loading...'}</span>
    );

    if (error) return (<span>{'An error has occurred: ' + error.message}</span>);
    if (!data) return (<span>{'No Map could be fetched'}</span>);

    return (<div>
        <span>{'Map ID: ' + data.id}</span>
        <div className="w-full h-full">
            <MapGraph map={data} />
        </div>
    </div>);
};

import {getMap, Map} from "../../client";
import {useQuery} from "react-query";
import {CSSProperties, useEffect, useLayoutEffect, useRef, useState} from "react";
import {Edge, Network, Node, Options} from "vis-network";

const buildNodesFromMap = (map: Map): Node[] => {
    const colorSpaceStation = "#FFA701";
    const colorPlanet = "#86DC3D";
    const colorUndefined = "#D4D4D4";

    return Object.entries(map.fields)
        .map(([key, value]) => ({
            id: key,
            color: value.spacestation ? colorSpaceStation : !!value.planet ? colorPlanet : colorUndefined,
            label: key
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
    const [ nodes ] = useState<Node[]>(buildNodesFromMap(map));
    const [ edges ] = useState<Edge[]>(buildEdgesFromMap(map));
    const [ network, setNetwork ] = useState<Network | null>(null);

    const options: Options = {
        nodes: {
            shape: "dot"
        },
        edges: {
            color: {
                inherit: true
            },
            smooth: true
        },
        physics: {
            stabilization: true
        },
    }

    useEffect(() => {
        if(container.current) {
            setNetwork(new Network(container.current, {nodes: nodes, edges: edges}, options));
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

import {getMap, Map} from "../../client";
import {useQuery} from "react-query";
import {CSSProperties, useEffect, useLayoutEffect, useRef, useState} from "react";
import {Edge, Network, Node, Options} from "vis-network";

const colorSpaceStation = "#FFA701";
const colorPlanet = "#86DC3D";
const colorUndefined = "#D4D4D4";

const buildLegendNodes = (startX: number, startY: number, step: number = 90): Node[] => {
    const legendEntries: Node[] = [
        {
            label: "Spacestation",
            color: colorSpaceStation
        },
        {
            label: "Planet",
            color: colorPlanet
        }
    ];
    return legendEntries.map((e, i) => ({
        ...e,
        x: startX,
        y: (e.y ?? startY) + i * step,
        fixed: true,
        physics: false,
        level: 9999,
        id: `legend-${i}`
    }));
};

const buildNodesFromMap = (map: Map): Node[] => {
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
export const MapGraph = ({
                             style = {width: "100%", height: "80vh", border: "1px solid red"},
                             map,
                             ...props
                         }: MapGraphProps) => {
    const container = useRef<HTMLDivElement | null>(null);
    const [nodes] = useState<Node[]>(buildNodesFromMap(map));
    const [edges] = useState<Edge[]>(buildEdgesFromMap(map));
    const [ legend, setLegend ] = useState<Node[]>([]);
    const [network, setNetwork] = useState<Network | null>(null);

    const options: Options = {
        nodes: {
            shape: "dot",
            scaling: {
                min: 16,
                max: 32
            }
        },
        edges: {
            smooth: true
        },
        physics: {
            stabilization: true
        },
    }

    useEffect(() => {
        if (container.current) {
            setNetwork(new Network(container.current, {nodes: [...nodes, ...legend], edges: edges}, options));
        }
    }, [nodes, legend]);

    useEffect(() => {
        if(container.current) {
            const lStartX = -container.current?.clientWidth / 2;
            const lStartY = -container.current?.clientHeight / 2;
            console.log({
                lStartX,
                lStartY
            })
            setLegend(buildLegendNodes(lStartX, lStartY));
        }
    }, [])

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
    const {isLoading, error, data} = useQuery<Map, Error>('mapData', async () => await getMap(), {useErrorBoundary: false});

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

    if (error) {
        return (<span>{'An error has occurred: ' + error.message}</span>);
    }
    if (!data) return (<span>{'No Map could be fetched'}</span>);

    return (<div>
        <span>{'Map ID: ' + data.id}</span>
        <div className="w-full h-full">
            <MapGraph map={data}/>
        </div>
    </div>);
};

import {useQuery} from "react-query";
import {Game, GameState, getGames} from "../../client";
import clsx from "clsx";

const bgStatusClasses: Record<GameState, string> = {
    "AVAILABLE": "bg-green-600",
    "RUNNING": "bg-yellow-600",
    "FINISHED": "bg-red-600"
};

const borderStatusClasses: Record<GameState, string> = {
    "AVAILABLE": "border-green-600",
    "RUNNING": "border-yellow-600",
    "FINISHED": "border-red-600"
};

const txtStatusClasses: Record<GameState, string> = {
    "AVAILABLE": "text-green-100",
    "RUNNING": "text-yellow-100",
    "FINISHED": "text-red-100"
};

export const GameStatusIndicator = (props: Pick<GameEntryProps, "state">) => {
    return (
        <div className={clsx("ml-3 my-5 p-1 w-20", bgStatusClasses[props.state], txtStatusClasses[props.state])}>
            <div className="uppercase text-xs leading-4 font-semibold text-center">{props.state}</div>
        </div>
    );
}

type GameEntryProps = {
    state: GameState,
    id: string;
};

// Tailwind Source: https://tailwindcomponents.com/component/test-5
const GameEntry = (props: GameEntryProps) => {
    return (
        <>
            <div className="bg-gray-100 mx-auto border-gray-500 border rounded-sm text-gray-700 mb-0.5 h-30">
                <div className={clsx("flex p-3 border-l-8", borderStatusClasses[props.state])}>
                    <div className="flex-1 border-r-2 pr-3">
                        <div className="ml-3 space-y-1">
                            <div className="text-base leading-6 font-normal">{props.id}</div>
                        </div>
                    </div>
                    <div>
                        <GameStatusIndicator state={props.state}/>
                    </div>
                </div>
            </div>
        </>
    );
}

type GameOverviewProps = {};

export const GameOverview = (props: GameOverviewProps) => {
    const {isLoading, error, data} = useQuery<Game[], Error>('gameData', () => getGames());

    if (isLoading) return (
        <span>{'Loading...'}</span>
    );

    if (error) return (<span>{'An error has occurred: ' + error.message}</span>);
    if (!data) return (<span>{'No Games could be fetched'}</span>);

    return (
        <div>
            <h1>Player Game History</h1>
            <div>
                {data.map(game => <GameEntry state={"AVAILABLE"} id={game.id} key={game.id}/>)}
            </div>
        </div>
    );
};

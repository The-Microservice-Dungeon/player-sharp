import {axios} from "../lib/axios";

export type GameState = "AVAILABLE" | "RUNNING" | "FINISHED";
export type GameId = string;

export type Game = {
    id: GameId;
};

export const getGames = (): Promise<Game[]> => {
    return axios.get("/games");
}

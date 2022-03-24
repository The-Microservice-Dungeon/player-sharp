import {axios} from "../lib/axios";
import { HubConnectionBuilder } from '@microsoft/signalr';
import {API_URL} from "../config/config";
import {buildConnection} from "../lib/signalr";

export type GameState = "AVAILABLE" | "RUNNING" | "FINISHED";
export type GameId = string;

export type Game = {
    id: GameId;
};

export const getGames = (): Promise<Game[]> => {
    return axios.get("/games");
}

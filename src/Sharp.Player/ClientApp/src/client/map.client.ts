import {axios} from "../lib/axios";
import { HubConnectionBuilder } from '@microsoft/signalr';
import {API_URL} from "../config/config";
import {buildConnection} from "../lib/signalr";

export type FieldId = string;

export type Map = {
    id: string;
    fields: Record<FieldId, Field>;
};

export type Field = {
    movementDifficulty?: number;
    planet?: Planet;
    spacestation?: SpaceStation;
    connections: FieldId[];
};

export type Planet = {
};

export type SpaceStation = {
};

export const getMap = (): Promise<Map> => {
    return axios.get("/map");
}

export const mapConnection = buildConnection("/socket/map");

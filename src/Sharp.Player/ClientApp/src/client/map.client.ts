import { Map } from "./types";
import {axios} from "../lib/axios";
import { HubConnectionBuilder } from '@microsoft/signalr';
import {API_URL} from "../config/config";
import {buildConnection} from "../lib/signalr";

export const getMap = (): Promise<Map> => {
    return axios.get("/map");
}

export const mapConnection = buildConnection("/socket/map");
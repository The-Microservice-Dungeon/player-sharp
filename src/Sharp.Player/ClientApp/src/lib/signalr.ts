import {HubConnectionBuilder} from "@microsoft/signalr";
import {API_URL} from "../config/config";

export const buildConnection = (path: string) => new HubConnectionBuilder()
    .withUrl(`${API_URL}${path.startsWith("/") ? path : `/${path}`}`)
    .withAutomaticReconnect()
    .build();
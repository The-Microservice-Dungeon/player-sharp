export interface Config {
    API_URL: string;
}

export const config: Config = {
    API_URL: process.env.REACT_APP_API_URL as string
}

export const { API_URL } = config;
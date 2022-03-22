import { API_URL} from "../config/config";
import Axios, { AxiosRequestConfig } from "axios";

export const axios = Axios.create({
    baseURL: API_URL
});

axios.interceptors.response.use(
    res => res.data,
    err => {
        const message = err.response?.data?.message || err.message;
        console.error({
            message,
            error: err
        });
        
        return Promise.reject(err);
    } 
);
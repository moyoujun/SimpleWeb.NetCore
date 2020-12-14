import Axios, { AxiosRequestConfig } from 'axios';
import { reactive } from 'vue';

export interface IUserState {
    isAuth: boolean;
    token: string;
    name: string;
}

export interface IAppState {
    user: IUserState;
}


export function createState() {
    let state: IAppState = {
        user: {
            isAuth: false,
            token: "",
            name: ""
        }
    }
    return reactive(state)
}

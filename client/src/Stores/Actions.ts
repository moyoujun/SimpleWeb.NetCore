import Axios, { AxiosRequestConfig } from 'axios';
import { IAppState, IUserState } from "./State";

function UserLogin(state: IAppState, response: any) {
    state.user = convertResponseToUserState(response);
    localStorage.setItem("user", JSON.stringify(state.user));
}

async function checkStoredToken(state: IAppState) {
    let item = localStorage.getItem("user");

    if (item) {
        let user = JSON.parse(item) as IUserState;
        let config: AxiosRequestConfig = {
            url: "/api/Account/is_auth",
            method: "get",
            headers: {
                Authorization: "Bearer " + user.token,
            },
        };
        try {
            let o = await Axios.request(config);
            user.isAuth = true;
            state.user = user;
        }
        catch (error) {
            console.log(error);
            localStorage.removeItem("user");
            state.user.isAuth = false;
            state.user.token = "";
            state.user.name = "";
        }
    }
}

function convertResponseToUserState(response: any): IUserState {
    let ret: IUserState = {
        isAuth: false,
        token: "",
        name: "",
    };

    if (response.status == 0) {
        ret.isAuth = true;
        ret.token = response.data.token;
        ret.name = response.data.userName;
    }
    return ret;
}

export function createAction(state: IAppState) {
    return {
        UserLogin: (o: any) => UserLogin(state, o),
        checkStoredToken: async () => await checkStoredToken(state)
    }
}
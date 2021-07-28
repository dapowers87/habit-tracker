import axios, { AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { history } from "../App"; 
import ITrackerWindow from "../types/ITrackerWindow";

declare global {
    interface Window { 
        _env_: any; 
    }
}

axios.defaults.baseURL = `${window._env_.REACT_APP_BACKEND_URL}/api`;

axios.interceptors.response.use(undefined, (error: any) => {
    if(error.name === "Error" && !error.response) {
        if(!error.config.url.toLowerCase().includes('heartbeat') && !error.config.url.toLowerCase().includes('nextemailtime')) {
            toast.error(`Unknown error when when trying to connect to ${error.config.url}`)
        }
    } else if(error.response.status === 401) {
        localStorage.removeItem("jwt");

        if(!localStorage.getItem("isRegistering")) {
            history.push("/login");
        }
    } else if(error.response.status === 403) {
        toast.error("You do not have sufficient rights to view this resource or perform the attempted action");
    } else if(error.response.status === 400) {
        return Promise.reject(error.response.data);
    } else {
        toast.error(`${error.message} for URL ${error.config.baseURL}${error.config.url}`);
    }

    if(error.response && error.response.data) {
        return Promise.reject(error.response.data);
    } else {
        return Promise.reject(error.message + "\n" + error.stack);
    }
});

axios.interceptors.request.use(
    (config: any) => {
        const jwt = localStorage.getItem("jwt");

        if(jwt) {
            config.headers.Authorization = `Bearer ${jwt}`;
        }
        
        return config;
    },
    (error: any) => {
      return Promise.reject(error);
    }
  );

const responseBody = async (response: AxiosResponse) =>( await response.data );

const requests = {
    get: async (url: string) =>
        await axios
            .get(url)
            .then(responseBody)
            .catch((e: any) => {
                console.log(e);
            }),
    post: async (url: string, body: {}) =>
        await axios
            .post(url, body)
            .then(responseBody, (value: AxiosResponse) => {
                if (value.data)
                    return value.data;
                else
                    return value;
            })
            .catch((e: any) => {
                console.log(e);
            }),
    put: async (url: string, body: {}) =>
        await axios
            .put(url, body)
            .then(responseBody)
            .catch((e: any) => {
                console.log(e);
            }),
    del: async (url: string) => 
        await axios
            .delete(url)
            .then((value: AxiosResponse) => {
                if (value.data)
                    return value.data;
                else
                    return value.status;
            })
            .catch((e: any) => {
                console.log(e);
            })
};

const Login = {
    login: async (username: string, password: string) => await requests.post(`/v1/Account/Login?username=${username}&password=${password}`, { }),
    quickAuthorizationCheck: async () => await requests.get('/v1/Account/QuickAuthorizationCheck'),
    create: async (username: string, password: string) => await requests.post(`/v1/Account/CreateUser?username=${username}&password=${password}`, { }),
    getAllUsers: async () => await requests.get('/v1/Account/GetUsernames') as string[]
}

const Window = {
    create: async (window: ITrackerWindow) => await requests.post('/v1/Window/Create', { ...window }),
    delete: async (windowId: number) => await requests.del(`/v1/Window/Delete?windowId=${windowId}`),
    get: async (windowId: number) => await requests.get(`/v1/Window/Get?windowId=${windowId}`) as ITrackerWindow,
    getAll: async () => await requests.get(`/v1/Window/GetAll`) as ITrackerWindow[],
    update: async (window: ITrackerWindow) => await requests.put('/v1/Window/Update', { ...window }),
    updateCheatDays: async (windowId: number, newCheatDaysUsed: number) => await requests.post(`/v1/Window/UpdateCheatDays?windowId=${windowId}&newCheatDaysUsed=${newCheatDaysUsed}`, { })
}

const exportedObject = {
    Login,
    Window
}

export default exportedObject;

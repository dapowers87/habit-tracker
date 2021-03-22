import React from "react";
import ISbrWindow from "./types/Window";

export interface IInitialState {
    Windows: ISbrWindow[]|undefined;
    IsLoggedIn: boolean;
}

export const initialState = {
    Windows: undefined,
    IsLoggedIn: false
} as IInitialState

export const types = {
    SETWINDOWS: "SETWINDOWS",
    SETISLOGGEDIN: 'SETISLOGGEDIN'
}

export const AppContext = React.createContext(initialState as any);

export const reducer = (state: any, action: any) => {
    switch (action.type) {
        case types.SETWINDOWS: {
            return { ...state, Windows: action.Windows };
        }
        case types.SETISLOGGEDIN: {
            return { ...state, IsLoggedIn: action.IsLoggedIn };
        }
    }
}
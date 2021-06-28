import ITrackerWindow from "../types/ITrackerWindow";

export const WindowUtil = {
    updateWindow: (window: ITrackerWindow, windowCollection: ITrackerWindow[]) => {
        const index: number = windowCollection.map((subItem: ITrackerWindow) => subItem.windowId).indexOf(window.windowId);
        const preWindow = windowCollection.slice(0, index);
        const postWindow = windowCollection.slice(index + 1);

        return [...preWindow, window, ...postWindow]
    }
}
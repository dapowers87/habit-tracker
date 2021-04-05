import ISbrWindow from "../types/ISbrWindow";

export const WindowUtil = {
    updateWindow: (window: ISbrWindow, windowCollection: ISbrWindow[]) => {
        const index: number = windowCollection.map((subItem: ISbrWindow) => subItem.windowId).indexOf(window.windowId);
        const preWindow = windowCollection.slice(0, index);
        const postWindow = windowCollection.slice(index + 1);

        return [...preWindow, window, ...postWindow]
    }
}
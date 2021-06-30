import React, { Fragment, useCallback, useContext, useEffect } from 'react'
import { Button, Card } from 'semantic-ui-react';
import agent from '../../api/agent';
import { AppContext, IInitialState, types } from '../../store';
import ITrackerWindow from '../../types/ITrackerWindow';
import WindowCard from './components/WindowCard';

const TrackerWindow: React.FC = () => {

    const { state, dispatch } = useContext(AppContext);
    const { Windows } = state as IInitialState;

    const loadWindows = useCallback(async () => {
        const windows = await agent.Window.getAll();

        dispatch({type: types.SETWINDOWS, Windows: windows});
    }, [dispatch]);

    useEffect(() => {
        loadWindows();
    }, [loadWindows]);

    if(!Windows) {
        return <div/>
    }
    
    return (
        <Fragment>
            <Button>New Window</Button>
            <Card.Group centered style={{marginTop: '20px'}}>
                {Windows.map((window: ITrackerWindow, i: number) => (
                    <WindowCard key={'window-' + i} window={window} />
                ))}
            </Card.Group>
        </Fragment>
    )
}

export default TrackerWindow;
import React, { Fragment, useCallback, useContext, useEffect } from 'react'
import { Card } from 'semantic-ui-react';
import agent from '../../api/agent';
import { AppContext, IInitialState, types } from '../../store';
import ISbrWindow from '../../types/ISbrWindow';
import WindowCard from './components/WindowCard';

const SbrWindow: React.FC = () => {

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
            <Card.Group centered style={{marginTop: '20px'}}>
                {Windows.map((window: ISbrWindow) => (
                    <WindowCard window={window} />
                ))}
            </Card.Group>
        </Fragment>
    )
}

export default SbrWindow;
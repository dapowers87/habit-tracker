import React, { Fragment, useCallback, useContext, useEffect, useState } from 'react'
import { Button, Card, Grid } from 'semantic-ui-react';
import agent from '../../api/agent';
import { AppContext, IInitialState, types } from '../../store';
import ITrackerWindow from '../../types/ITrackerWindow';
import CreateUpdateModal from './components/CreateUpdateModal';
import WindowCard from './components/WindowCard';

const TrackerWindow: React.FC = () => {

    const { state, dispatch } = useContext(AppContext);
    const { Windows } = state as IInitialState;

    const [showCreateEditModal, setShowCreateEditModal] = useState<boolean>(false);

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
            <Grid>
                <Grid.Column textAlign='center'>
                    <Grid.Row>
                        <Button icon='plus' content='New Habit' style={{marginTop: '10px'}} positive onClick={() => setShowCreateEditModal(true)}/>
                    </Grid.Row>
                </Grid.Column>
            </Grid>
            <CreateUpdateModal open={showCreateEditModal} setOpen={setShowCreateEditModal}/>
            <Card.Group centered style={{marginTop: '20px'}}>
                {Windows.map((window: ITrackerWindow, i: number) => (
                    <WindowCard key={'window-' + i} window={window} />
                ))}
            </Card.Group>
        </Fragment>
    )
}

export default TrackerWindow;
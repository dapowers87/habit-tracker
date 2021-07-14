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
    
    const LogOut = () => {
        localStorage.removeItem('jwt');
        window.location.reload();
    }

    return (
        <Fragment>
            <Grid>
                <Grid.Row>
                    <Grid.Column textAlign='center'>
                        <Button floated='right' icon='logout' content='Log Out' style={{marginTop: '10px', marginRight: '10px'}} onClick={LogOut}/>
                    </Grid.Column>
                </Grid.Row>
                <Grid.Row textAlign='center'>
                    <Grid.Column textAlign='center'>
                        <Button icon='plus' content='New Habit' style={{marginTop: '10px'}} positive onClick={() => setShowCreateEditModal(true)}/>
                    </Grid.Column>
                </Grid.Row>
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
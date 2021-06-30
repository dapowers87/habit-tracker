import dateFormat from 'dateformat';
import React, { useCallback, useContext, useState } from 'react'
import { Button, Card, Header, Label, Progress, Statistic } from 'semantic-ui-react';
import agent from '../../../api/agent';
import { AppContext, IInitialState, types } from '../../../store';
import ITrackerWindow from '../../../types/ITrackerWindow';
import { WindowUtil } from '../../../util/WindowUtil';

const WindowCard: React.FC<{window: ITrackerWindow}> = ({ window }) => {

    const { state, dispatch } = useContext(AppContext);
    const { Windows } = state as IInitialState;

    const [disableAddButton, setDisableAddButton] = useState<boolean>(false);
    const [disableRemoveButton, setDisableRemoveButton] = useState<boolean>(false);
    const [, setMinuteCounter] = useState<number>(0);

    const handleAddCheatDay = async () => {
        setDisableAddButton(true);
        await agent.Window.updateCheatDays(window.windowId, ++window.numberOfCheatDaysUsed);
        setDisableAddButton(false);

        dispatch({type: types.SETWINDOWS, Windows: WindowUtil.updateWindow(window, Windows ?? [])})
    }

    const handleRemoveCheatDay = async () => {
        setDisableRemoveButton(true);
        await agent.Window.updateCheatDays(window.windowId, --window.numberOfCheatDaysUsed);
        setDisableRemoveButton(false);

        dispatch({type: types.SETWINDOWS, Windows: WindowUtil.updateWindow(window, Windows ?? [])})
    }

    const updateMinute = () => {
        setTimeout(() => 
        {
            setMinuteCounter(prevCount => prevCount + 1);
            updateMinute();
        }, 60000);
    }

    updateMinute();

    const getCurrentNumberOfDays = useCallback(() => {
        const oneDay = 24 * 60 * 60 * 1000;
        const startDate = new Date(window.startDate);
        const endDate = new Date();
        const diffInTime = endDate.getTime() - startDate.getTime();
        const numDays = diffInTime / oneDay;

        return numDays;
    }, [window.startDate]);

    const getPercentage = () => {
        const percentage = Math.ceil(getCurrentNumberOfDays() / window.numberOfDays * 100);

        return percentage;
    }

    const isActive = () => {
        const now = new Date();
        const startDate = new Date(window.startDate);
        const endDate = startDate.setDate(startDate.getDate() + window.numberOfDays);

        return endDate as any > now;
    }

    return (
        <>
            <Card style={{width: '75%'}}>
                <Card.Header>
                    <Header as='h2' textAlign='center'>Window #{window.windowId}</Header>
                </Card.Header>
                <Card.Meta textAlign='center'>Date Range: {dateFormat(window.startDate, 'm/dd/yyyy')} - {dateFormat((new Date(window.startDate)).setDate((new Date(window.startDate)).getDate() + window.numberOfDays), 'm/dd/yyyy')}</Card.Meta>
                <Card.Content>
                    { isActive()
                    ?
                        <Progress label={`Day ${Math.ceil(getCurrentNumberOfDays())} of  ${window.numberOfDays}`} percent={getPercentage()} indicating progress/>
                    :
                        <Label ribbon color='green'>COMPLETE</Label>
                    }
                </Card.Content>
                <Card.Content>
                    <div style={{display: 'flex', justifyContent: 'center'}}>
                        <Statistic.Group>
                            <Statistic>
                                <Statistic.Value>{window.numberOfDays}</Statistic.Value>
                                <Statistic.Label>Window Length</Statistic.Label>
                            </Statistic>
                            <Statistic>
                                <Statistic.Value>{window.numberOfCheatDays}</Statistic.Value>
                                <Statistic.Label>Number of Cheat Days</Statistic.Label>
                            </Statistic>
                            <Statistic>
                                <Statistic.Value>{window.numberOfCheatDaysUsed}</Statistic.Value>
                                <Statistic.Label>Cheat Days Used</Statistic.Label>
                            </Statistic>
                        </Statistic.Group>
                    </div>
                </Card.Content>
                <Card.Content>
                    <div style={{display: 'flex', justifyContent: 'center'}}>
                        <Button positive content="Take a Cheat Day" onClick={handleAddCheatDay} loading={disableAddButton} disabled={disableAddButton || window.numberOfCheatDaysUsed === window.numberOfCheatDays}/>
                        <Button negative content="Remove a Cheat Day" onClick={handleRemoveCheatDay} loading={disableRemoveButton} disabled={disableRemoveButton || window.numberOfCheatDaysUsed === 0}/>
                    </div>
                </Card.Content>
            </Card>
        </>
    )
}

export default WindowCard;
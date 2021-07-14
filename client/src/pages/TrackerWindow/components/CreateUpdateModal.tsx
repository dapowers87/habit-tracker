import React, { useContext, useEffect, useState } from 'react'
import SemanticDatepicker from 'react-semantic-ui-datepickers';
import { toast } from 'react-toastify';
import { Form, Modal } from 'semantic-ui-react';
import agent from '../../../api/agent';
import { AppContext, IInitialState, types } from '../../../store';
import ITrackerWindow from '../../../types/ITrackerWindow';

const CreateUpdateModal: React.FC<{window?: ITrackerWindow, open: boolean, setOpen: (newVal: boolean) => void}> = ({window, open, setOpen}) => {

    const { state, dispatch } = useContext(AppContext);
    const { Windows } = state as IInitialState;
    
    const [windowName, setWindowName] = useState<string>('');
    const [startDate, setStartDate] = useState<Date>(new Date());
    const [numberOfDays, setNumberOfDays] = useState<number>(100);
    const [numberOfCheatDays, setNumberOfCheatDays] = useState<number>(10);

    useEffect(() => {
        if(window) {
            setWindowName(window.windowName);
            setStartDate(new Date(window.startDate));
            setNumberOfDays(window.numberOfDays);
            setNumberOfCheatDays(window.numberOfCheatDays);
        }
    }, [window]);

    const handleCreateOrUpdate = async () => {
        const clearModal = () => {
            setWindowName('');
            setStartDate(new Date());
            setNumberOfDays(100);
            setNumberOfCheatDays(10);
        };

        if(numberOfCheatDays > numberOfDays) {
            toast.error('Number of cheat days cannot be greater than number of days');
            return;
        }

        let windowToSend = {
            windowName,
            startDate,
            numberOfCheatDays,
            numberOfDays
        } as ITrackerWindow;

        if(window && Windows) {
            windowToSend.windowId = window.windowId;
            windowToSend.numberOfCheatDaysUsed = window.numberOfCheatDaysUsed;

            const result = await agent.Window.update(windowToSend); 
            
            if(result) {
                toast.success("Habit updated");
                
                const index: number = Windows.map((subItem: ITrackerWindow) => subItem.windowId).indexOf(window.windowId);
                const preWindow = Windows.slice(0, index);
                const postWindow = Windows.slice(index + 1);

                dispatch({type: types.SETWINDOWS, Windows: [...preWindow, windowToSend, ...postWindow]});
                setOpen(false);
            }
        } else {

            const result = await agent.Window.create(windowToSend);

            if(result && result !== 2) {
                toast.success(`Habit '${windowName}' Created`);

                const newWindow = {...windowToSend, windowId: result, numberOfCheatDaysUsed: 0 } as ITrackerWindow;

                if(Windows) {
                    dispatch({type: types.SETWINDOWS, Windows: [newWindow, ...Windows]});
                } else {
                    dispatch({type: types.SETWINDOWS, Windows: [newWindow]});
                }

                clearModal();
                
                setOpen(false);
            } else {
                toast.error("Error creating habit");
            }
        }
    }

    return (
        <>
            <Modal open={open} closeIcon onClose={() => setOpen(false)}>
                <Modal.Header>New Habit</Modal.Header>
                <Modal.Content>
                    <Form>
                        <Form.Input fluid label="Habit Name" placeholder='Running' value={windowName} onChange={(_, val) => setWindowName(val.value)}/> 
                        <SemanticDatepicker label='Start Date' value={startDate} onChange={(_, value) => setStartDate(value.value as Date)} />
                        <Form.Input min={1} type='number' fluid label="Habit Length" placeholder='100' value={numberOfDays} 
                            onChange={(_, value) => setNumberOfDays(parseInt(value.value))}/> 
                        <Form.Input min={0} type='number' fluid label="Number of Cheat Days" placeholder='10' value={numberOfCheatDays}
                            onChange={(_, value) => setNumberOfCheatDays(parseInt(value.value))}/> 
                        <Form.Button positive onClick={handleCreateOrUpdate}>{window ? 'Update' : 'Create'}</Form.Button>
                    </Form>
                </Modal.Content>
            </Modal>
        </>
    )
}

export default CreateUpdateModal;
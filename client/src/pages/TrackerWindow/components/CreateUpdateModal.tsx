import React, { useState } from 'react'
import SemanticDatepicker from 'react-semantic-ui-datepickers';
import { toast } from 'react-toastify';
import { Form, Modal } from 'semantic-ui-react';
import agent from '../../../api/agent';
import ITrackerWindow from '../../../types/ITrackerWindow';

const CreateUpdateModal: React.FC<{open: boolean, setOpen: (newVal: boolean) => void}> = ({open, setOpen}) => {

    const [windowName, setWindowName] = useState<string>('');
    const [startDate, setStartDate] = useState<Date>(new Date());
    const [numberOfDays, setNumberOfDays] = useState<number>(0);
    const [numberOfCheatDays, setNumberOfCheatDays] = useState<number>(0);

    const handleCreateOrUpdate = async () => {
        const window = {
            windowName,
            startDate,
            numberOfCheatDays,
            numberOfDays
        } as ITrackerWindow;

        const result = await agent.Window.create(window);

        if(result) {
            toast.success("Window Created");
            setOpen(false);
        }
    }

    return (
        <>
            <Modal open={open} closeIcon onClose={() => setOpen(false)}>
                <Modal.Header>New Habit</Modal.Header>
                <Modal.Content>
                    <Form>
                        <Form.Input fluid label="Window Name" placeholder='Running' value={windowName} onChange={(_, val) => setWindowName(val.value)}/> 
                        <SemanticDatepicker label='Start Date' value={startDate} onChange={(_, value) => setStartDate(value.value as Date)} />
                        <Form.Input min={1} type='number' fluid label="Window Length" placeholder='100' value={numberOfCheatDays} 
                            onChange={(_, value) => setNumberOfDays(parseInt(value.value))}/> 
                        <Form.Input min={0} type='number' fluid label="Number of Cheat Days" placeholder='10' value={numberOfCheatDays}
                            onChange={(_, value) => setNumberOfCheatDays(parseInt(value.value))}/> 
                        <Form.Button positive onClick={handleCreateOrUpdate}>Create</Form.Button>
                    </Form>
                </Modal.Content>
            </Modal>
        </>
    )
}

export default CreateUpdateModal;
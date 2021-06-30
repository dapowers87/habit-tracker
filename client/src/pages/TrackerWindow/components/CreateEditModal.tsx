import React, { useState } from 'react'
import SemanticDatepicker from 'react-semantic-ui-datepickers';
import { Form, Modal } from 'semantic-ui-react';

const CreateEditModal: React.FC<{open: boolean, setOpen: (newVal: boolean) => void}> = ({open, setOpen}) => {

    const [windowName, setWindowName] = useState<string>('');
    const [startDate, setStartDate] = useState<Date>(new Date());
    const [windowLength, setWindowLength] = useState<number>(0);
    const [numCheatDays, setNumCheatDays] = useState<number>(0);

    return (
        <>
            <Modal open={open} closeIcon onClose={() => setOpen(false)}>
                <Modal.Header>New Habit</Modal.Header>
                <Modal.Content>
                    <Form>
                        <Form.Input fluid label="Window Name" placeholder='Running' value={windowName} onChange={(_, val) => setWindowName(val.value)}/> 
                        <SemanticDatepicker label='Start Date' value={startDate} onChange={(_, value) => setStartDate(value.value as Date)} />
                        <Form.Input type='number' fluid label="Window Length" placeholder='100'/> 
                        <Form.Input type='number' fluid label="Number of Cheat Days" placeholder='10'/> 
                        <Form.Button>Create</Form.Button>
                    </Form>
                </Modal.Content>
            </Modal>
        </>
    )
}

export default CreateEditModal;
import React, { Fragment, useEffect, useState } from 'react'
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';
import { InputOnChangeData, Modal, Form } from 'semantic-ui-react';
import agent from '../../api/agent';

const RegisterModal: React.FC = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [password2, setPassword2] = useState('');
    const [isRegistering, setIsRegistering] = useState<boolean>(false);

    let history = useHistory();

    useEffect(() => {
        return () => {
          localStorage.removeItem('isRegistering');
        };
    }, []);

    const Register = async () => {
        if(password !== password2) {
            toast.error("Passwords do not match");
            return;
        } else if(!username) {
            toast.error("Enter a username");
            return
        } else if(!password) {
            toast.error("Enter a password");
            return;
        }

        try {
            setIsRegistering(true);

            var response = await agent.Login.create(username, password);

            if(response) {
                localStorage.removeItem('isRegistering');
                history.push("/login");
            } else {
                toast.error(`Failed to create account. ${response.errorMessage}`);
            }
        }
        finally {
            setIsRegistering(false);
        }
    }

    const changeUsername = (event: React.ChangeEvent<HTMLInputElement>, data: InputOnChangeData) => {
        setUsername(data.value);
    }

    const changePassword = (event: React.ChangeEvent<HTMLInputElement>, data: InputOnChangeData) => {
        setPassword(data.value);
    }

    const changePassword2 = (event: React.ChangeEvent<HTMLInputElement>, data: InputOnChangeData) => {
        setPassword2(data.value);
    }

    return (
        <Fragment>
            <Modal
                closeOnEscape={false}
                closeOnDimmerClick={false}
                open={true}>
                <Modal.Header style={{backgroundColor: 'lightblue'}}>Register</Modal.Header>
                <Modal.Description style={{margin: '10px'}}>Please create an account.</Modal.Description>
                <Modal.Content>
                    <Form>
                        <Form.Input onChange={changeUsername} label='Username' value={username}/>
                        <Form.Input type='password' onChange={changePassword} label='Password' value={password}/>
                        <Form.Input type='password' onChange={changePassword2} label='Retype Password' value={password2}/>
                        <Form.Button floated='right' loading={isRegistering} onClick={Register} color='green' style={{marginBottom: '10px'}}>Register</Form.Button>
                    </Form>
                </Modal.Content>
            </Modal>
        </Fragment>
    )
}

export default RegisterModal;
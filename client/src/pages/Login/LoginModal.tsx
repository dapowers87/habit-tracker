import React, { Fragment, useState, useContext, useEffect } from 'react'
import { Modal, Form, InputOnChangeData, Grid } from 'semantic-ui-react'
import { useHistory } from 'react-router-dom'
import agent from '../../api/agent'
import { AppContext, IInitialState, types } from '../../store'
import { toast } from 'react-toastify'

const LoginModal: React.FC = () => {
    const { state, dispatch } = useContext(AppContext);
    const { IsLoggedIn } = state as IInitialState;
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [isLoggingIn, setIsLoggingIn] = useState<boolean>(false);
    
    let history = useHistory();

    useEffect(() => {
        if(IsLoggedIn) {
            history.push("/");
        }
    });

    const login = async () => {
        try {
            setIsLoggingIn(true);

            var response = await agent.Login.login(username, password);

            if(response) {
                const jwt = response;
                localStorage.setItem("jwt", jwt);
                
                dispatch( { type: types.SETISLOGGEDIN, IsLoggedIn: true } );
                
                history.push("/");
            } else {
                setPassword("");
                toast.error("Incorrect username or password. Try again.");
            }
        }
        finally {
            setIsLoggingIn(false);
        }
    }

    const changeUsername = (event: React.ChangeEvent<HTMLInputElement>, data: InputOnChangeData) => {
        setUsername(data.value);
    }

    const changePassword = (event: React.ChangeEvent<HTMLInputElement>, data: InputOnChangeData) => {
        setPassword(data.value);
    }

    const setRegistering = () => {
        localStorage.setItem("isRegistering", 'true');
    }

    return (
        <Fragment>
            <Modal
                closeOnEscape={false}
                closeOnDimmerClick={false}
                open={true}>
                <Modal.Header style={{backgroundColor: 'lightblue'}}>Login</Modal.Header>
                <Modal.Description style={{margin: '10px'}}>Please login to continue.</Modal.Description>
                <Modal.Content>
                    <Form>
                        <Form.Input onChange={changeUsername} label='Username' value={username}/>
                        <Form.Input type='password' onChange={changePassword} label='Password' value={password}/>
                        <Form.Button floated='right' loading={isLoggingIn} onClick={login} color='green'>Login</Form.Button>
                    </Form>
                    <Grid>
                        <Grid.Column textAlign='center'>
                            <a href='/register' onClick={setRegistering}>Register a New Account</a>
                        </Grid.Column>
                    </Grid>
                </Modal.Content>
            </Modal>
        </Fragment>
    )
}

export default LoginModal;
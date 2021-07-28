import React, { useEffect, useState } from 'react'
import { Modal } from 'semantic-ui-react'
import agent from '../../../api/agent';

const UsersModal: React.FC<{open: boolean, setOpen: (newVal: boolean) => void}> = ({ open, setOpen }) => {

    const [users, setUsers] = useState<string[]>([]);
    
    const fetchUsers = async () => {
        const result = await agent.Login.getAllUsers();

        if(result) {
            setUsers(result);
        }
    }

    useEffect(() => {
        fetchUsers();
    }, []);

    return (
        <>
            <Modal open={open} onClose={() => setOpen(false)}>
                {users.map((user: string) => (
                    <>
                        <div>{user}</div>
                        <br/>
                    </>
                ))}
            </Modal>
        </>
    )
}

export default UsersModal;
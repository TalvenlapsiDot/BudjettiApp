import { useState, useCallback } from 'react'
import { Button, Input, Tooltip} from '@chakra-ui/react'

// Look at Login function for reference on how registartion works, the ideology is mostly the same.

const Register = ({onRegister}) => {
    const [ userName, setUserName] = useState('');
    const [ passWord, setUserPassword] = useState('');

    return (
        <>
        <Tooltip hasArrow label="Minimum of 5 characters. No special characters other than - or _.">
            <Input
                width="100%"
                size='sm'
                placeholder='Username'
                value={userName}
                onChange={(e) => setUserName(e.target.value)}
                />
        </Tooltip>

        <Tooltip hasArrow label="Minimum length of 10, make sure it contains atleast one number and one uppercase letter.">
            <Input
                width="100%"
                size='sm'
                placeholder='Password'
                value={passWord}
                onChange={(b) => setUserPassword(b.target.value)}/>
        </Tooltip>
            <Button
                variant='ghost'
                width="100%"
                backgroundColor='teal.200'
                onClick={() => onRegister(userName, passWord)}
                >
                Register
            </Button>
        </>
    )
}

export default Register;
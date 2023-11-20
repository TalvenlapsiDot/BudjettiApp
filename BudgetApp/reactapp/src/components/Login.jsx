import { useState, useCallback } from 'react'
import {Button, Input} from '@chakra-ui/react'

const Login = ({onLogin}) => {
  const [ userName, setUserName] = useState('');
  const [ passWord, setUserPassword] = useState('');

  return (
    <>
        <Input
            width="100%"
            size='sm'
            placeholder='Username'
            value={userName}
            onChange={(e) => setUserName(e.target.value)} />
        <Input
            width="100%"
            size='sm'
            placeholder='Password'
            type='password'
            value={passWord}
            onChange={(b) => setUserPassword(b.target.value)} />
        <Button
            width="100%"
            variant='ghost'
            backgroundColor='teal.200'
            onClick={() => onLogin(userName, passWord)}>
            Login
        </Button>
    </>
  )
}

export default Login;
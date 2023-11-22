import { useState, useCallback } from 'react'
import {Button, Input, InputGroup, InputRightElement} from '@chakra-ui/react'

const Login = ({onLogin}) => {
  const [ userName, setUserName] = useState('');
  const [ passWord, setUserPassword] = useState('');
  const [show, setShow] = useState(false)
  const handleClick = () => setShow(!show)

  return (
    <>
        <Input
            width="100%"
            size='sm'
            placeholder='Username'
            value={userName}
            onChange={(e) => setUserName(e.target.value)} />
      <InputGroup size="sm">
        <Input
            width="100%"
            size='sm'
            placeholder='Password'
            type={show ? "text" : "password"}
            value={passWord}
            onChange={(b) => setUserPassword(b.target.value)} />
        <InputRightElement>
          <Button size="sm" onClick={handleClick}>
            { show ? " Hide " : " Show "}
          </Button>
        </InputRightElement>
      </InputGroup>
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
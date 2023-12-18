import { useState } from 'react'
import {Button, Input, InputGroup, InputRightElement} from '@chakra-ui/react'

/* Login that takes the state of OnLogin as a parameter. */
const Login = ({onLogin}) => {
  // React UseState hook, when called it sets userName and/or passWord value to be what is given to them (through input fields in this case).
  // React State Hook documentation:
  // https://legacy.reactjs.org/docs/hooks-state.html
  const [ userName, setUserName] = useState('');
  const [ passWord, setUserPassword] = useState('');

  // See ChakraUI input documentation;
  // https://chakra-ui.com/docs/components/input
  const [show, setShow] = useState(false)
  const handleClick = () => setShow(!show)

  return (
    <>
        <Input
            width="100%"
            size='sm'
            placeholder='Username'
            value={userName}
            // onChange (When the value of the input field changes by user) it makes the userName value to be what is currently written in the field.
            onChange={(e) => setUserName(e.target.value)} />
      <InputGroup size="sm">
        <Input
            width="100%"
            size='sm'
            placeholder='Password'
            // Look at Input documentation on using password field type.
            type={show ? "text" : "password"}
            value={passWord}
            onChange={(b) => setUserPassword(b.target.value)} />
        <InputRightElement>
        {/* Button that shows or hides the password field. */}
          <Button size="sm" onClick={handleClick}>
            { show ? " Hide " : " Show "}
          </Button>
        </InputRightElement>
      </InputGroup>
        <Button
            width="100%"
            variant='ghost'
            backgroundColor='teal.200'
            //Button sends the userName and passWord to the onLogin function on click.
            onClick={() => onLogin(userName, passWord)}>
            Login
        </Button>
    </>
  )
}

export default Login;
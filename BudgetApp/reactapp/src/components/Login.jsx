import { useState, useCallback } from 'react'
import {Box, Button, Flex, Input, Heading} from '@chakra-ui/react'
import {ArrowRightIcon} from '@chakra-ui/icons'
import Register from './Register';

const Login = () => {
  const [ login, logUserIn ] = useState(false);
  const [ userName, setUserName] = useState('');
  const [ passWord, setUserPassword] = useState('');

  const handleSubmit = useCallback(() => {
    if ( passWord === 1234 && userName === 'Talvi') {
        logUserIn(false)
    } else {
        logUserIn(true)
    }
  }, [passWord , userName]);

  return (
    <Flex direction='column' justifyContent='center' align='center' height='60%' >
        { !login &&
            <Box
                pos='relative'
                bottom='200'
                w="400px"
                h="300px"
                bg="gray.700"
                p="5"
                borderRadius="25"
                boxShadow="xl"
                maxWidth="700px"
                zIndex="1"
            >
                <Heading>Login</Heading>
            < Input
                width="100%"
                top='10'
                size='sm'
                placeholder='Username'
                value={userName}
                onChange={(e) => setUserName(e.target.value)}/>
            < Input
                width="100%"
                top='43'
                size='sm'
                placeholder='Password'
                value={passWord}
                onChange={(b) => setUserPassword(b.target.value)}/>
            <Button
                top='110'
                right='-280'
                variant='ghost'
                backgroundColor='teal.200'
                onClick={handleSubmit}> Login </Button>
            <Register/>
            </Box>
            }
        { login &&
            <Button
                variant='ghost'
                backgroundColor='teal.200'
                size='sm'
                left='280'
                bottom='520'
                rightIcon={<ArrowRightIcon />}
                onClick={() => logUserIn(false)}> Log Out</Button>
        }
    </Flex>
  )
}

export default Login;
import { useState, useCallback } from 'react'
import { Button, Input, useToast} from '@chakra-ui/react'

const Register = () => {
    const toast = useToast();

    return (
        <>
            <Input
                width="100%"
                size='sm'
                placeholder='Username' /><Input
                width="100%"
                size='sm'
                placeholder='Password' /><Button
                variant='ghost'
                width="100%"
                backgroundColor='teal.200'
                //Turn this into promise-based later
                // https://chakra-ui.com/docs/components/toast
                // Make it send info to database
                onClick={() => toast({
                    title: 'Account Created.',
                    description: 'We have created your account successfully',
                    status: 'success',
                    duration: 2000,
                    isClosable: true,
                })}
            >
                Register
            </Button>
        </>
    )
}

export default Register;
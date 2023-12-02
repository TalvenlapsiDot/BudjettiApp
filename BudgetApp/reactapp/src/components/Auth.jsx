import { useState } from 'react';
import {
  Button,
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalBody,
  VStack,
} from '@chakra-ui/react'

/* Import Login and Register components */
import Login from './Login'
import Register from './Register'

const Auth = ({ open, onLogin, onRegister }) => {
  //React UseState hook that uses boolean (true/false)  and changes it when called (remember to write the state you want to change into on the call)
  // example at row 43
  const [registering, setRegistering] = useState(false)

  return (
    /* Read ChakraUI modal documentation
    https://chakra-ui.com/docs/components/modal */
    <Modal
      isOpen={open}
      isCentered
      p="5"
      borderRadius="25"
      boxShadow="xl"
      zIndex="1"
    >
      <ModalOverlay bg='blackAlpha.400' backdropFilter='blur(2px)' />
      <ModalContent>
        <ModalHeader>{registering ? 'Register' : 'Login'}</ModalHeader>

        <ModalBody>
          {/* VStack is used to stack elements vertically
          Docs: https://chakra-ui.com/docs/components/stack */ }
          <VStack gap={3}>
            {/*Checks the state of Registering, if true shows Register, otherwise shows Login */}
            {registering ? <Register onRegister={onRegister} /> : <Login onLogin={onLogin} />}

            <Button width="100%" onClick={() => setRegistering(!registering)}>
              {registering ? 'Have an account? Login' : 'Need an account? Register'}
            </Button>
          </VStack>
        </ModalBody>
      </ModalContent>
    </Modal>
  )
}

export default Auth
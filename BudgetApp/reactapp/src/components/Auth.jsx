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

import Login from './Login'
import Register from './Register'

const Auth = ({ open, onLogin, onRegister }) => {
  const [registering, setRegistering] = useState(false)

  return (
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
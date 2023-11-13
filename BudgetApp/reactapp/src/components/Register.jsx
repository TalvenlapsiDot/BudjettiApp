import { useState, useCallback } from 'react'
import { Button, Flex, Input, Modal, ModalOverlay, ModalContent, ModalHeader, ModalBody, ModalCloseButton, useToast, useDisclosure} from '@chakra-ui/react'

const Register = () => {
  const {isOpen, onOpen, onClose} = useDisclosure();
  const toast = useToast();

  return (
    <Flex direction='column' justifyContent='center' align='center' height='60%'>
            <Button
            top='3'
            right='120'
            variant='ghost'
            backgroundColor='teal.200'
            onClick={onOpen}
            > Register </Button>

            <Modal
                isOpen={isOpen} onClose={onClose} isCentered
                p="5"
                borderRadius="25"
                boxShadow="xl"
                zIndex="1"
            >
            <ModalOverlay
            bg='blackAlpha.400'
            backdropFilter='blur(2px)'/>
            <ModalContent>
                <ModalHeader>Register</ModalHeader>
                <ModalCloseButton/>
                <ModalBody>
            < Input
                width="100%"
                top='5'
                size='sm'
                placeholder='Username'/>
            < Input
                width="100%"
                top='21'
                size='sm'
                placeholder='Password'/>
            </ModalBody>
            <Button
                top='30px'
                variant='ghost'
                backgroundColor='teal.200'
                //Turn this into promise-based later
                // https://chakra-ui.com/docs/components/toast
                onClick={() => toast({
                    title: 'Account Created.',
                    description: 'We have created your account successfully',
                    status: 'success',
                    duration: 2000,
                    isClosable: true,
                })}
                > Register </Button>
                </ModalContent>
            </Modal>
    </Flex>
  )
}

export default Register;
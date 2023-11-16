import { useState, useCallback } from 'react'
import {Box, Button, Container, Flex, Input, Modal, ModalOverlay, ModalContent, ModalHeader, ModalFooter, ModalBody,
    ModalCloseButton, Table, Thead, Tbody, Tfoot, Tr, Th, Td, TableCaption, TableContainer, useToast, useDisclosure} from '@chakra-ui/react'

const Income = () => {

  const { isOpen, onOpen, onClose } = useDisclosure()
  const toast = useToast();

  return (
    //Maybe Card instead of Container
    //https://chakra-ui.com/docs/components/card
    <>
    <Box bg='gray.500' borderRadius='25' width='85%' height='600'>
    <Button
         left='550'
         variant='ghost'
         size='sm'
         backgroundColor='teal.200'
         onClick={onOpen}> Add Income</Button>
        <TableContainer>
    <Table size='sm'>
    <Thead>
      <Tr>
        <Th>Source</Th>
        <Th>Amount</Th>
      </Tr>
    </Thead>
    <Tbody>
      <Tr>
        <Td>Work</Td>
        <Td>234</Td>
      </Tr>
      <Tr>
        <Td>Sales</Td>
        <Td>54222</Td>
      </Tr>
      <Tr>
        <Td>Rent</Td>
        <Td>546</Td>
      </Tr>
    </Tbody>
  </Table>
</TableContainer>

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
                <ModalHeader>Add Income</ModalHeader>
                <ModalCloseButton/>
                <ModalBody>
            < Input
                width="100%"
                top='5'
                size='sm'
                placeholder='Source'/>
            < Input
                width="100%"
                top='21'
                size='sm'
                placeholder='Amount'/>
            </ModalBody>
            <Button
                top='30px'
                variant='ghost'
                backgroundColor='teal.200'
                //Turn this into promise-based later
                // https://chakra-ui.com/docs/components/toast
                onClick={() => toast({
                    title: 'Added',
                    description: 'Successfully added income',
                    status: 'success',
                    duration: 2000,
                    isClosable: true,
                })}
                > Add </Button>
                </ModalContent>
            </Modal>
     </Box>
    </>
  )
}

export default Income;
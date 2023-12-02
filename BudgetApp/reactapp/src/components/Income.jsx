import { useState, useCallback } from 'react'
import {Box, Button,Input, Modal, ModalOverlay, ModalContent, ModalHeader, ModalBody,
    ModalCloseButton, Table, Thead, Tbody, Tr, Th, Td, TableContainer, useToast, useDisclosure} from '@chakra-ui/react'

const Income = () => {

  const { isOpen, onOpen, onClose } = useDisclosure()
  const toast = useToast();

  return (
    <>
    {/* Chakra UI Box Documentation:
    https://chakra-ui.com/docs/components/box */}
    <Box bg='gray.500' borderRadius='25' width='85%' height='600'>
    <Button
         left='550'
         variant='ghost'
         size='sm'
         backgroundColor='teal.200'
         onClick={onOpen}> Add Income</Button>
        <TableContainer>

    {/* A code sample from another project which populates a Menu-Component with similar ideology to populating this table;
     return (
      <MenuGroup
        key={name}
        title={name}
        color="cyan.400">

        {group.sort(sortNames).map((race) => (
          <MenuItem key={race.Name} onClick={() => setCurrentRace(race)}>
            {race.Name}
          </MenuItem>
        ))}
      </MenuGroup>
    );

    The function needs to map over the entries in database and then generate an entry on the Table for each item mapped.

    Javascript .map function documentation and help:
    https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Map
    https://www.w3schools.com/jsref/jsref_map.asp

    */}
    {/* ChakraUI Table documentation
    https://chakra-ui.com/docs/components/table*/}

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
          {/*This modal doesn't currently send info to database. Make an API call on the button press that sends in information from the inputs.
          Look at the registration function and API call for reference point. */ }
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
                // Move Toast into an API call as a response to successfull API call.
                // Look at Registration function and API call for reference.
                // Make OnClick trigger the API call.
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
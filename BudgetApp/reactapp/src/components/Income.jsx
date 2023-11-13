import { useState, useCallback } from 'react'
import {Box, Button, Container, Flex, Heading, Input, List, ListItem, ListIcon, OrderedList, UnorderedList, } from '@chakra-ui/react'

const Income = () => {

  return (
    //Maybe Card instead of Container
    //https://chakra-ui.com/docs/components/card
    // and maybe Table instead of list
    // https://chakra-ui.com/docs/components/table
    <>
    <Container bg='gray.500' borderRadius='25' centerContent>
        <Heading>Text</Heading>
        <UnorderedList>
            <ListItem>Lorem ipsum dolor sit amet</ListItem>
            <ListItem>Consectetur adipiscing elit</ListItem>
            <ListItem>Integer molestie lorem at massa</ListItem>
            <ListItem>Facilisis in pretium nisl aliquet</ListItem>
        </UnorderedList>
     </Container>
    < Input
         width="40%"
         bottom='-400'
         left=''
         size='md'
         placeholder='Income Source'/>
    < Input
         width="40%"
         bottom='-400'
         left='1'
         size='md'
         placeholder='Amount'/>
      <Button
         bottom='-397'
         left='2'
         variant='ghost'
         backgroundColor='teal.200'> Add Income</Button>
    </>
  )
}

export default Income;
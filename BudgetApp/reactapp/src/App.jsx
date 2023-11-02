import {Box } from '@chakra-ui/react'
import React, { Component } from 'react';



export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
                <Box w="700px" h="500px" bg="teal.400" p="5" borderRadius="25" boxShadow="xl" marginTop="200" maxWidth="700px" zIndex="1" marginLeft="auto" marginRight="auto">
                <h1 id="tabelLabel" >FrontPage Here Later Maybe Idk Man</h1>
                <p>Derp</p>
                </Box>
        );
    }
}

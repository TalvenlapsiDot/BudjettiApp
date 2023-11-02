import {Box } from '@chakra-ui/react'
import React, { Component } from 'react';
import styles from './styles.css'

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
                <Box className={styles.Boxes} w="700px" h="600px" bg="whiteAlpha.500" p="5" borderRadius="25" boxShadow="xl" marginTop="150" maxWidth="700px" zIndex="1" marginLeft="auto" marginRight="auto">
                <h1>FrontPage Here Later Maybe Idk Man</h1>
                <p>Derp</p>
                </Box>
        );
    }
}

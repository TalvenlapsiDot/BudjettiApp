import {Box, Button, Flex} from '@chakra-ui/react'
import { useState } from 'react';

import Dingle from './Dingle';
import Dangle from './Dangle';

import styles from './styles.css'

const App = () => {
    /* React usestate hook to change the status of login or whatever when it's called also translate to Finnish someday*/
    const [authenticated, setAuthenticated] = useState(false);

    return (
        /* Insert here about why the Flex works, tldr direction gives vertical/horizontal
        and height makes it use 100% of the height on screen idk translate to finnish sometime
        some stuff about HTML elements taking only the space they need unless told otherwise */
        <Flex direction='column' justifyContent='center' align='center' height='100%'>
            <Box
                w="700px"
                h="600px"
                bg="whiteAlpha.500"
                p="5"
                borderRadius="25"
                boxShadow="xl"
                maxWidth="700px"
                zIndex="1"
            >
                <h1>FrontPage Here Later Maybe Idk Man</h1>
                <p>Derp</p>

                <Dingle />

                <Dangle />

                <Button onClick={() => setAuthenticated(true)} />

                {/* This is a "short-circui"*/}
                {authenticated && (<div>Hello user!</div>)}
            </Box>
        </Flex>
    );
}

export default App;

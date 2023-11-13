import {Box, Button, Container, Flex, Divider, Tabs, TabList, TabPanels, Tab, TabPanel} from '@chakra-ui/react'
import { useState } from 'react';
/* Remember to import useState you dingus I know you'll forget it in future TALVI.
So when you come to debug it THIS IS WHY IT DOESN'T WORK. */
import Login from './components/Login';
import Income from './components/Income';


const App = () => {
    /* React usestate hook to change the status of login or whatever when it's called also translate to Finnish someday*/
    const [authenticated, setAuthenticated] = useState(false);

    return (
        /* Insert here about why the Flex works, tldr direction gives vertical/horizontal
        and height makes it use 100% of the height on screen idk translate to finnish sometime
        some stuff about HTML elements taking only the space they need unless told otherwise */
        /*Maybe multiple cards instead of tabs, look into it
        https://chakra-ui.com/docs/components/card */
        <Flex direction='column' justifyContent='center' align='center' height='100%'>
            <Box
                zIndex="2"
                w="700px"
                h="700px"
                bg="gray.600"
                p="5"
                borderRadius="25"
                boxShadow="xl"
                maxWidth="700px"
            >
            <Tabs isFitted marginBottom='10' variant='soft-rounded'>
            <TabList background='blackAlpha.300' borderRadius='300' >
              <Tab _hover={{ color: 'white' }}  fontWeight="bold" textColor='teal.300'>Income</Tab>
              <Tab _hover={{ color: 'white' }} fontWeight="bold" textColor='teal.300'>Expenses</Tab>
              <Tab _hover={{ color: 'white' }} fontWeight="bold" textColor='teal.300'>Overview</Tab>
            </TabList>

            <TabPanels>
              <TabPanel>
                <Income />
              </TabPanel>
              <TabPanel>
              <Income />
              </TabPanel>
              <TabPanel>
              <Income />
              </TabPanel>
            </TabPanels>
          </Tabs>
          <Login/>
            </Box>
        </Flex>
    );
}

export default App;

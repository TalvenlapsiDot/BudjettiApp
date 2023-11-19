import {Box, Button, Flex, Tabs, TabList, TabPanels, Tab, TabPanel, useToast} from '@chakra-ui/react'
import { useCallback, useState } from 'react';
/* Remember to import useState you dingus I know you'll forget it in future TALVI.
So when you come to debug it THIS IS WHY IT DOESN'T WORK. */
import {ArrowRightIcon} from '@chakra-ui/icons'

import Auth from './components/Auth';
import Income from './components/Income';

import './styles.css'

const App = () => {
    const toast = useToast();
    /* React usestate hook to change the status of login or whatever when it's called also translate to Finnish someday*/
    const [authenticated, setAuthenticated] = useState(false);

    const handleRegistration = () => {
      // do registration thing in API!
      toast({
          title: 'Registering.',
          description: 'Registering user',
          status: 'success',
          duration: 2000,
          isClosable: true,
      })
    }

    const handleSubmit = useCallback((userName, passWord) => {
      //Turn this into promise-based later
      // https://chakra-ui.com/docs/components/toast
      toast({
          title: 'Logged in.',
          description: 'Logging in',
          status: 'success',
          duration: 2000,
          isClosable: true,
      })

      if ( passWord !== '1234' || userName !== 'Talvi') {
        setAuthenticated(false)
      } else {
        setAuthenticated(true)
      }
    }, [toast]);

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
          <Auth open={!authenticated} onLogin={handleSubmit} onRegister={handleRegistration} />
          <Button
                variant='ghost'
                backgroundColor='teal.200'
                size='xs'
                top='-750'
                left='590'
                rightIcon={<ArrowRightIcon />}
                onClick={() => setAuthenticated(false)}> Log Out</Button>
            </Box>
        </Flex>
    );
}

export default App;

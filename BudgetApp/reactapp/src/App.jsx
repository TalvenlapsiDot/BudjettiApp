import {Box, Button, Flex, Tabs, TabList, TabPanels, Tab, TabPanel, useToast} from '@chakra-ui/react'
import { useState } from 'react';
/* Remember to import useState you dingus I know you'll forget it in future TALVI.
So when you come to debug it THIS IS WHY IT DOESN'T WORK. */
import {ArrowRightIcon} from '@chakra-ui/icons'

/* Imports for components. All components go into /components folder. */
import Auth from './components/Auth';
import Income from './components/Income';

/* Styles.css has only global styles. */
import './styles.css'

const App = () => {
    const toast = useToast();
    /* React usestate hook to change the status of login. When handleLogin returns an OK response, useState changes to True. False means
    the user sees "Login" page. */
    const [authenticated, setAuthenticated] = useState(false);

    const handleRegistration = async(userName, passWord) => {
      console.log(userName, passWord),
      await fetch("https://localhost:7123/API/UserManagement/Register/", {
     method: "POST",
     headers: {
       "Accept": "application/json",
       "Content-Type": "application/json"
     },
     body: JSON.stringify({
       userid: 0,
       username: userName,
       password: passWord,
     }
     ),
     })
     .then((response) => {
       if (!response.ok) {
         toast({
           title: 'Error',
           description: 'Something went wrong',
           status: 'error',
           duration: 2000,
           isClosable: true,
       })
       } else {
         toast({
           title: 'Success',
           description: 'Your account has been created, please login!',
           status: 'success',
           duration: 2000,
           isClosable: true,
       })
       }
     })
   }

    const handleLogin = async(userName, passWord) => {
     await fetch("https://localhost:7123/API/UserManagement/Login/", {
    method: "POST",
    headers: {
      "Accept": "application/json",
      "Content-Type": "application/json"
    },
    body: JSON.stringify({
      userid: 0,
      username: userName,
      password: passWord,
    }
    ),
    })
    .then((response) => {
      if (!response.ok) {
        toast({
          title: 'Error',
          description: 'Something went wrong',
          status: 'error',
          duration: 2000,
          isClosable: true,
      })
      } else {
        toast({
          title: 'Logging in',
          description: 'Logging in successfull',
          status: 'success',
          duration: 2000,
          isClosable: true,
      })
      setAuthenticated(true)
      }
    })
  }

    return (
        /* Insert here about why the Flex works, tldr direction gives vertical/horizontal
        and height makes it use 100% of the height on screen idk translate to finnish sometime
        some stuff about HTML elements taking only the space they need unless told otherwise */
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
          {/* Read ChakraUI documentation of Tabs
          https://chakra-ui.com/docs/components/tabs*/}
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
          <Auth open={!authenticated} onLogin={handleLogin} onRegister={handleRegistration} />
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

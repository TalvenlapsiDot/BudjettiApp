import { ChakraProvider } from '@chakra-ui/react'
import ReactDOM from 'react-dom/client'
import App from './App.jsx'

ReactDOM.createRoot(document.getElementById('root')).render(
  /* Install ChakraUI in the app
  https://chakra-ui.com/docs/components */
  <ChakraProvider>
    <App />
  </ChakraProvider>
)

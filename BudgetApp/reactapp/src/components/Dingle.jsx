import React, { useState } from 'react'

// Modern React Arrow-based Functional Component Export (RAFCE), modern hook based React
// syntax that is documented and standardized in React documentation: https://react.dev/learn
const Dingle = () => {
  const [ counter, setCounter ] = useState(0);

  return (
    <div>
      <button onClick={() => setCounter(counter - 1)}>-</button>
        <div>{`Functional Counter: ${counter}`}</div>
      <button onClick={() => setCounter(counter + 1)}>+</button>
    </div>
  )
}

export default Dingle
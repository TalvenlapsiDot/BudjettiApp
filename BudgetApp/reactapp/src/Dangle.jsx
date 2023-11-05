import React, { Component } from 'react'

/*Delete later */
export default class Dangle extends Component {
  constructor(props) {
    super(props);
    this.state = { counter: 0 };
    this.handleCounterChange = this.handleCounterChange.bind(this);
  }

  handleCounterChange = (change) => {
    this.setState({
      counter: this.state.counter + (change),
    })
  }

  render() {
    return (
      <div>
        <button onClick={() => this.handleCounterChange(-1)}>-</button>
        <div>{`Class Counter: ${this.state.counter}`}</div>
        <button onClick={() => this.handleCounterChange(1)}>+</button>
      </div>
    )
  }
}

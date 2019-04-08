import React from 'react';
import dotnetify from 'dotnetify';
import { Button } from "antd";
import "antd/dist/antd.css";

class HelloWorld extends React.Component {
    constructor(props) {
        super(props);
        this.vm = dotnetify.react.connect("HelloWorld", this);
        // *** Use below to initialize the user's name from the client. *** //
        dotnetify.react.connect("HelloWorld", this, { vmArg: { User: { Name: "Joe" } } });
        this.state = { Greetings: "", ServerTime: "", Data: [], ClickCount: 1 };
        this.dispatchState = state => {
            this.setState(state);
            this.vm.$dispatch(state);
        };
    }
    componentWillUnmount() {
        this.vm.$destroy();
    }

render() {
        return (<div>
            {this.state.Greetings}<br /><br />
            Server time is: {this.state.ServerTime}<br /><br />
            <ul>
                {this.state.Data.map( item => <li key={item.id}>{item.title}</li>)}
            </ul>
            <Button type="button" onClick={_ => this.dispatchState({ ButtonClicked: true })}>Click Me</Button>
            <Button type="button" onClick={_ => this.dispatchState({ OtherButtonClicked: true })}>sfsdf</Button>
            {this.state.ClickCount != 0 && (<span> You clicked me<b> {this.state.ClickCount}</b>&nbsp;times!</span>)}
        </div>);
    }
}
export default HelloWorld;
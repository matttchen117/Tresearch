import axios from "axios";
import React, { useState } from "react";
import Select, { createFilter } from 'react-select';
import jwt_decode from "jwt-decode";  
import "./ChangeNodeContent.css";
import "../JwtVerification/JwtVerification.js"
import verifyToken from "../JwtVerification/JwtVerification.js";
import Button from "../../UI/Button/ButtonComponent.js";

class ChangeNodeContent extends React.PureComponent{
  constructor(props) {
    super(props);

    // Retrieve token
    this.token = sessionStorage.getItem('authorization');

    // State of this class
    this.state = {
      node: props.node,
      title: props.node.nodeTitle,
      summary: props.node.summary,
      isConnected: true,
      errorMessage: '',
    }

    this.inputTitleHandler = this.inputTitleHandler.bind(this);
    this.inputSummaryHandler = this.inputSummaryHandler.bind(this)
  }

    inputTitleHandler = (e) => {
        this.setState({title: e.target.value});
    }   

    inputSummaryHandler = (e) => {
        this.setState({summary: e.target.value});
    }  
  

  // Handle change in internet connection (display message as per BRD) 
  handleStatus = () => {
    const conStatus = navigator.onLine ? 'online' : 'offline';
    this.setState({conStatus: conStatus});
    if(conStatus == 'offline')
        this.setState({errorMessage: 'Internet Connection Lost. Your changes will not be saved.'});
    else{
      this.setState({errorMessage: ''});
    }
  }

  // Runs when page laods
  componentDidMount() {                                      // Retrieve tag data
    this.handleStatus();                                        // Handle internet status
    window.addEventListener('online', this.handleStatus );      // Listen for internet online (refresh)
    window.addEventListener('offline', this.handleStatus);      // Listen for internet offline
    this.state.token = verifyToken();
  }

  // Runs before user leaves component
  componentWillUnmount() {
    this.setState( { })    // Reset state
    window.removeEventListener('online', this.handleStatus );   // Remove listener
    window.removeEventListener('offline', this.handleStatus );  // Remove listener
  }

  handleEncoded = (e) => {
    var parsedData = e.toString();
    if(parsedData.includes('!')){
        parsedData = parsedData.replaceAll('!', '%21');
    }
    if(parsedData.includes('#')){
        parsedData = parsedData.replaceAll('#', '%23');
    }
    if(parsedData.includes('$')){
        parsedData = parsedData.replaceAll('$', '%24');
    }

    // DO NOT DO % SHOULD NOT BE REPLACED
    
    if(parsedData.includes('&')){
        parsedData = parsedData.replaceAll('&', '%26');
    }
    if(parsedData.includes('+')){
        parsedData = parsedData.replaceAll('+', '%2B');
    }
    return parsedData;
}
  // if user logged in for edit, token deleted, now user shouldnt be allowed to do
  // if no frontend validation, reducing user experience, have to wait for backend to do security check
  // should be checking token before navigation, on page load, and tertiary (before executing operation)
  UpdateContent = async() => {
    await axios.post('https://trialbyfiretresearchwebapi.azurewebsites.net/NodeContent/update?owner=' + this.state.node.userHash + '&nodeID=' + this.state.node.nodeID + 
    '&title=' + this.handleEncoded(this.state.title) + '&summary=' + this.handleEncoded(this.state.summary))
    .then(response => {
        console.log(response.data)
        sessionStorage.setItem('authorization', response.headers['authorization']);
    })
    .catch(err => {

    })
}
  // hard coding event, want to programatically assign event to html element 
  // can change how event gets triggered, don't need to modify code if want to change how even is triggered
  // Check Github for how to do - JS registration
  render() {
    return(
        <div className="node-wrapper">
            <div className="node-container">
                <div className="node-info-container">
                    <input className="node-title-container" type="text" value={this.state.title} onChange={this.inputTitleHandler}/>
                    <input className="node-summary-container" type="text" value={this.state.summary} onChange={this.inputSummaryHandler}/>
                </div>  
                <button onClick={this.UpdateContent}>Save</button>        
            </div>
        </div>
    )
  }
}

export default(ChangeNodeContent);
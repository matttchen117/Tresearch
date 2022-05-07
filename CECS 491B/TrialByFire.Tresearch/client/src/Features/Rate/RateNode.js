import axios from "axios";
import React, { useState } from "react";
import Select, { createFilter } from 'react-select';
import Rating from "../../UI/Rating/Rating";
import jwt_decode from "jwt-decode";  
import "./RateNode.css";

class RateNode extends React.PureComponent{
  constructor(props) {
    super(props);

    // Retrieve token
    this.token = sessionStorage.getItem('authorization');

    // State of this class
    this.state = {
      nodes: props.nodes,
      isConnected: true,
      errorMessage: '',
    }
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

  // Check JWT Token
  checkToken = () => {
    const token = sessionStorage.getItem('authorization');
    if(token){
        // Token exists, decode and check credentials
        const decoded = jwt_decode(token);
        const tokenExpiration = decoded.tokenExpiration;
        const now = new Date();

        // Check if expired
        if(now.getTime() > tokenExpiration * 1000){
            localStorage.removeItem('authorization');
            window.location.assign(window.location.origin);
            window.location = '/';
        }
    }else{
        // Token doesn't exist or not valid
        localStorage.removeItem('authorization');
        window.location.assign(window.location.origin);
        window.location = '/';
    }
  }

  // Runs when page laods
  componentDidMount() {                                      // Retrieve tag data
    this.handleStatus();                                        // Handle internet status
    window.addEventListener('online', this.handleStatus );      // Listen for internet online (refresh)
    window.addEventListener('offline', this.handleStatus);      // Listen for internet offline
    this.state.token = this.checkToken();
  }

  // Runs before user leaves component
  componentWillUnmount() {
    this.setState( { })    // Reset state
    window.removeEventListener('online', this.handleStatus );   // Remove listener
    window.removeEventListener('offline', this.handleStatus );  // Remove listener
  }

  SetRating = async (rating) => {
    axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');
    console.log(this.state.nodes);
    await axios.post("https://localhost:7010/Rate/rateNode?rating=" + rating, this.state.nodes)
    .then(response => {
        console.log("rated");
    })
    .catch(err => {
        console.log(err.response);
    })
}
  
  render() {
    const renderSetRatings = (
        <div>
            <p>How would you the node(s)?
                    <Rating rating={0} SetRating={this.SetRating} IsEnabled = {true}/>
            </p>
        </div>
    )
    return(
      <div className="tagger-wrapper">
          <div className = "rate-status-wrapper">
              {this.state.errorMessage}
          </div>
          <div className = "rate-table-wrapper">
              
          </div>
          <div className = "rate-multi-star-wrapper">
            {this.token && (<div>
                            {renderSetRatings}
            </div>)}
          </div>
      </div>
    )
  }
}

export default(RateNode);
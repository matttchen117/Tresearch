import axios from "axios";
import React, { useState } from "react";
import Select, { createFilter } from 'react-select';
import Tag from "../../UI/Tag/Tag";
import jwt_decode from "jwt-decode";  
import "./Tagger.css";

class Tagger extends React.PureComponent{
  constructor(props) {
    super(props);

    // Retrieve token
    this.token = sessionStorage.getItem('authorization');

    // State of this class
    this.state = {
      tagData: [],    
      tags: [], 
      tagOptions: [],
      nodes: props.nodes,
      nullSearch: null,
      isConnected: true,
      errorMessage: ''
    }
  }

  // Retrieve Tags from tag bank
  GetTagData = async () => {

     // Set token header when sending post
     axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');

    // Post request
    await axios.post("https://trialbyfiretresearchwebapi.azurewebsites.net/Tag/nodeTagList", this.state.nodes)
    .then(response => {
      // Data from post
      const responseData = Object.values(response.data);
      // Set state to list of node tags
      this.setState( {tagData: responseData});

      // Retrieve lists of tags
      axios.get("https://trialbyfiretresearchwebapi.azurewebsites.net/Tag/taglist", {})
        .then(response => {
          // Tags returned from post
          const responseData = Object.values(response.data);
          
          // Map resposne data to tag
          const options = responseData.map(d => ({
            "value": d.tagName,
            "label": d.tagName
          }));

          // Set state of tag bank
          this.setState( {tags: options});

          // Remove tags that are used
          let diff = options.filter(x => !this.state.tagData.includes(x.value));

          // Set state of tag options
          this.setState( {tagOptions: diff});

          // Refresh Token
          sessionStorage.setItem('authorization', response.headers['authorization']);
          
        })  
        .catch((err => {
          switch(err.response.status){
            case 400: 
                break;
            case 401:          
                  // Not enabled/confirmed  (account disabled)                
                  localStorage.removeItem('authorization');
                  window.location.assign(window.location.origin);
                  window.location = '/';
                break;
            case 403:
                  // Not authorized to to retrieve tags
                  window.location = '/Portal';
              break;
            case 503: 
                    // Server cannot make database connection
                    this.setState({errorMessage: 'Cannot connect to database.'});
                break;
            default: 
              this.setState({errorMessage: 'Unable to retrieve tags.'});
          }
        }))

        // Refresh Token
        sessionStorage.setItem('authorization', response.headers['authorization']);
    })
    .catch((err => {
      switch(err.response.status) {
        case 400:
          this.setState({errorMessage: 'Unable to make server request'});
          break;
        case 401:
          // Not authorized to view/make changes to node
          window.location = '/Portal';
          break;
        case 503:
          // Cannot make database connection
          this.setState({errorMessage: 'Cannot connect to database.'});
          break;
        default: 
          this.setState({errorMessage: 'Unable to load node tags.'});
      }
    }))
  }

  // Handle change in internet connection (display message as per BRD) 
  handleStatus = () => {
    const conStatus = navigator.onLine ? 'online' : 'offline';
    this.setState({conStatus: conStatus});
    if(conStatus == 'offline')
        this.setState({errorMessage: 'Internet Connection Lost. Your changes will not be saved.'});
    else{
      this.GetTagData();
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
  componentDidMount() {
    this.checkToken();
    this.GetTagData();                                          // Retrieve tag data
    this.handleStatus();                                        // Handle internet status
    window.addEventListener('online', this.handleStatus );      // Listen for internet online (refresh)
    window.addEventListener('offline', this.handleStatus);      // Listen for internet offline
    console.log(this.state.nodes);
  }

  // Runs before user leaves component
  componentWillUnmount() {
    this.setState( { tagData: [], tags: [], tagOptions: []})    // Reset state
    window.removeEventListener('online', this.handleStatus );   // Remove listener
    window.removeEventListener('offline', this.handleStatus );  // Remove listener
  }

  // Handle Add
  handleSelection = (e) => {
    // Check Token
    this.checkToken();

    var value = e.value;

    // Set token header when sending post
    axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');

    // Add tag from node(s)
    axios.post("https://trialbyfiretresearchwebapi.azurewebsites.net/Tag/addTag?tagName="+this.handleEncoded(value) ,this.state.nodes)
        .then((response => {
          this.setState( previousState => ({
            // Add tags to list of node tags
            tagData: [...previousState.tagData, value],
            // Filter tag out of list
            tagOptions: previousState.tagOptions.filter(item => item.value !== value ), 
          })); 
          
          // Refresh Token
          sessionStorage.setItem('authorization', response.headers['authorization']);     
        }))
        .catch((err => {
            
        }))  
  }

  // Handle refresh of tag options
  handleSearchRefresh = (e) => {
    let diff = this.state.tags.filter(x => !this.state.tagData.includes(x.value));
    this.setState( {tagOptions: diff});
  }

  // Handle tag remove
  handleClick = (e) => {

    // Check Token
    this.checkToken();

    // Retrieve value of tag to remove
    var value = e.target.getAttribute('data-item');

    // Set token header when sending post
    axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');

    // Delete tag from node(s)
    axios.post("https://trialbyfiretresearchwebapi.azurewebsites.net/Tag/removeTag?tagName="+this.handleEncoded(value), this.state.nodes)
        .then(response => {
            this.setState( previousState => ({
                tagData: previousState.tagData.filter(item => item !== value ), 
                tagOptions: [...previousState.tagOptions, {"value": value, "label": value}],
            }));
            this.handleSearchRefresh();

            // Refresh Token
            sessionStorage.setItem('authorization', response.headers['authorization']);
        })
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
  
  render() {
    // Render node tags
    const renderTags = (
      <div className="tagger-container">
          <p>Tags:</p>
          <ul>
            {this.state.tagData.sort().map(item => 
                <span key={item} onClick = { this.handleClick } ><Tag name = {item}/></span>
              )}
          </ul>
      </div>
    );

    // Render search bar for tag options
    const renderSearchBar = (
      <div className = "tagger-searchBar-container">
        <Select 
          options = {this.state.tagOptions}  
          onChange = {this.handleSelection} 
          value = {this.state.nullSearch} 
          filterOption = {createFilter( {ignoreAccents: false})} 
        />
      </div>
    )

    return(
      <div className="tagger-wrapper">
          <div className = "tagger-status-wrapper">
              {this.state.errorMessage}
          </div>
          <div className = "tagger-table-wrapper">
              {renderTags}
          </div>
          <div className = "tagger-search-form-wrapper">
              {renderSearchBar}
          </div>
      </div>
    )
  }
}

export default(Tagger);
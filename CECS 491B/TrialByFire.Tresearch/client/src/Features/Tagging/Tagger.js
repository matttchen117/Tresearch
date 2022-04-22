import axios from "axios";
import React from "react";
import Select, { createFilter } from 'react-select';
import Tag from "../../UI/Tag/Tag";
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
      nullSearch: null
    }
  }

  // Retrieve Tags from tag bank
  GetTagData = async () => {

     // Set token header when sending post
     axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');

    // Post request
    await axios.post("https://localhost:7010/Tag/nodeTagList", this.state.nodes)
    .then(response => {
      // Data from post
      const responseData = Object.values(response.data);
      // Set state to list of node tags
      this.setState( {tagData: responseData});

      // Retrieve lists of tags
      axios.get("https://localhost:7010/Tag/taglist", {})
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
                break;
            default: 
          }
        }))
    })
  }

  componentDidMount() {
    this.GetTagData();          // Get initial tag data
  }

  componentWillUnmount() {
    this.setState( { tagData: [], tags: [], tagOptions: []})
  }

  // Handle Add
  handleSelection = (e) => {
    var value = e.value;

    // Set token header when sending post
    axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');

    // Add tag from node(s)
    axios.post("https://localhost:7010/Tag/addTag?tagName="+value ,this.state.nodes)
        .then((response => {
          this.setState( previousState => ({
            // Add tags to list of node tags
            tagData: [...previousState.tagData, value],
            // Filter tag out of list
            tagOptions: previousState.tagOptions.filter(item => item.value !== value ), 
          }));
        
        }))
        .catch((err => {
            console.log(err);
        }))
  }

  handleSearchRefresh = (e_ => {
    let diff = this.state.tags.filter(x => !this.state.tagData.includes(x.value));
    this.setState( {tagOptions: diff});
  })

  // Handle tag remove
  handleClick = (e) => {
    // Retrieve value of tag to remove
    var value = e.target.getAttribute('data-item');

    // Set token header when sending post
    axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');

    // Delete tag from node(s)
    axios.post("https://localhost:7010/Tag/removeTag?tagName="+value, this.state.nodes)
        .then(response => {
            this.setState( previousState => ({
                tagData: previousState.tagData.filter(item => item !== value ), 
                tagOptions: [...previousState.tagOptions, {"value": value, "label": value}],
            }));
            this.handleSearchRefresh();
        })
  }
  
  render() {
    // Render node tags
    const renderTags = (
      <div className="tagger-container">
          <p>Tags:</p>
          <ul>
            {this.state.tagData.map(item => 
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
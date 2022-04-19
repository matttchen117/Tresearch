import axios from "axios";
import React, {useState, useEffect } from "react";
import Select, { createFilter } from 'react-select';
import { useParams} from "react-router-dom";
import Tag from "../../UI/Tag/Tag";
import "./Tagger.css";

class Tagger extends React.PureComponent{
  constructor(props) {
    super(props);
    this.token = sessionStorage.getItem('authorization');

    this.state = {
      tagData: [],
      tags: [], 
      tagOptions: [],
      nodes: props.nodes,
      nullSearch: null
    }
  }

  GetTagData = async () => {
    await axios.post("https://localhost:7010/Tag/nodeTagList", this.state.nodes)
    .then(response => {
      const responseData = Object.values(response.data);
      this.setState( {tagData: responseData});

      axios.get("https://localhost:7010/Tag/taglist", {})
        .then(response => {
          const responseData = Object.values(response.data);
          
          const options = responseData.map(d => ({
            "value": d.tagName,
            "label": d.tagName
          }));
          this.setState( {tags: options});
          let diff = options.filter(x => !this.state.tagData.includes(x.value));
          this.setState( {tagOptions: diff});
        })    
    })
  }

  componentDidMount() {
    this.GetTagData();          // Get initial tag data

  }

  componentWillUnmount() {
    
  }

  handleSelection = (e) => {
    var value = e.value;
    axios.post("https://localhost:7010/Tag/addTag?tagName="+value ,this.state.nodes)
        .then((response => {
          this.setState( previousState => ({
            tagData: [...previousState.tagData, value],
            tagOptions: previousState.tagOptions.filter(item => item.value != value ), 
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

  handleClick = (e) => {
    var value = e.target.getAttribute('data-item');
    axios.post("https://localhost:7010/Tag/removeTag?tagName="+value, this.state.nodes)
        .then(response => {
            this.setState( previousState => ({
                tagData: previousState.tagData.filter(item => item != value ), 
                tagOptions: [...previousState.tagOptions, {"value": value, "label": value}],
            }));
            this.handleSearchRefresh();
        })
  }
  

  render() {
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

    const renderSearchBar = (
      <div className = "tagger-searchBar-container">
        <Select options = {this.state.tagOptions}  onChange = {this.handleSelection} value = {this.state.nullSearch} filterOption = {createFilter( {ignoreAccents: false})}/>
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
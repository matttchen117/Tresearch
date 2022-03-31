import axios from "axios";
import React, {useState, useEffect } from "react";
import Select from 'react-select';

import Tag from "../../UI/Tag/Tag";
import "./Tagger.css";

function Tagger() {
  // Holds array of tags that node currently has tagged
  const [tagData, setTagData] = useState([]);
  //Holds array of tags user can add to their current node (does not inlcude tags in tagData)
  const [tagOptions, setTagOptions] = useState([]);
  //Holds null value, when an item is selected, we want search bar to not have previously selected item highlighted
  const nullSearch = null;  

  //Array of nodes this views context
  const nodeData = [1,2];

  /**
   * Fetches tag(s) that the current node has tagged
   */
  const fetchNodeTags = () => {
    async function fetchData() {
        // Get array of tags node currently has tagged
        const request = await axios.post("https://localhost:7010/Tag/nodeTagList", nodeData );
        //Set tag array
        setTagData(request.data);
    }
    fetchData();
    return tagData;
  }

  const fetchTagOptions = () => {
    async function fetchData() {
      const res = await axios.get("https://localhost:7010/Tag/taglist")
      const resData = res.data;
      let diff = await resData.filter(x => !tagData.includes(x));
      const options = diff.map(d=> ({
        "value": d,
        "label": d
      }))
      setTagOptions(options);
    }
    fetchData();
    return tagOptions;
  }

  const handleSelection = (e) =>{
    const value = e.value;
    axios.post("https://localhost:7010/Tag/addTag?tagName="+value ,nodeData)
        .then((response => {
          fetchNodeTags();
          fetchTagOptions();
        }))
        .catch((err => {
            console.log(err);
        }))
  }

  const handleClick = (e) => {
    var value = e.target.getAttribute('data-item');
    axios.post("https://localhost:7010/Tag/removeTag?tagName="+value ,nodeData)
        .then((response => {
          fetchNodeTags();
          fetchTagOptions();
        }))
        .catch((err => {
            console.log(err);
        }))
  }

  useEffect(() => {
      fetchNodeTags();
      fetchTagOptions();
      //Refresh after every 3 seconds
      const interval = setInterval(() => {
        fetchNodeTags();                   //post data and get list of strings to make tags
        fetchTagOptions();
      }, 1000);
      return () => clearInterval(interval);
  }, [tagData], [tagOptions]) //tracks if state has been changed or not

  const renderTags = (
    <div className="tagger-container">
        <p>Tags:</p>
        <ul>
          {tagData.map(item => 
              <span key={item} onClick = {handleClick}><Tag name = {item}/></span>
            )}
        </ul>
    </div>
  );

  const renderSearchBar = (
    <div className = "tagger-searchBar-container">
      <Select options = {tagOptions} onChange = {handleSelection} value = {nullSearch}/>
    </div>
  )
  
  return (
    <div className="tagger-wrapper">
        <div className = "tagger-table-wrapper">
            {renderTags}
        </div>
        <div className = "tagger-search-form-wraper">
            {renderSearchBar}
        </div>
    </div>
  );
}
  
  export default Tagger;
  
import axios from "axios";
import React, {useState, useEffect } from "react";
import Select from 'react-select';
import { useParams} from "react-router-dom";

import Tag from "../../UI/Tag/Tag";
import "./Tagger.css";

function Tagger() {
  // Holds array of tags that node currently has tagged
  const [tagData, setTagData] = useState([]);
  //Holds array of tags user can add to their current node (does not inlcude tags in tagData)
  const [tagOptions, setTagOptions] = useState([]);
  //Holds null value, when an item is selected, we want search bar to not have previously selected item highlighted
  const nullSearch = useState([]);  

  //Array of nodes this views context
  const nodeData = [1];

  

  const handleSelection = (e) =>{
    
  }

  const handleClick = (e) => {
    
  }



  const fetchNodeTags = () => {
    async function fetchData() {
      await axios.post("https://localhost:7010/Tag/nodeTagList", nodeData)
        .then(response => {
          const responseData = response.data;
          setTagData(responseData);
        })
        .catch(err => {
          switch(err.response.data){
            case 403: {
                setTagData(nullSearch);
                console.log('test');
            }
          }
        })
    }
    fetchData();
  }

  const fetchTagOptions = () => {
    
    async function fetchData() {
        await axios.get("https://localhost:7010/Tag/taglist")
        .then(response => {
          const responseData = response.data;
          const options = responseData.map(d => ({
            "value": d.tagName,
            "label": d.tagName
          }));
          setTagOptions(options);
        })
    }
    fetchData();
  }

  const refresh  = (
    
    useEffect(() => {
      axios.defaults.headers.common['Authorization'] = localStorage.getItem('authorization');
      fetchNodeTags();
      fetchTagOptions();
      const intervalRefresh = setInterval(() => {
        fetchNodeTags();
        fetchTagOptions();
      }, 5000)

      return() => {
        clearInterval(intervalRefresh);
      }
      
    }, [])
  )

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
  
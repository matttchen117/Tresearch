import axios from "axios";
import React, {useState, useEffect } from "react";
import Select from 'react-select';
import { useParams} from "react-router-dom";

import Tag from "../../UI/Tag/Tag";
import "./Tagger.css";

function Tagger(nodes) {
  // Holds array of tags that node currently has tagged
  const [tagData, setTagData] = useState([]);
  //Holds array of tags user can add to their current node (does not inlcude tags in tagData)
  const [tagOptions, setTagOptions] = useState([]);
  //Holds null value, when an item is selected, we want search bar to not have previously selected item highlighted
  const nullSearch = useState([]);  

  //Array of nodes this views context

  const nodeD = nodes.nodes;

  const handleSelection = (e) =>{
    var value = e.value;
    axios.post("https://localhost:7010/Tag/addTag?tagName="+value ,nodeD)
        .then((response => {
          refreshTagData();
        }))
        .catch((err => {
            console.log(err);
        }))
  }

  const handleClick = (e) => {
    async function fetchData() {
      var value = e.target.getAttribute('data-item');
      await axios.post("https://localhost:7010/Tag/removeTag?tagName="+value, nodeD)
        .then(response => {
            refreshTagData();
        })
        .catch(err => {
          switch(err.response){
            case 403: {
                setTagData(nullSearch);    
            }
          }
        })
    }
    fetchData();
  }

  const refreshTagData = () => {
    axios.post("https://localhost:7010/Tag/nodeTagList", nodeD)
    .then(response => {
      const responseData = response.data;
      setTagData(responseData);
    })
    axios.get("https://localhost:7010/Tag/taglist", {})
        .then(response => {
          const responseData = response.data;
          const options = responseData.map(d => ({
            "value": d.tagName,
            "label": d.tagName
          }));
          let diff = options.filter(x => !tagData.includes(x.value));
          setTagOptions(diff);
    })
  }

  const refresh  = (
    useEffect(() => {
      axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');
     
      refreshTagData(); 
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
  
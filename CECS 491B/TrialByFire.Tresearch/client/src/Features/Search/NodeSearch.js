import axios from "axios";
import React, {useState, useEffect } from "react";
import Select from 'react-select';
import { useParams} from "react-router-dom";
import NavBar from "../../UI/Navigation/NavBar";
import "./NodeSearch.css";
import userEvent from "@testing-library/user-event";
import jwtVerification from "../JwtVerification/JwtVerification";

function Search() {
    // Holds array of nodes result
    const [nodeData, setNodeData] = useState([]);
    // Holds array of tags selected for the filter
    const [tagData, setTagData] = useState([]);
    //Holds array of tags user can add to the current search filter
    const [tagOptions, setTagOptions] = useState([]);
    //Holds null value, when an item is selected, we want search bar to not have previously selected item highlighted
    const nullSearch = useState([]);  

    const [query, setQuery] = useState("")
    const [filterByRating, setFilterByRating] = useState(false)
    const [filterByTime, setFilterByTime] = useState(false)
    const [reverseList, setReverseList] = useState(false)

    const handleChangeFilterByRating = () => {
        setFilterByRating(!filterByRating);
    };

    const handleChangeFilterByTime = () => {
        setFilterByTime(!filterByTime);
    };

    const handleReverseList = () => {
        setReverseList(!reverseList);
    };

    const handleEncoded = (e) => {
        var parsedData = e.toString();
        if(parsedData.includes(' ')){
            parsedData = parsedData.replaceAll(' ', '%20');
        }
        if(parsedData.includes('!')){
            parsedData = parsedData.replaceAll('!', '%21');
        }
        if(parsedData.includes('#')){
            parsedData = parsedData.replaceAll('#', '%23');
        }
        if(parsedData.includes('$')){
            parsedData = parsedData.replaceAll('$', '%24');
        }
        if(parsedData.includes('&')){
            parsedData = parsedData.replaceAll('&', '%26');
        }
        if(parsedData.includes('+')){
            parsedData = parsedData.replaceAll('+', '%2B');
        }
        return parsedData;
    }

    const handleSubmit = event => {
        event.preventDefault();
        //axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');
        var parsedData = handleEncoded(query);
        var request = "https://trialbyfiretresearchwebapi.azurewebsites.net/NodeSearch/search?search=" + parsedData + "&filterByRating=" + filterByRating + 
        "&filterByTime=" + filterByTime;
        for(var i=0; i<tagData.length; i++)
        {
            request += "&tags=" + tagData[i].tagName;
        }
        async function fetchData()
        {
            const response = await axios.get(request);
            if(response !== null && response !== undefined)
            {
                setNodeData(response.data);
            }else{
                setNodeData(nullSearch);
            }
        }
        fetchData()
    };

    const fetchTableData = () => {
        async function fetchData() {
            const response = await axios.get("https://trialbyfiretresearchwebapi.azurewebsites.net/Tag/taglist");
            if(response !== null && response !== undefined)
            {
                setTagOptions(Object.values(response.data));
            }else{
                setTagOptions(nullSearch);
            }
        }
        fetchData();
    }

    useEffect(() => {
        fetchTableData()
    }, []);

    useEffect(() => {
        const copy = [...nodeData]
        for(var i=0; i<copy.length; i++)
        {
            var node = copy[i]
            var numMatches = node.tags.filter(t => tagData.some(td => td.tagName === t.tagName)).length
            node.tagScore = numMatches / (tagData.length + node.tags.length - numMatches)
        }
        if(filterByTime && filterByRating)
        {
            const sorted = [...nodeData].sort((a,b) => {
                // if timeModified are the same, check if ratingScores are the same, otherwise order by timeModified
                // if rating scores are the same, check if exactMatch are the same, otherwise order by ratingScore
                // if exactMatch are the same, order by tagScore, otherwise order by exactMatch
                var dateA = new Date(a.timeModified)
                var dateB = new Date(b.timeModified)
                return dateA - dateB || a.ratingScore - b.ratingScore || a.exactMatch - b.exactMatch || a.tagScore - b.tagScore; 
            });
            setNodeData(sorted.reverse())
        }else if(filterByTime){
            const sorted = [...nodeData].sort((a,b) => {
                // if timeModified are the same, check if exactMatch are the same, otherwise order by timeModified
                // if exactMatch are the same, order by tagScore, otherwise order by exactMatch
                var dateA = new Date(a.timeModified)
                var dateB = new Date(b.timeModified)
                return dateA - dateB || a.exactMatch - b.exactMatch || a.tagScore - b.tagScore  
            });
            setNodeData(sorted.reverse())
        }else if(filterByRating)
        {
            const sorted = [...nodeData].sort((a,b) => {
                // if ratingScore are the same, check if exactMatch are the same, otherwise order by ratingScore
                // if exactMatch are the same, order by tagScore, otherwise order by exactMatch
                return a.ratingScore - b.ratingScore || a.exactMatch - b.exactMatch || a.tagScore - b.tagScore
            });
            setNodeData(sorted.reverse())
        }else{
            const sorted = [...nodeData].sort((a,b) => {
                // if exactMatch are the same, order by tagScore, otherwise order by exactMatch
                return a.exactMatch - b.exactMatch || a.tagScore - b.tagScore
            });
            setNodeData(sorted.reverse())
        }
    }, [filterByRating, filterByTime, tagData]);

    const Checkbox = ({ label, value, onChange }) => {
        return (
            <label>
                <input type="checkbox" checked={value} onChange={onChange}/>
                {label}
            </label>
        );
    };

    function getIndex(array, value, prop) {
        for(var i = 0; i < array.length; i++) {
            if(array[i][prop] === value) {
                return i;
            }
        }
        return -1;
    }

    //add tag to selected tags, remove tag from options
    const selectTag = (e) => {
        if(e.target.getAttribute("value") !== null)
        {
            var tags = [...tagOptions];
            var name = e.target.getAttribute("value");
            var index = getIndex(tags, name, 'tagName');
            var tag = tags[index];
            if(!(tagData.some(tag => tag.tagName === name)))
            {
                setTagData([...tagData, tag])
            }
            const options = tags.filter(function(tag) {
                return tag.tagName !== e.target.getAttribute("value")
            })
            setTagOptions(options)
        }
    }

    //remove tag from selected tags, readd tag to options
    const removeTag = (e) => {
        if(e.target.getAttribute("value") !== null)
        {
            var tags = [...tagData];
            var tagName = e.target.getAttribute("value");
            var index = getIndex(tags, tagName, "tagName");
            var tag = tags[index]
            if(!(tagOptions.some(tag => tag.tagName === tagName)))
            {
                setTagOptions([...tagOptions, tag])
            }
            const data = tags.filter(function(tag) {
                return tag.tagName !== e.target.getAttribute("value")
            })
            setTagData(data)
        }
    }

    const getTags = (item) => {
        let text = ""
        for(var i=0; i<item.tags.length; i++)
        {
            text += item.tags[i].tagName + " "
        }
        return text
    }

    const movePage = (user) =>{
        window.location = '/SearchPage?page=' + user;
    }

    const renderAvailableTags = (
        <div className="available-tags-container">
            <p className="tags-header">Available Tags:</p>
            <ol>
              {tagOptions.map(item => 
                  <button key={item.tagName} value={item.tagName} onClick={selectTag}>{item.tagName}</button>
                )}
            </ol>
        </div>
    );

    const renderSelectedTags = (
        <div className="selected-tags-container">
            <p className="tags-header">Selected Tags:</p>
            <ol>
              {tagData.map(item => 
                  <button key={item.tagName} value={item.tagName} onClick={removeTag}>{item.tagName}</button>
                )}
            </ol>
        </div>
    );

    const renderTable = (
        <div className="node-search-table-container">
            <table className = "node-search-table">
                <thead className = "node-search-table-thead">
                    <tr>
                        <th>Results</th>
                    </tr>
                </thead>
                {reverseList ? 
                    <tbody>
                        {nodeData.map(item =>{
                            return(
                                <tr key={item.nodeID} onClick={() => movePage(item.userHash)} className = "row-search-table">
                                        <td className = "node-table-title" data-item = {item.nodeTitle} >{item.nodeTitle}</td>
                                        <td>{getTags(item)}</td>
                                        <td className = "node-table-time-modified" data-item = {new Date(item.timeModified)} >{item.timeModified}</td>
                                        <td className = "node-table-rating" data-item = {item.ratingScore} >{item.ratingScore}</td>
                                </tr>
                            );
                        }).reverse()}
                    </tbody>
                    :
                    <tbody>
                        {nodeData.map(item =>{
                            return(
                                <tr key={item.nodeID} onClick={() => movePage(item.userHash)} className = "row-node-table">
                                        <td className = "node-table-title" data-item = {item.nodeTitle} >{item.nodeTitle}</td>
                                        <td>{getTags(item)}</td>
                                        <td className = "node-table-time-modified" data-item = {item.timeModified} >{item.timeModified}</td>
                                        <td className = "node-table-rating" data-item = {item.ratingScore} >{item.ratingScore}</td>
                                </tr>
                            );
                        })}
                    </tbody>
                }
            </table>
        </div>
    );

    return (
        <div>
            <div className="form-search-container">
                {<NavBar/>}
                <form onSubmit = {handleSubmit}>
                    <div className="search-bar-container">
                        <input type="text" value={query} placeholder="Search" onChange = {event => setQuery(event.target.value)}/>
                    </div>
                    <div className="search-filter-container">
                    <Checkbox
                            label = "Filter By Time"
                            value = {filterByTime}
                            onChange={handleChangeFilterByTime}
                        />
                        <Checkbox
                            label = "Filter By Rating"
                            value = {filterByRating}
                            onChange={handleChangeFilterByRating}
                        />
                        <button value={reverseList} onClick={handleReverseList}>Reverse Result List</button>
                    </div>
                </form>
                <div>
                    {renderTable}
                    {renderSelectedTags}
                    {renderAvailableTags}
                </div>
            </div>
        </div>
    );
}
  
export default Search;
  
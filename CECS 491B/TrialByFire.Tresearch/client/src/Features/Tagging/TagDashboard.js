import axios from "axios";                                  // Make post requests to backend
import React, {useState, useEffect } from "react";          // Usestate to initialize data on load, useEffect run on load and every 10s
import NavBar from "../../UI/Navigation/NavBar";            // Navigation Bar
import Button from "../../UI/Button/ButtonComponent";       // Custom Button Component
import jwt_decode from "jwt-decode";                        // Check Token expiration, every 10s, before changes made

import "./TagDashboard.css";

function TagDashboard() {
    // State holding tags retrieved from backend
    const [tagData, setTagData] = useState([]);

    // State holding create tag input field data
    const [createData, setCreateData] = useState('');
    
    // State holding message and error status
    const [alertData, setAlertData] = useState(
        {
            message: '',
            isError: false
        }
    )

    //Axios doesn't allow certain special characters. This will encode special characters 
    const  handleEncoded = (e) => {
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

    // Handle deleting tag
    const handleClick = (e) => {
        
        // Verify credentials
        checkToken();

        // Get tag name to delete
        var value = e.target.getAttribute('data-item');
        // Encode data if there are special characters
        var parsedData = handleEncoded(value);

        // Set token header when sending post
        axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');

        // Delete tag
        axios.post("https://localhost:7010/Tag/deleteTag?tagName=" + parsedData)
        .then((response => {
            fetchTableData();
        }))
        .catch((err => {
            switch(err.response.status) {
                case 400: 
                    // Returns when request is not correct (or when empty string)
                    setAlertData({message: 'Cannot make request to server.'});
                    break;
                case 401: 
                    // Not authorized
                    localStorage.removeItem('authorization');
                    window.location.assign(window.location.origin);
                    window.location = '/';
                    break;
                case 422:
                    // Invalid tag name (null, empty, only whitespace)
                    setAlertData({message: 'Invalid tag name.'});
                case 503: 
                    // Server cannot make database connection
                    setAlertData({message: 'Unable to connect to database. Try again.'});
                    break;
                default:
                    setAlertData({message: 'Unable to delete tag.'});
            }
        }))
    }

    // Bind state to input field
    const handleChange = (e) => {
        setAlertData({message: ''});
        setCreateData(e.target.value);
    }
    
    // Fetch tag bank from server
    const fetchTableData = () => {
        async function fetchData() {
            // Verify credentials
            checkToken();

            // Set token header when sending post
            axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');

            // Retrieve tag list
            await axios.get("https://localhost:7010/Tag/taglist")
            .then((response => {
                setTagData(response.data); 
                return true;
            }))
            .catch((err => {
                switch(err.response.status){
                    case 400: 
                    // Returns when request is not correct (or when empty string)
                    setAlertData({message: 'Cannot make request to server.'});
                        break;
                    case 401:          
                            // Not authorized  (account disabled)                
                            localStorage.removeItem('authorization');
                            window.location.assign(window.location.origin);
                            window.location = '/';
                        break;
                    case 503: 
                            // Server cannot make database connection
                            setAlertData({message: 'Unable to connect to database. Try again.'});
                        break;
                    default: 
                        setAlertData({message: 'Unable to retrieve tags.'});
                }
                return false;
            }))                  
        }
        fetchData();
    }

    // Check JWT Token
    const checkToken = () => {
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

            // Check Role
            if(decoded.authorizationLevel !== 'admin'){
                window.location.assign(window.location.origin);
                window.location = '/Error404';
            }
        }else{
            // Token doesn't exist or not valid
            localStorage.removeItem('authorization');
            window.location.assign(window.location.origin);
            window.location = '/';
        }
    }

    // Refresh data and check token every 10s
    useEffect(() => {
        checkToken();
        fetchTableData();
        //Refresh after every 10 seconds
        const interval = setInterval(() => {
            checkToken();
            fetchTableData();   
        }, 10000);
        return () => clearInterval(interval);
    }, [])


    // Create tag
    const createTag = (e) => {
        e.preventDefault();

        // Check credentials
        checkToken();

        // Encode data if there are special characters
        var parsedData = handleEncoded(createData);
        
        // Set token headers before post
        axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');
        
        axios.post("https://localhost:7010/Tag/createTag?tagName=" + parsedData)
        .then((response => {

            // Reset input field
            setCreateData('');
            setAlertData({message: 'Added'});
            fetchTableData();

            // Remove success message after 1s
            setTimeout(() => {
                setAlertData({message: ''});
            }, 1000)
            
        }))
        .catch((err => {
            switch(err.response.status){
                case 400: 
                    // Cannot post request
                    setAlertData({message: 'Cannot make request.'});
                    break;
                case 409: 
                    // Tag already exists in database
                    setAlertData({message: 'Tag already exists'});
                    break;
                case 401: 
                        // User is not authorized to perform
                        console.log("Not Authorized");
                        localStorage.removeItem('authorization');
                        window.location.assign(window.location.origin);
                        window.location = '/';
                    break;
                case 503: 
                        // Server cannot connect to database
                        console.log("Database offline");
                        setAlertData({message: 'Database offline'});
                    break;
                default: 
                    setAlertData({message: 'Unable to create tag'});
            }
        }))
    }

    // Render table
    const renderTable = (
        <div className="tag-dashboard-table-container">
            <table className = "tag-dashboard-table">
                <thead className = "tag-dashboard-table-thead">
                    <tr>
                        <th>Tags</th>
                    </tr>
                </thead>
                <tbody>
                    {tagData.map(item =>{
                        return(
                            <tr key={item.tagName} className = "row-tag-table">                          
                                        <td className = "tag-table-name" data-item = {item.tagName} ><span className = "delete-icon-tag-table" data-item = {item.tagName} onClick = {handleClick}> &#10005;&emsp;</span>{item.tagName}</td>
                                        <td className = "tag-table-count" data-item = {item.tagName} >{item.tagCount}</td>
                            </tr>
                        );
                    })}
                </tbody>
            </table>
        </div>
    );

    // Render create tag input form
    const renderForm = (
        <div className = "tag-dashboard-form-container">
            <form className="tag-dashboard-create-form" onSubmit = {createTag}>
                    <div className="tag-dashboard-create-input-container">
                        <input type="text" value={createData} required placeholder="Tag Name" onChange = {handleChange}/>
                    </div>
                    <div className = "tag-dashboard-create-button">
                        <Button type="button" color="green" name="Create"/>
                    </div> 
                    <div className="create-tag-error"> {alertData.message} </div>
                </form>
        </div>
    );

    // Render return link to Admin Dashboard
    const renderBack = (
        <div className = "tag-dashboard-back-container">
            <a href="/Admin/Dashboard">&lt;&emsp;Return to Portal</a>
        </div>
    );

    return (       
        <div className="tag-dashboard-wrapper">
            <div className = "tag-dashboard-nav-wrapper">
                {<NavBar/>}
            </div>
            <div className = "tag-dashboard-back-wrapper">
                {renderBack}
            </div>
            <div className = "tag-dashboard-table-wrapper">
                {renderTable}
            </div>
            <div className = "tag-dashboard-form-wrapper">
                {renderForm}
            </div>
        </div>
    )
}

export default TagDashboard;
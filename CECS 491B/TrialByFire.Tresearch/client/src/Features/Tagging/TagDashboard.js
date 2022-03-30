import axios from "axios";
import React, {useState, useEffect } from "react";

import "./TagDashboard.css";
import Button from "../../UI/Button/ButtonComponent";

function TagDashboard() {
    const [data, setData] = useState([]);

    const [createData, setCreateData] = useState(
        {
            tagName: ''
        }
    )

    const [alertData, setAlertData] = useState(
        {
            message: ''
        }
    )

    const handleClick = (e) => {
        var value = e.target.getAttribute('data-item');
        axios.post("https://localhost:7010/Tag/deleteTag?tagName=" + value)
        .then((response => {
            fetchTableData();
        }))
        .catch((err => {
            console.log(err);
        }))
    }

    const handleChange = (e) => {
        setAlertData({message: ''});
        setCreateData(e.target);
    }
     
    const fetchTableData = () => {
        async function fetchData() {
            const request = await axios.get("https://localhost:7010/Tag/taglist");
            const responseData = await request.data;
            setData(responseData);
        }
        fetchData();
    }

    useEffect(() => {
        fetchTableData();
        //Refresh after every 3 seconds
        const interval = setInterval(() => {
            fetchTableData();
        }, 3000);
        return () => clearInterval(interval);
    }, [])

    const createTag = (e) => {
        e.preventDefault();
        console.log(createData);
        axios.post("https://localhost:7010/Tag/createTag?tagName=" + createData)
        .then((response => {
            setCreateData({tagName: ''});
            setAlertData({message: 'Added'});
            fetchTableData();
            setTimeout(() => {
                console.log('hi');
                setAlertData({message: ''});
            }, 1000)
            
        }))
        .catch((err => {
            console.log(err);
        }))
    }

    const renderTable = (
        <div className="tag-dashboard-table-container">
            <table className = "tag-dashboard-table">
                <thead>
                    <tr>
                        <th>Name</th>
                    </tr>
                </thead>
                <tbody>
                    {data.map(item =>{
                        return(
                            <tr key={item} >
                                <div className = "row-tag-table">
                                    <span>&#10006;<td data-item = {item} onClick = {handleClick}>{item}</td></span>
                                </div>
                                
                            </tr>
                        );
                    })}
                </tbody>
            </table>
        </div>
    );

    const renderForm = (
        <div className = "tag-dashboard-form-container">
            <form className="tag-dashboard-create-form" onSubmit = {createTag}>
                    <div className="tag-dashboard-create-input-container">
                        <input type="text" value={createData.tagName} required placeholder="Tag Name" onChange = {handleChange}/>
                    </div>
                    <div className = "tag-dashboard-create-button">
                        <Button type="button" color="green" name="Create"/>
                    </div> 
                    <div className="create-tag-error"> {alertData.message} </div>
                </form>
        </div>
    );

    return (
        
        <div className="tag-dashboard-wrapper">
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
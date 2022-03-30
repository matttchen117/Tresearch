import axios from "axios";
import React, {useState, useEffect } from "react";

import "./TagDashboard.css";
import Button from "../../UI/Button/ButtonComponent";

function TagDashboard() {
    const [data, setData] = useState([]);
     
    const fetchTableData = () => {
        async function fetchData() {
            const request = await axios.get("https://localhost:7010/Tag/taglist");
            const responseData = await request.data;
            setData(responseData);
        }
        fetchData();
    }

    useEffect(() => {
        //Refresh after every 3 seconds
        const interval = setInterval(() => {
            fetchTableData();
        }, 3000);
        return () => clearInterval(interval);
    }, [])

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
                            <tr key={data.name}>
                                <td>{item}</td>
                            </tr>
                        );
                    })}
                </tbody>
            </table>
        </div>
    );

    const renderForm = (
        <div className = "tag-dashboard-form-container">
            <form className="tag-dashboard-create-form">
                    <div className="tag-dashboard-create-input-container">
                        <input type="text" required placeholder="Tag Name"/>
                    </div>
                    <div className = "tag-dashboard-create-button">
                        <Button type="button" color="green" name="Create"/>
                    </div> 
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
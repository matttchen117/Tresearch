import React, { useState } from "react";
import axios from 'axios';

import "./LogoutForm.css";

const LogoutForm = () => {
    // States
    const [errorMessages, setErrorMessages] = useState({});

    const handleSubmit = (event) => {
        event.preventDefault();


        axios.post('https://localhost:7010/Logout/logout?=', {})
        .then(res => {
            
         }).catch(err => console.log("api Erorr: ", err.message))
    };

    const logoutForm = (
        <div className="form-container">
            <div className="form-components">
                <form onSubmit={handleSubmit}>
                    <div className="create-button-container">
                        <input type="submit" value="Logout" />
                    </div>
                </form>
            </div>
        </div>
        
    );

    return (
        <div className="form">
            <div className="title-text">
                <h1 className="logout-title">Log In</h1>
            </div>
            {logoutForm}
        </div>
    );
}

export default LogoutForm;
import React, { useState } from "react";
import axios from 'axios';

import "./LogoutForm.css";

const LogoutForm = () => {
    // States
    const [errorMessages, setErrorMessages] = useState({});

    const handleSubmit = (event) => {
        event.preventDefault();


        axios.post('https://localhost:7010/Logout/logout?=', {})
        .then(response => {
            console.log(response.data);
            console.log(response.value)
            console.log(response.headers["TresearchAuthenticationCookie"])
            console.log(response.headers)
        }).catch(err => {
             console.log(err.data);
             console.log(err.value);
        })
    };

    const logoutForm = (
        <div className="form-container">
            <div className="form-components">
                <form onSubmit={handleSubmit}>
                    <div className="logout-button-container">
                        <input type="submit" value="Logout" />
                    </div>
                </form>
            </div>
        </div>
        
    );

    return (
        <div className="form">
            <div className="title-text">
                <h1 className="logout-title">Logout</h1>
            </div>
            {logoutForm}
        </div>
    );
}

export default LogoutForm;
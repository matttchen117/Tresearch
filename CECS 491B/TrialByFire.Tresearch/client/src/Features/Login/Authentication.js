import React, { useState } from "react";
import {useNavigate} from 'react-router-dom';
import axios from 'axios';

import "./Login.css";

const Authentication = () => {
    // States
    const [errorMessages, setErrorMessages] = useState({});

    const navigate = useNavigate();

    const errors = {
        credentials: "Invalid username or password"
    };

    const renderErrorMessage = (name) =>
        name === errorMessages.name && (
            <div className="error">{errorMessages.message}</div>
        );

    const handleSubmit = (event) => {
        event.preventDefault();
        var { username, otp } = document.forms[1];
        var authorizationLevel = "user";
        axios.post('https://localhost:7010/Authentication/authenticate?username=' + username + '&otp=' + otp 
        + '&authorizationLevel=' + authorizationLevel)
        .then(
            console.log("success")
        ).catch(err => console.log("api Erorr: ", err.message))
    };

    const renderBody = (
        <div className="form-container">
            <div className="form-components">
                <form onSubmit={handleSubmit}>
                    <div className="input-container">
                        <input type="username" name="uname" required placeholder="Username" />
                        {renderErrorMessage("uname")}
                    </div>
                    <div className="input-container">
                        <input type="otp" name="onetime" required placeholder="OTP" />
                        {renderErrorMessage("otp")}
                    </div>

                    <div className="create-button-container">
                        <input type="submit" value="Verify" />
                    </div>
                </form>
            </div>
        </div>
    );

    return (
        <div className="form">
            <div className="authentication-text">
                <h1 className="login-title">Verify</h1>
            </div>
            {renderBody}
        </div>
    );
}

export default Authentication;
import React, { useState } from "react";
import axios from 'axios';

import "./LoginForm.css";

const LoginForm = () => {
    // States
    const [errorMessages, setErrorMessages] = useState({});
    const [isSubmitted, setIsSubmitted] = useState(false);
    const [isConfirmed, setConfirmed] = useState(false);
    const [isVerified, setVerified] = useState(false);
    const [checked, setChecked] = useState(false);

    const errors = {
        credentials: "Invalid username or password"
    };

    const renderErrorMessage = (name) =>
        name === errorMessages.name && (
            <div className="error">{errorMessages.message}</div>
        );

    const handleSubmit = (event) => {
        event.preventDefault();
        var { username, passphrase, otp } = document.forms[0];
        var authorizationLevel = "user";
        if(!isConfirmed)
        {
            axios.post('https://localhost:7010/OTPRequest/requestotp?username=' + username + '&passphrase=' + passphrase 
            + '&authorizationLevel=' + authorizationLevel)
            .then(
                handleConfirm,
            ).catch(err => console.log("api Erorr: ", err.message))
        }else if(!isVerified){
            axios.post('https://localhost:7010/Authentication/authenticate?username=' + username + '&otp=' + otp 
            + '&authorizationLevel=' + authorizationLevel)
            .then(
                handleVerify,
            ).catch(err => console.log("api Erorr: ", err.message))
        }
    };

    const handleConfirm = () => {
        setConfirmed(!isConfirmed);
        username = "";
        passphrase = "";
    }

    const handleVerify = () => {
        setVerified(!isVerified);
    }


    const loginForm = (
        <div className="form-container">
            <div className="form-components">
                <form onSubmit={handleSubmit}>
                    <div className="input-container">
                        <input type="username" name="uname" required placeholder="Username" />
                        {renderErrorMessage("uname")}
                    </div>
                    <div className="input-container">
                        <input type="passphrase" name="pass" required placeholder="Passphrase" />
                        {renderErrorMessage("passphrase")}
                    </div>

                    <div className="create-button-container">
                        <input type="submit" value="Log In" />
                    </div>
                </form>
            </div>
        </div>
    ); 

    const authenticationForm = (
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

    if(!isConfirmed)
    {
        return (
            <div className="form">
                <div className="title-text">
                    <h1 className="login-title">Log In</h1>
                </div>
                {loginForm}
            </div>
        ); 
    }else if(!isVerified){
        return (
            <div className="form">
                <div className="title-text">
                    <h1 className="login-title">Verify</h1>
                </div>
                {authenticationForm}
            </div>
        );
    }else{
        return (
            <div className="form">
                <div className="title-text">
                    <h1 className="login-title">Success</h1>
                </div>
            </div>
        );
    }
}

export default LoginForm;
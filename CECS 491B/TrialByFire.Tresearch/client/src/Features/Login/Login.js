import React, { useState } from "react";
import {useNavigate} from 'react-router-dom';
import axios from 'axios';

import "./Login.css";

const Login = () => {
    // States
    const [errorMessages, setErrorMessages] = useState({});
    const specialChars = /[`!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/
    const validSpecialCharacters = /[`.,@!`]/


    function validateLoginForm(){
        // still allowing invalid special characters
        return username.length > 8 && (specialChars.test(username) ? validSpecialCharacters.test(username) : true) 
        &&
        (verified ? passphrase.length > 8 && !specialChars.test(passphrase) : otp.length > 8 
        || 
        (specialChars.test(otp) ? validSpecialCharacters.test(otp) : true))
    }

    const navigate = useNavigate();

    const errors = {
        credentials: "Invalid username or password"
    };

    const renderErrorMessage = (name) =>
        name === errorMessages.name && (
            <div className="error">{errorMessages.message}</div>
        );

    const [username, setUsername] = useState('')
    const [passphrase, setPassphrase] = useState('')
    const [otp, setOTP] = useState('')
    const authorizationLevel = 'user'

    const [error, setError] = useState(false)
    const [verified, setVerified] = useState(false)

    const onSubmit = (event) => {
        event.preventDefault();
        console.log(validateLoginForm())
        if(validateLoginForm())
        {
            setError(false)
            {verified ? 
                axios.post('https://localhost:7010/Authentication/authenticate?username=' + username.toLowerCase + '&otp=' + otp 
                + '&authorizationLevel=' + authorizationLevel)
                .then(response => {
                        console.log('success');
                        console.log(response.headers["TresearchAuthenticationCookie"])
                        console.log(response.headers)
                        //navigate('/Login/Authentication');
                }).catch(err => {
                        console.log(err.data);
                        setErrorMessages(err.data);
                        setError(true)
                    })
                :
                axios.post('https://localhost:7010/OTPRequest/requestotp?username=' + username + '&passphrase=' + passphrase 
                + '&authorizationLevel=' + authorizationLevel)
                .then(response => {
                        console.log('success');
                        console.log(response.data);
                        setVerified(true)
                        //navigate('/Login/Authentication');
                }).catch(err => console.log(err.data))
            }
        }else{
            console.log(errors.credentials)
            setError(true)
        }

        setUsername('')
        setPassphrase('')
        setOTP('')
    }

    

    const renderBody = (
        <div className="form-container">
            <div className="form-components">
                <form onSubmit={onSubmit}>
                    <div className="input-container">
                        <input 
                            type="username" 
                            name="uname" 
                            required placeholder="Email Address" 
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                        />
                    </div>
                    <div className="input-container">
                        {verified ? 
                            <input 
                            type="otp" 
                            name="otp" 
                            required placeholder="OTP" 
                            value={otp}
                            onChange={(e) => setOTP(e.target.value)}
                            />
                            :
                            <input 
                                type="password" 
                                name="pass" 
                                required placeholder="Password" 
                                value={passphrase}
                                onChange={(e) => setPassphrase(e.target.value)}
                            />
                        }
                    </div>
                    <div className="login-button-container">
                        {verified ? 
                            <input type="submit" value="Verify" />
                            :
                            <input type="submit" value="Log In" />
                        }
                    </div>
                    <div className="error-text">
                        {error && <h3 className="error-title">{error.credentials}</h3>}
                    </div>
                </form>
            </div>
        </div>
    ); 

    return (
        <div className="form">
            <div className="title-text">
                {verified ? <h1 className="login-title">Verify</h1> : <h1 className="login-title">Log In</h1>}
            </div>
            {renderBody}
        </div>
    ); 
}

export default Login;
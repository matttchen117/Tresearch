import React, { useState } from "react";
import {useNavigate} from 'react-router-dom';
import axios from 'axios';

import "./RegistrationForm.css";

const RegistrationForm = () => {
    // States
    const [errorMessages, setErrorMessages] = useState({});
    const [checked, setChecked] = useState(false);

    const navigate = useNavigate();

    const [data, setData] = useState({
        email: "pammypoor@gmail.com",
        passphrase: "myPassword123"
    });

    const renderErrorMessage = (name) =>
        name === errorMessages.name && (
            <div className="error">{errorMessages.message}</div>
        );

    const handleSubmit = (event) => {
        event.preventDefault();
        
        
        
        axios.post('https://localhost:7010/Registration/register?email=' + data.email + '&passphrase=' + data.passphrase
            ).then(response => {
                console.log(response.data)
            }).catch(err => console.log("api Erorr: ", err.message))
        
        axios.post('https://localhost:7010/Registration/confirmation?email=' + data.email
            ).then(response => {
                navigate('/Registration/ConfirmationSent');
            }).catch(err => console.log("api Erorr: ", err.message))
    };

    const handleCheck = () => {
        setChecked(!checked);
    }

    const renderForm = (
        <div className="form-container">
            <div className="form-components">
                <form onSubmit={handleSubmit}>
                    <div className="input-container">
                        <input type="username" name="uname" required placeholder="Email Address" />
                        {renderErrorMessage("uname")}
                    </div>
                    <div className="input-container">
                        <input type="password" name="pass" required placeholder="Password" />
                        {renderErrorMessage("pass")}
                    </div>
                    <div className="checkbox-container">
                        <input id = "ageCheck" type="checkbox" onChange={handleCheck} />
                        <label for="agree">I am 15 years or older </label>
                    </div>
                    <div className="create-button-container">
                        <input type="submit" value="Create" />
                    </div>
                </form>
            </div>
        </div>
    );

    return (
        <div className="form">
            <div className="title-text">
                <h1 className="register-title">Registration</h1>
            </div>
            {renderForm}
        </div>
    );
}

export default RegistrationForm;
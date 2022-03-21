import React, { useState } from "react";
import {useNavigate} from 'react-router-dom';
import axios from 'axios';

import "./RegistrationForm.css";
import Button from "../Button/ButtonComponent";


const RegistrationForm = () => {
    //States for form input
    const [checked, setChecked] = useState(false);
    const [email, setEmail] = useState("");
    const [passphrase, setPassphrase] = useState("");

    // States for checking the errors
    const[submitted, setSubmitted] = useState(false);
    const[errorMessages, setErrorMessages] = useState({});
    
    // Error messages
    const errors = {
        accountExists: "Account already exists",
        invalidInput: "Invalid username or passphrase",
        genericFail: "System is down. Try again soon."
    }


    //Navigate to route
    const navigate = useNavigate();

    //Handle change in checkbox status
    const handleChangeCheck = (event) => {
        setChecked(!checked);
    }

    //Handle credential validation
    const checkCredentials = () =>{
        
        if(email.length > 8 && passphrase.length > 8){
        
        } else {
            setErrorMessages({name: "errorMessage", message: errorMessages.invalidInput })
        
        }
    }

    //Handle clicking submit
    const handleSubmit = (event) => {
        //event.preventDefault();

       checkCredentials();

        if(errorMessages){
         
        }
        
    };

    const renderErrorMessage = (err) =>
        err === errorMessages.name &&(
            <div className="error">{errorMessages.message}</div>
          );

    const renderForm = (
        <div className="form-register-container">
             <form class="register-form">
                    <div className="input-container">
                        <input type="text" value={email} required placeholder="Email Address" onChange = {(e) => setEmail(e.target.value)}/>
                    </div>
                    <div className="input-container">
                        <input type="text" value={passphrase} required placeholder="Password" onChange = {(e) => setPassphrase(e.target.value)}/>
                    </div>
                    <div className = "input-check">
                        <label>I am 15 years or older</label>
                            <input type="checkbox" checked = {checked} required onChange = {handleChangeCheck}/>
                    </div>
                    <div className="create-button-container">
                        <Button color="green" name="Register" onClick={handleSubmit} />
                    </div>
                    {renderErrorMessage("errorMessage")}
                </form>
        </div>
    );

    return (
        <div className="form-register-wrapper">
            <div className="container-register-text">
                <h1 className="register-title">Registration</h1>
            </div>
            {renderForm}
        </div>
    );
}

export default RegistrationForm;
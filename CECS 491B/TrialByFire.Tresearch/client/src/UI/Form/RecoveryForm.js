import React, { useState } from "react";
import {useNavigate} from 'react-router-dom';
import axios from 'axios';

import "./RecoveryForm.css";
import Button from "../Button/ButtonComponent";

const RecoveryForm = () => {
    //States for form input
    const [email, setEmail] = useState("");

    // States for checking the errors
    const[submitted, setSubmitted] = useState(false);
    const[errorMessages, setErrorMessages] = useState({});
    
    // Error messages
    const errors = {
        
    }


    //Navigate to route
    const navigate = useNavigate();

    //Handle credential validation
    const checkCredentials = () =>{
        
        if(email.length > 8 ){
        
        } else {
            setErrorMessages({name: "errorMessage", message: errorMessages.invalidInput })
        
        }
    }

    //Handle clicking submit
    const handleRecoverSubmit = (event) => {
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
        <div className="form-recover-container">
             <form class="recover-form">
                    <div className="recover-input-container">
                        <input type="text" value={email} required placeholder="Email Address" onChange = {(e) => setEmail(e.target.value)}/>
                    </div>   
                    <div className="recover-button-container">
                        <Button color="green" name="Recover Account" onClick={handleRecoverSubmit} />
                    </div>
                    {renderErrorMessage("errorMessage")}
                </form>
        </div>
    );

    return (
        <div className="form-recover-wrapper">
            <div className="container-recover-text">
                <h1 className="recover-title">Your Account has been temporarily disabled</h1>
                
                <p className ="recover-text">Due to unusual activity on your account, you will need to recover your account</p>
            </div>
            {renderForm}
        </div>
    );
}

export default RecoveryForm;
import React, { useState } from "react";
import {useNavigate} from 'react-router-dom';
import axios, {AxiosResponse, AxiosError} from 'axios';

import "./LogoutForm.css";
import Button from "../Button/ButtonComponent";

class LogoutForm extends React.Component  {
    state = {
        errorMessage: ''
    }

    onSubmitHandler = (e) => {
        e.preventDefault();
        axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');
        axios.post('https://trialbyfiretresearchwebapi.azurewebsites.net/Logout/logout', {})
        .then(response => {
            sessionStorage.removeItem('authorization');
        }).catch(err => {

        })
    }

    render() {
        const renderForm = (
            <div className="form-logout-container">
                 <form className="logout-form" onSubmit = {this.onSubmitHandler}>
                        <div className="create-button-container">
                            <Button type="button" color="green" name="Logout"/>
                        </div>
                        {this.state.errorMessage && <div className="error-logout"> {this.state.errorMessage} </div>}
                    </form>
            </div>
        );

        return (
            <div className="form-logout-wrapper">
                <div className="container-logout-text">
                    <h1 className="logout-title">Logout</h1>
                </div>
                {renderForm}
            </div>
        );
    }
}

export default(LogoutForm);
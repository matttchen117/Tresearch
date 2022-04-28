import React from "react";
import axios from 'axios';

import "./RecoveryForm.css";
import Button from "../Button/ButtonComponent";

class RegistrationForm extends React.Component  {
    state = {
        email: '',
        errorMessage: ''
    }

    handleInput() {
        // [username]@[domain name].[domain]
        // Username: a-z, A-Z, 0-9, ._-
        // Domain Name: a-z, A-Z, 0-9, .-
        // Domain: a-z, A-Z
        // Satisfy all three requirements
        var regexEmail = new RegExp("^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{3}$");    
        if(!regexEmail.test(this.state.email)){
            this.setState({errorMessage: 'Invalid email'});
            return false;
        }
        return true; 
    }

    inputEmailHandler = (e) => {
        let updatedEmail = e.target.value;
        this.setState({ email: updatedEmail});
    }

    onSubmitHandler = (e) => {
        e.preventDefault();
        
        if(this.handleInput() == true){
            axios.post('https://localhost:7010/Recovery/SendRecovery?email=' + this.state.email.toLowerCase() + '&authorizationLevel=user')
            .then(res => {
                this.setState({errorMessage: ''})
                window.location = '/Recover/RecoverySent';
            })
            .catch( err => {
                switch(err.response.status){
                    case 403: this.setState({errorMessage: 'Account is not disabled'});
                        break;
                    case 404: this.setState({errorMessage: 'Account does not exist'});
                        break;
                    case 409: this.setState({errorMessgae: 'A recovery link has already been sent to your email'});
                        break;
                    default: this.setState({errorMessage: 'Unable to recover account'});
                }
            })
        }
    }

    render() {
        const renderForm = (
            <div className="form-recover-container">
                 <form className="recover-form" onSubmit = {this.onSubmitHandler}>
                        <div className="recover-input-container">
                            <input type="text" value={this.state.email} required placeholder="Email Address" onChange = {this.inputEmailHandler}/>
                        </div>
                        <div className="create-button-container">
                            <Button type="button" color="green" name="recover"/>
                        </div>
                        {this.state.errorMessage && <div className="error-recover"> {this.state.errorMessage} </div>}
                    </form>
            </div>
        );

        return (
            <div className="form-recover-wrapper">
                <div className="container-recover-text">
                    <h1 className="recover-title">Recover Account</h1>
                    <p className ="recover-text">Due to unusual activity on your account, you will need to recover your account</p>
                </div>
                {renderForm}
            </div>
        );
    }
}

export default(RegistrationForm);
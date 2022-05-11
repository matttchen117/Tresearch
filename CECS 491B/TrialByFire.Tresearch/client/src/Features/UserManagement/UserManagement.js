import axios from "axios";
import React, {useState } from "react";
import AuthenticatedNavBar from "../../UI/Navigation/AuthenticatedNavBar";


import "./UserManagement.css";
import Button from "../../UI/Button/ButtonComponent";

function UserManagement() {
    const [alertData, setAlertData] = useState(
        {
            message: ''
        }
    )

    const [accountData, setAccountData] = useState(
        {
            username: '',
            Passphrase: '',
            AuthorizationLevel: 'user',
            AccountStatus: true,
            Confirmed: false
        }
    )

    const handleUsernameChange = (e) => {
        setAlertData({message: ''});
        setAccountData( previousState => ({
            username: e.target.value,
            Passphrase: previousState.Passphrase,
            AuthorizationLevel: previousState.AuthorizationLevel,
            AccountStatus: previousState.AccountStatus,
            Confirmed: previousState.Confirmed,
        }));
    }

    const handlePassphraseChange = (e) => {
        setAlertData({message: ''});
        setAccountData( previousState => ({
            username: previousState.username,
            Passphrase: e.target.value,
            AuthorizationLevel: previousState.AuthorizationLevel,
            AccountStatus: previousState.AccountStatus,
            Confirmed: previousState.Confirmed,
        }));
    }

    const handleRoleChange = (e) => {
        setAlertData({message: ''});
        setAccountData( previousState => ({
            username: previousState.username,
            Passphrase: previousState.Passphrase,
            AuthorizationLevel: e.target.value,
            AccountStatus: previousState.AccountStatus,
            Confirmed: previousState.Confirmed,
        }));
    }

    const hashInput = (value) => {
        var pbkdf2 = require('pbkdf2');      
        const pbkdfKey = pbkdf2.pbkdf2Sync(value, '',  10000,  64, 'sha512');
        return pbkdfKey.toString('hex').toUpperCase();
    }

    const handleInput =() => {
        // [username]@[domain name].[domain]
        // Username: a-z, A-Z, 0-9, ._-
        // Domain Name: a-z, A-Z, 0-9, .-
        // Domain: a-z, A-Z
        // Satisfy all three requirements
        var regexEmail = new RegExp("^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{3}$");    
        
        //  passphrase: a-z, A-Z, 0-9, .,@! space
        var regexPassphrase = new RegExp("^[a-zA-Z0-9.,@!\s]+$");

        if(accountData.Passphrase.length < 8){
            setAlertData({message: 'Password must be at least 8 characters'});
            return false;
        }

        if(!regexEmail.test(accountData.username)){
            setAlertData({message: 'Invalid email'});
            return false;
        }

        if(!regexPassphrase.test(accountData.Passphrase)){
            setAlertData({message: 'Your password can only contain: \nblank space\na-z\nA-Z\n.,@!'});
            return false;
        }
        return true; 
    }

    const onSubmitHandler = (e) => {

        // pbkdf2 uses callbacks not promises, need to wrap in a promise object

        if(handleInput()){
            setAlertData({message: ''})
            axios.post('https://trialbyfiretresearchwebapi.azurewebsites.net/UserManagement/createAccount?username=' + accountData.username.toLowerCase() + '&passphrase=' + hashInput(accountData.Passphrase) + '&authorizationLevel=' + accountData.AuthorizationLevel )
            .then(res => {
                setAlertData({message: 'Account Created'});
            })
            .catch( err => {
                switch(err.response.status){
                    case 409: setAlertData({message: 'Account already exists'});
                        break;
                    default: setAlertData({message: 'Unable to create account'});
                }
            })
        }
    }

    const renderForm = (
        <div className = "um-dashboard-form-container">
            <form className="um-dashboard-create-form" onSubmit={onSubmitHandler}>
                    <div className="um-dashboard-create-input-container">
                        <input type="text" value={accountData.username} required placeholder="Username" onChange = {handleUsernameChange}/>
                        <input type="text" value={accountData.Passphrase} required placeholder="Passphrase" onChange = {handlePassphraseChange}/>
                        <select value = {accountData.AuthorizationLevel} onChange={handleRoleChange}>
                            <option value="user">User</option>
                            <option value="admin">Admin</option>
                        </select>
                    </div>
                    <div className = "um-dashboard-create-button">
                        <Button type="button" color="green" name="Create"/>
                    </div> 
                    <div className="create-um-error"> {alertData.message} </div>
                </form>
        </div>
    );

    const renderBack = (
        <div className = "um-dashboard-back-container">
            <a href="/Admin/Dashboard">&lt;&emsp;Return to Portal</a>
        </div>
    );

    return (
        
        <div className="um-dashboard-wrapper">
            <div className = "um-dashboard-nav-wrapper">
                {<AuthenticatedNavBar/>}
            </div>
            <div className = "um-dashboard-back-wrapper">
                {renderBack}
            </div>
            <div className = "um-dashboard-form-wrapper">
                {renderForm}
            </div>
        </div>
    )
}

export default UserManagement;
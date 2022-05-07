import React, { useCallback, useState, useEffect } from "react";
import "./Settings.css";
import axios, {AxiosResponse, AxiosError} from 'axios';

import Button from "../../UI/Button/ButtonComponent";


const handleDelete = (e) => { 
    const value = e.va;
    axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');


    axios.post("https://localhost:7010/AccountDeletion/DeleteAccount")
        .then((response => {
            console.log("success");
            sessionStorage.removeItem('authorization');
            window.location = '/';
        }))
}


function Settings() {

    const renderSettings = (
        <div className = "Settings-Container">
            <div className = "Settings-title">
                    <h1>Settings</h1>
                </div> 
                <div className = "Settings-text">
                   <Button type= "button" color="green" name="Delete Account" onClick={handleDelete}/>
                </div>
        </div>

        
    );

  return (
    <div className="settings"> 

        <div className="settings-wrapper">
            {renderSettings}
        </div>
    </div>
  );
}

export default Settings;
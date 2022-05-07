import React from "react";
import axios from 'axios';

import "./AccountDeleted.css";

const AccountDeleted = () => {
    
    const renderBody = (
        <div className="deleted-container">
            <div className = "deleted-notif">
                <hr></hr>
                <h1>We're sad to see you go</h1>
                <p>All data has been removed from our database</p>
            </div>
        </div>
    );

    return (
        <div className="component-container">
            <div className="title-text">
                <h1 className="deleted-title">Account Deleted</h1>
            </div>
            {renderBody}
        </div>
    );
}

export default AccountDeleted;
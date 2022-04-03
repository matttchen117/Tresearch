import React from "react";
import NavigationBar from "../../UI/Navigation/NavigationBar";
import "./AccountEnabled.css";

const AccountEnabled = () => {
    const renderBody = (
        <div className="account-enabled-container">
            <div className = "account-enabled-notif">
                <p>Your account has been enabled</p>
            </div>
        </div>
    );

    return (
        <div className="account-enabled-component-container">
            <NavigationBar/>
            <div className="account-enabled-title">
                <h1 className="account-enabled-title-text">Congradulations!</h1>
            </div>
            {renderBody}
        </div>
    );
}

export default AccountEnabled;
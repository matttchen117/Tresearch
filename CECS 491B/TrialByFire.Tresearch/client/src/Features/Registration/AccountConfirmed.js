import React from "react";
import NavigationBar from "../../UI/Navigation/NavigationBar";
import "./AccountConfirmed.css";

const AccountConfirmed = () => {
    const renderBody = (
        <div className="account-confirmed-container">
            <div className = "account-confirmed-notif">
                <p>Your account has been confirmed</p>
            </div>
        </div>
    );

    return (
        <div className="account-confirmed-component-container">
            <NavigationBar/>
            <div className="caccount-confirmed-title">
                <h1 className="account-confirmed-title-text">Congratulations!</h1>
            </div>
            {renderBody}
        </div>
    );
}

export default AccountConfirmed;
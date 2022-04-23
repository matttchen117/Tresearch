import React from "react";
import LoginForm from "../../UI/Form/LoginForm";
import "./LoginPopup.css"

const LoginPopup = props => {
    return (
        <div className="login-popup-page">
            <span className ="close-icon" onClick={props.onClick}>&#10006;</span>
            {<LoginForm/>}
        </div>
    )
}

  export default LoginPopup;
  
import React from "react";
import RegistrationForm from "../../UI/Form/RegistrationForm";
import "./RegistrationPopup.css"

const RegistrationPopup = props => {
    return (
        <div className="register-popup-page">
            <span className ="close-icon" onClick={props.onClick}>&#10006;</span>
            {<RegistrationForm/>}
        </div>
    )
}

  export default RegistrationPopup;
  
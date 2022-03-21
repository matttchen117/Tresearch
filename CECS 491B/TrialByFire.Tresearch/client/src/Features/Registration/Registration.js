import React from "react";
import RegistrationForm from "../../UI/Form/RegistrationForm";
import "./Registration.css"

function Register() {
    return (
      <div className="register-page">
        {<RegistrationForm/>}
      </div>
    );
  }
  
  export default Register;
  
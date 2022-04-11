import React from "react";
import LoginForm from "../../UI/Form/LoginForm";
import "./Login.css"

function Login() { 
    return (
      <div className="login-page">
        {<LoginForm />}
      </div>
    );
  }
  
  export default Login;
import React from "react";
import LogoutForm from "../../UI/Form/LogoutForm";
import "./Logout.css"

function Logout() { 
    return (
      <div className="logout-page">
        {<LogoutForm />}
      </div>
    );
  }
  
  export default Logout;
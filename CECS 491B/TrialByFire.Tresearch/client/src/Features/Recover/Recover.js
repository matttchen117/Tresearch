import React, { useState } from "react";
import RecoveryResend from "./RecoveryResend";
import RecoveryForm from "../../UI/Form/RecoveryForm";
import {useNavigate} from 'react-router-dom';
import NavigationBar from "../../UI/Navigation/NavigationBar";
import "./Recover.css"

function Recover() {
  
    return (
      <div className="recovery-page">
        <div className = "recovery-nav-bar">
          <NavigationBar/>
        </div>
        <div className = "recover-form-container">
          <RecoveryForm/>
        </div>
        
      </div>
    );
  }
  
  export default Recover;
  
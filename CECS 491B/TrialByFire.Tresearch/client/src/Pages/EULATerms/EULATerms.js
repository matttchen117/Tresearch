import React, { useCallback, useState, useEffect } from "react";
import NavigationBar from "../../UI/Navigation/NavigationBar";
import "./EULATerms.css";

function EULATerms() {

    const renderEula = (
        <div className = "eula-Container">
            <div className = "eula-title">
                <h1>End-User License Agreement</h1>
            </div> 
            <div className = "eula-text">
                <h2> Last Updated: 3/26/2022 </h2>
                <h3>Introduction</h3>
                <p>Please read this End-User License agreement before clicking the "I agree to the terms and conditions" checkbox. By clicking the checkbox, you are  agreeing
                   to the terms and conditions of this agreement. This agreement will remain in effect until terminated by you or Trial By Fire. You may terminate this agreement 
                   at any time by deleting your account.
                </p>
                <h3>Restrictions on Use</h3>
                <p>By agreeing to the terms, you certify that you are (a) above the age of 13 to comply with  
                    <a href = "https://www.ftc.gov/legal-library/browse/rules/childrens-online-privacy-protection-rule-coppa" target="_blank"> the Childrens Online Privacy Protection Act (COPPA) </a> 
                     and (b) reside within the United States of America.  
                </p>
                <h3>Termination</h3>
                <p>We reserve the right to terminate this license with or without prior notice. Failing to comply with our terms and conditions will result in a termination of this license.</p>
            </div>
        </div>
    );

   
  return (
    <div className="EULATerms"> 
        <div className = "eula-nav">
            {<NavigationBar/>}
        </div>
        <div className="eula-wrapper">
            {renderEula}
        </div>
    </div>
  );
}

export default EULATerms;

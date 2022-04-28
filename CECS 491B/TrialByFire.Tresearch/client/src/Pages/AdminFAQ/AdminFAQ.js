import React from "react";
import NavBar from "../../UI/Navigation/NavBar";
import "./AdminFAQ.css";

function AdminFAQ() {
    const renderTagFAQ = (
        <div className = "admin-feature-faq-container">
            
        </div>
    )

    const renderFAQ = (
        <div className = "admin-FAQ-container">
            <h1>FAQ</h1>
                {renderTagFAQ}
        </div>
    );

   
  return (
    <div className="admin-FAQ-wrapper"> 
        {<NavBar/>}
        <div className="admin-FAQ-content-wrappers">
           {renderFAQ}
        </div>
    </div>
  );
}

export default AdminFAQ;

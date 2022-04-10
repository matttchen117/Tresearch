import React, { useCallback, useState, useEffect } from "react";
import AuthenticatedNavBar from "../../UI/Navigation/AuthenticatedNavBar";
import './Portal.css';

function Portal() {
  return (
    <div className="Portal-wrapper"> 
        {<AuthenticatedNavBar/>}
    </div>
  );
}

export default Portal;

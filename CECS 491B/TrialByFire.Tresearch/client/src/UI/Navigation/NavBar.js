import React, { useEffect, useState } from "react";

import jwt_decode from "jwt-decode";
import AuthenicatedNavBar from "./AuthenticatedNavBar";
import NavigationBar from "./NavigationBar";

function NavBar() {
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    const checkToken = () => {
        const token = sessionStorage.getItem('authorization');
        if(token != null){
            const decoded = jwt_decode(token);
            setIsAuthenticated(true);
        }      
    }

    useEffect(() => {
        checkToken();
    }, [])
   
    return (
        <div className="navbar-wrapper"> 
            {isAuthenticated ? (<AuthenicatedNavBar/>) : (<NavigationBar/>)}
        </div>
    );
}

export default NavBar;

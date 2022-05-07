import React, { pPropTypes, useState } from "react";

import './App.css';
import NavBar from "./NavBar.js";
import RegistrationForm from "./Features/Registration/RegistrationForm.js"
function App() {
    const [isShowRegistration, setIsShowRegistration] = useState(true);

    const handleRegistrationClick = () => {
        setIsShowRegistration((isShowRegistration) => !isShowRegistration)
    }




    return (
        <div className="App">
            <RegistrationForm isShowRegistration={isShowRegistration} />
           loren ipsum
        </div>
    );
}

export default App;
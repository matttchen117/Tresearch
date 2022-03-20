import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import './App.css';
import RegistrationForm from './Features/Registration/RegistrationForm';
import ConfirmationSent from './Features/Registration/ConfirmationSent';
import InactiveLink from './Features/Registration/InactiveLink';
import LogoutForm from "./Features/Logout/LogoutForm";
import LoginForm from "./Features/Login/Login";
import Authentication from "./Features/Login/Authentication";
import Register from "./Features/Registration/Registration";
import Home from "./Pages/Home/Home";
import Tagger from "./Features/Tagging/Tagger";

function App() {
  return (
    <div className="App">
      <Router>
        <header></header>
        <Routes>
          <Route path="/Login/Login" element = {<LoginForm />}/>
          <Route path="/Login/Authentication" element = {<Authentication />}/>
          <Route path="/Logout/Logout" element = {<LogoutForm />}/>
          <Route path="/Registration/Register" element = {<RegistrationForm />}/>
          <Route path="/Registration/ConfirmationSent" element = {<ConfirmationSent />}/>
          <Route path="/Registration/InactiveLink" element = {<InactiveLink />}/>
          <Route path="/Home" element = {<Home />}/>
          <Route path="/Registration/Register" element = {<Register />}/>
          <Route path ="/Tag/Tagger" element = {<Tagger/>} />
        </Routes>
      </Router>
    </div>
  );
}

export default App;

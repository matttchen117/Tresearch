import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import './App.css';
import ConfirmationSent from './Features/Registration/ConfirmationSent';
import InactiveLink from './Features/Registration/InactiveLink';
import LogoutForm from "./Features/Logout/LogoutForm";
import LoginForm from "./Features/Login/Login";
import Authentication from "./Features/Login/Authentication";
import Register from "./Features/Registration/Registration";
import Home from "./Pages/Home/Home";
import Tagger from "./Features/Tagging/Tagger";
import Recover from "./Features/Recover/Recover";
import RecoveryForm from "./UI/Form/RecoveryForm";
function App() {
  return (
    <div className="App">
      <Router>
        <header></header>
        <Routes>
          <Route path="/" element = {<Home />}/>
          <Route path="/Login/Login" element = {<LoginForm />}/>
          <Route path="/Login/Authentication" element = {<Authentication />}/>
          <Route path="/Logout/Logout" element = {<LogoutForm />}/>
          <Route path = "/Recovery/Recover" element = {<Recover/>} />
        </Routes>
      </Router>
    </div>
  );
}

export default App;

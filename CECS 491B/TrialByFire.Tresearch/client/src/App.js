import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import './App.css';

import ConfirmationSent from './Features/Registration/ConfirmationSent';
import LogoutForm from "./Features/Logout/LogoutForm";
import LoginForm from "./Features/Login/Login";
import Authentication from "./Features/Login/Authentication";
import Register from "./Features/Registration/Registration";
import Home from "./Pages/Home/Home";
import EULATerms from "./Pages/EULATerms/EULATerms";
import Recover from "./Features/Recover/Recover";
import Confirm from "./Features/Registration/Confirm";
import AccountConfirmed from "./Features/Registration/AccountConfirmed";
import RecoveryConfirm from "./Features/Recover/RecoveryConfirm";
import AccountEnabled from "./Features/Recover/AccountEnabled";
import RecoverySent from "./Features/Recover/RecoverySent";
import TagDashboard from "./Features/Tagging/TagDashboard";
import Tagger from "./Features/Tagging/Tagger";

class App extends React.Component {
  constructor(props){
    super(props);
    this.state = {
      confirmationGuid: Confirm,
      recoveryConfirm: RecoveryConfirm
    };
  }

  render() {
    return (
      <div className="App">
        <Router>
          <header></header>
          <Routes>
            <Route path="/" element = {<Home />}/>
            <Route path="/Register/ConfirmationSent" element = {<ConfirmationSent />}/>
            <Route path="/Register/EULATerms" element = {<EULATerms/>} />
            <Route path ="/Register/Confirm/:confirmationGuid" element = {<Confirm guid={this.state.confirmationGuid}/>}/>
            <Route path = "/Register/AccountConfirmed" element = {<AccountConfirmed/>}/>
            <Route path="/Register" element = {<Register />}/>
            <Route path="/Login/Login" element = {<LoginForm />}/>
            <Route path="/Login/Authentication" element = {<Authentication />}/>
            <Route path="/Logout/Logout" element = {<LogoutForm />}/>
            <Route path = "/Recover" element = {<Recover/>} />
            <Route path = "/Recover/RecoverySent" element = {<RecoverySent/>} />
            <Route path = "/Recover/Enable/:recoveryConfirm" element = {<RecoveryConfirm guid={this.state.recoveryConfirm}/>}/>
            <Route path = "/Recover/AccountEnabled" element = {<AccountEnabled/>} />
            <Route path = "/Admin/TagDashboard" element = {<TagDashboard/>} />
            <Route path = "/Tagger" element = {<Tagger/>}/>
          </Routes>
        </Router>
      </div>
    );
  }
}

export default App;

import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import axios, {AxiosResponse, AxiosError} from 'axios';
import './App.css';
import UserManagement from "./Features/UserManagement/UserManagement";

import ConfirmationSent from './Features/Registration/ConfirmationSent';
import Login from "./Features/Login/Login";
import Logout from "./Features/Logout/Logout";
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
import InactiveLink from "./Features/Registration/InactiveLink";
import Portal from "./Pages/Portal/Portal";
import AdminPortal from "./Pages/AdminPortal/AdminPortal";
import AdminLogin from "./Pages/AdminLogin/AdminLogin";
import AdminFAQ from "./Pages/AdminFAQ/AdminFAQ";
import Error404 from "./Pages/Error404/Error404";
import Settings from "./Pages/Settings/Settings";
import FAQ from "./Pages/FAQ/FAQ";
import Features from "./Pages/Features/Features";
import Search from "./Features/Search/NodeSearch";
import SearchPage from "./Features/Search/SearchPage";
import CreateNode from "./Features/CreateNode/CreateNode"

class App extends React.Component {
  constructor(props){
    super(props);
    axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');
    this.state = {
      confirmationGuid: Confirm,
      inactiveLinkGuid: InactiveLink,
      recoveryConfirm: RecoveryConfirm,
      nodeID: Tagger
    };
  }

  render() {
    return (
      <div className="App">
        <Router>
          <header></header>
          <Routes>
            //Any page that doesn't have path will load as Error 404
            <Route path = '*' exact = {true} element = {<Error404/>}/>
            <Route path="/" element = {<Home />}/>
            <Route path = "/Portal" element ={<Portal />}/>
            <Route path = "/Settings" element = {<Settings/>} exact/>
            <Route path = "/Features" element = {<Features/>}/>
            <Route path = "/FAQ" element ={<FAQ />}/>

            <Route path="/Login/Login" element = {<Login />}/>
            <Route path="/Logout/Logout" element = {<Logout />}/>

            <Route path="/Register/ConfirmationSent" element = {<ConfirmationSent />}/>
            <Route path="/Register/EULATerms" element = {<EULATerms/>} />
            <Route path ="/Register/Confirm/:confirmationGuid" element = {<Confirm guid={this.state.confirmationGuid}/>}/>
            <Route path ="/Register/InactiveLink/:inactiveLinkGuid" element = {<InactiveLink guid={this.state.inactiveLinkGuid}/>}/>
            <Route path = "/Register/AccountConfirmed" element = {<AccountConfirmed/>}/>
            <Route path="/Register" element = {<Register />}/>
            <Route path = "/Recover" element = {<Recover/>} />
            <Route path = "/Recover/RecoverySent" element = {<RecoverySent/>} />
            <Route path = "/Recover/Enable/:recoveryConfirm" element = {<RecoveryConfirm guid={this.state.recoveryConfirm}/>}/>
            <Route path = "/Recover/AccountEnabled" element = {<AccountEnabled/>} />

            <Route path = "/Admin/Login" element = {<AdminLogin/>} />
            <Route path = "/Admin/Dashboard" element = {<AdminPortal/>} />
            <Route path = "/Admin/TagDashboard" element = {<TagDashboard/>} />
            <Route path = "/Admin/UserManagement" element = {<UserManagement/>} />
            <Route path = "/Admin/FAQ" element = {<AdminFAQ/>}/>

            <Route path = "/Tagger" element = {<Tagger/>} exact/>
            <Route path = "/Search" element = {<Search/>} exact/>
            <Route path = "/SearchPage" element = {<SearchPage/>} exact/>
          </Routes>
        </Router>
      </div>
    );
  }
}

export default App;

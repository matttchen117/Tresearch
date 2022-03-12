import {
  BrowserRouter as Router,
  Routes,
  Route
} from "react-router-dom";
import './App.css';
import RegistrationForm from './Features/Registration/RegistrationForm';
import ConfirmationSent from './Features/Registration/ConfirmationSent';
import InactiveLink from './Features/Registration/InactiveLink';
import LogoutForm from "./Features/Logout/LogoutForm";
import LoginForm from "./Features/Login/LoginForm";

function App() {
  return (
    <div className="App">
      <Router>
        <header></header>
        <Routes>
          <Route path="/Login/Login" element = {<LoginForm />}/>
          <Route path="/Logout/Logout" element = {<LogoutForm />}/>
          <Route path="/Registration/Register" element = {<RegistrationForm />}/>
          <Route path="/Registration/ConfirmationSent" element = {<ConfirmationSent />}/>
          <Route path="/Registration/InactiveLink" element = {<InactiveLink />}/>
        </Routes>
      </Router>
    </div>
  );
}

export default App;

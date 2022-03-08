import {
  BrowserRouter as Router,
  Routes,
  Route
} from "react-router-dom";
import './App.css';
import RegistrationForm from './Features/Registration/RegistrationForm';
import ConfirmationSent from './Features/Registration/ConfirmationSent';
import InactiveLink from './Features/Registration/InactiveLink';

function App() {
  return (
    <div className="App">
      <Router>
        <header></header>
        <Routes>
        <Route path="/Authentication/Login" element = {<RegistrationForm />}/>

          <Route path="/Registration/Register" element = {<RegistrationForm />}/>
          <Route path="/Registration/ConfirmationSent" element = {<ConfirmationSent />}/>
          <Route path="/Registration/InactiveLink" element = {<InactiveLink />}/>
        </Routes>
      </Router>
    </div>
  );
}

export default App;

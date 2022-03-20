import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import './App.css';

import Register from "./Features/Registration/Registration";
import Home from "./Pages/Home/Home";

function App() {
  return (
    <div className="App">
      <Router>
        <header></header>
        <Routes>
        <Route path="/Home" element = {<Home />}/>
        <Route path="/Registration/Register" element = {<Register />}/>
        </Routes>
      </Router>
    </div>
  );
}

export default App;

import React from "react";
<<<<<<< HEAD
import { useParams,} from "react-router-dom";
import "./InactiveLink.css";
import axios from 'axios';

class InactiveLink extends React.Component {
     render() {
        function GetGuid() {
            const { confirmationGuid } = useParams();
            console.log(confirmationGuid);
            if(confirmationGuid != null){
                axios.post('https://localhost:7010/Registration/resend?'+confirmationGuid)
                .then(res => {
                    window.location = '/Register/AccountConfirmed';
                    return res;
                })
                .catch( err => {
                    //window.location = '/Register/EULATerms';
                    console.log(err);
                    return err;
                })
            }
            return null;
        }

        const renderConfirm = (
          <div className = "register-confirm-container">
              <p><GetGuid/></p>
          </div>
        )
      return (
          <div className = "register-confirm-wrapper">
            {renderConfirm}
          </div>
      );
  }
=======
import axios from 'axios';

import "./InactiveLink.css";

const InactiveLink = () => {
    
    const handleSubmit = (event) => {
        event.preventDefault();
        var { username, password } = document.forms[0];

        axios.post('https://localhost:44303/Registration/register?=', {username, password})
        .then(res => {
            
         })
    };

    const renderBody = (
        <div class="inactive-container">
            <div class = "inactive-notif">
                <hr></hr>
                <h1>Uh Oh! Looks like your link expired.</h1>
                <div className="email-button-container">
                        <input type="submit" value="Resend Verification link "  onSubmit={handleSubmit}/>
                    </div>
            </div>
        </div>
    );

    return (
        <div className="component-container">
            <div className="title-text">
                <h1 className="inactive-title">Email confirmation link has expired</h1>
            </div>
            {renderBody}
        </div>
    );
>>>>>>> Working
}

export default InactiveLink;
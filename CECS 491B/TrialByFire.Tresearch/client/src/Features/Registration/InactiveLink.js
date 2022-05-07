import React from "react";
import { useParams,} from "react-router-dom";
import "./InactiveLink.css";
import axios from 'axios';

class InactiveLink extends React.Component {
     render() {
        function GetGuid() {
            const { confirmationGuid } = useParams();
            console.log(confirmationGuid);
            if(confirmationGuid != null){
                axios.post('http://trialbyfiretresearchwebapi.azurewebsites.net//Registration/resend?'+confirmationGuid)
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
}

export default InactiveLink;
import React from "react";
import { useParams,} from "react-router-dom";
import "./RecoveryConfirm.css";
import axios from 'axios';

class RecoveryConfirm extends React.Component {
     render() {
        function GetGuid() {
            const { recoveryConfirm } = useParams();
            if(recoveryConfirm != null){
                axios.post('https://trialbyfiretresearchwebapi.azurewebsites.net/Recovery/recover?'+recoveryConfirm)
                .then(res => {
                    window.location = '/Recover/AccountEnabled';
                    return res;
                })
                .catch( err => {
                    
                    switch(err.response.status){
                        case 404: {
                        }
                            break;
                        default: {

                        }
                    }
                })
            }
            return null;
        }

        const renderConfirm = (
          <div className = "recover-confirm-container">
              
              <p><GetGuid/></p>
          </div>
        )
      return (
          <div className = "recover-confirm-wrapper">
            {renderConfirm}
          </div>
      );
  }
}

export default RecoveryConfirm;
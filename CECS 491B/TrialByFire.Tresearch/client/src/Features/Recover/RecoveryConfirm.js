import React from "react";
import { useParams,} from "react-router-dom";
import NavigationBar from "../../UI/Navigation/NavigationBar";
import "./RecoveryConfirm.css";
import axios, {AxiosResponse, AxiosError} from 'axios';

class RecoveryConfirm extends React.Component {
     render() {
        function GetGuid() {
            const { recoveryConfirm } = useParams();
            console.log(recoveryConfirm);
            if(recoveryConfirm != null){
                axios.post('https://trialbyfiretresearchwebapi.azurewebsites.net/Recovery/recover?'+recoveryConfirm)
                .then(res => {
                    window.location = '/Recover/AccountEnabled';
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
import React from "react";
import ChangeContent from "../../Features/ChangeContent/ChangeNodeContent";

const ChangeContentPopup = props => {
    return (
        <div className="content-popup-page">
            <span className ="close-icon" onClick={props.onClick}>&#10006;</span>
            {<ChangeContent node = {props.node}/>}
        </div>
    )
}

  export default ChangeContentPopup;
  
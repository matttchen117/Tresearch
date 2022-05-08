import React from "react";
import RateNode from "../../Features/Rate/RateNode";

const RateNodePopup = props => {
    return (
        <div className="rate-popup-page">
            <span className ="close-icon" onClick={props.onClick}>&#10006;</span>
            {<RateNode nodes = {props.nodes}/>}
        </div>
    )
}

  export default RateNodePopup;
  
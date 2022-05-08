import React from "react";
import NodeView from "../Components/NodeView/NodeView";

const NodePopup = props => {
    return (
        <div className="node-popup-page">
            <span className ="close-icon" onClick={props.onClick}>&#10006;</span>
            {<NodeView node = {props.node} IsEnabled = {props.canMakeChanges}/>}
        </div>
    )
}

export default NodePopup;
  
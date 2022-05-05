import React from "react";
import CreateNode from "../../Features/CreateNode/CreateNode";
import NodeCreationForm from "../Form/NodeCreationForm";

const CreateNodePopup = props => {
    return (
        <div className="create-node--popup-page">
            <span className ="close-icon" onClick={props.onClick}>&#10006;</span>
            {<NodeCreationForm cRForm = {props.node}/>}
        </div>
    )
}

  export default CreateNodePopup;
  
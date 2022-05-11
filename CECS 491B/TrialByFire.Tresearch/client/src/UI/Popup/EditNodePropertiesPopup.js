import React from "react"
import EditNodePropertiesForm from "../Form/EditNodePropertiesForm"

const EditNodePropertiesPopup = props => {
    return (
        <div className="edit-node-properties-page">
            <span className = "close-icon" onClick={props.onClick}>&#10006;</span>
            {<EditNodePropertiesForm cRForm = {props.node}/>}
        </div>
    )
}

export default EditNodePropertiesPopup
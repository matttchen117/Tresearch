import React from "react";
import Tagger from "../../Features/Tagging/Tagger";

const TagPopup = props => {
    return (
        <div className="tag-popup-page">
            <span className ="close-icon" onClick={props.onClick}>&#10006;</span>
            {<Tagger nodes = {props.nodes}/>}
        </div>
    )
}

  export default TagPopup;
  
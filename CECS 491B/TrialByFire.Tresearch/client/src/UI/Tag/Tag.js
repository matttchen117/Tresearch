import React from "react";
import './Tag.css';

const Tag = ({name = "name"}) => {
 

    return(
        <div className="tag-container">
            <li ><a href="#" className = "tag" data-item = {name}>{name}&emsp;&times;</a></li>
        </div>
    )
}

export default Tag;
import React from "react";
import './Tag.css';

const Tag = ({name = "name",  onClick}) => {
    const onButtonClick = () => {
        if(onClick) {
            onClick(name);
        }
    }

    return(
        <div className="tag-container">
            <li><a href="#" class = "tag">{name}&emsp;&times;</a></li>
        </div>
    )
}

export default Tag;
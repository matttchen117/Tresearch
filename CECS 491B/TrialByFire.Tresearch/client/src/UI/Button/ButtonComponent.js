import React from "react";
import './ButtonComponent.css';

const Button = ({color = "green", name = "name", disabled = false,  onClick}) => {
    const onButtonClick = () => {
        if(onClick) {
            onClick(name);
        }
    }

    return(
        <div className="bttn-container">
            <button className = {`custom-bttn ${color}`} disabled = {disabled} onClick={onButtonClick}>{name}</button>
        </div>
    )
}

export default Button;
import React from "react";

import Tag from "../../UI/Tag/Tag";
import "./Tagger.css"

function Tagger() {
  return (
    <div className="tagger-container">
        <div className = "tags-container">
          <ul>
            <Tag name="Cooking"></Tag>
            <Tag name="Vegan"></Tag>
            <Tag name="Gluten Free"></Tag>
            <Tag name="Foodie"></Tag>
          </ul>
        </div>
    </div>
  );
}
  
  export default Tagger;
  
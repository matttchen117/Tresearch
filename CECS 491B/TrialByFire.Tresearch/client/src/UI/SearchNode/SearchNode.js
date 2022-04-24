import React from "react";
import './SearchNode.css';

const SearchNode = ({userHash = "userHash", nodeID = "nodeID", nodeTitle = "nodeTitle", summary = "summary", 
               timeModified = "timeModified", exactMatch = "exactMatch", tags = {}, tagScore = 0, ratingScore = 0}) => {
 
    return(
        <button className="searchnode">
            {nodeTitle}
            {summary}
            {timeModified}
            {tags}
        </button>
    )
}

export default SearchNode;
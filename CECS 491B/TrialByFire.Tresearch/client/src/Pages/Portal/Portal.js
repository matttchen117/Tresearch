import React, { useCallback, useState, useEffect } from "react";
import NavBar from "../../UI/Navigation/NavBar";
import TreeView from "../../UI/Components/Tree/TreeView";
import './Portal.css';

function Portal() {
  const nodes = {
    name: 'Tree 1',
    visibility: true,
    children: [
        {
          name: 'Tree 2',
          visibility: false,
          children: [
            {
              name: 'Tree 4',
              visibility: false
            }
          ]
        },
        {
          name: 'Tree 3',
          visibility: true
        }
    ]
  }

  return (
    <div className="Portal-wrapper"> 
        {<NavBar/>}
        <div className = "portal-tree-wrapper">
            <TreeView nodes = {nodes} />
        </div>
    </div>
    
  );
}

export default Portal;

import React, { useCallback, useState, useEffect } from "react";
import NavBar from "../../UI/Navigation/NavBar";
import TreeView from "../../UI/Components/Tree/TreeView";
import './Portal.css';

function Portal() {
  


  const nodes = {
    name: 'Tree 1',
    attributes: {
      nodeID: 1
    },
    children: [
        {
          name: 'Tree 2',
          attributes: {
            nodeID: 2
          },
          children: [
            {
              name: 'Tree 4',
              attributes: {
                nodeID: 4
              }
            }
          ]
        },
        {
          name: 'Tree 3',
          attributes: {
            nodeID: 3
          }
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

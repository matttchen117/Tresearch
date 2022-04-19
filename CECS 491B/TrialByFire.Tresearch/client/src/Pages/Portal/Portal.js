import React, { useCallback, useState, useEffect } from "react";
import NavBar from "../../UI/Navigation/NavBar";
import TreeView from "../../UI/Components/Tree/TreeView";
import './Portal.css';

function Portal() {
  


  const nodes = {
    name: 'Computer Programming',
    attributes: {
      nodeID: 1809
    },
    children: [
        {
          name: 'Intro to HTML/CSS',
          attributes: {
            nodeID: 1810
          },
          children: [
            {
              name: 'Intro to SQL',
              attributes: {
                nodeID: 1811
              }
            }
          ]
        },
        {
          name: 'Intro to Javascript',
          attributes: {
            nodeID: 1812
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

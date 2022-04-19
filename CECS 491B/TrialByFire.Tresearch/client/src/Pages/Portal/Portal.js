import React, { useCallback, useState, useEffect } from "react";
import NavBar from "../../UI/Navigation/NavBar";
import TreeView from "../../UI/Components/Tree/TreeView";
import './Portal.css';

function Portal() {
  


  const nodes = {
    name: 'Cooking with thumbs',
    attributes: {
      nodeID: 1809
    },
    children: [
        {
          name: 'Pretzal Supremacy',
          attributes: {
            nodeID: 1810
          },
          children: [
            {
              name: 'Fishing for wheels',
              attributes: {
                nodeID: 1811
              }
            }
          ]
        },
        {
          name: 'CS',
          attributes: {
            nodeID: 1812
          }
        }
    ],
    name: 'TEST',
    attributes: {
      nodeID: 1813
    }
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

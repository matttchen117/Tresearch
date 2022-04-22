import React from "react";
import NavBar from "../../UI/Navigation/NavBar";
import TreeView from "../../UI/Components/Tree/TreeView";
import './Portal.css';

function Portal() {
  
   const nodes = {
    name: 'Computer Programming',
    attributes: {
      nodeID: 2
    },
    children: [
        {
          name: 'Intro to HTML/CSS',
          attributes: {
            nodeID: 3
          },
          children: [
            {
              name: 'Intro to SQL',
              attributes: {
                nodeID: 5
              }
            }
          ]
        },
        {
          name: 'Intro to Javascript',
          attributes: {
            nodeID: 4
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

import React, { pPropTypes, useState } from "react";
import axios from "axios";
import Tree from 'react-d3-tree';


import './App.css';
import NavBar from "./NavBar.js";

const orgChart = {
    name: 'CEO',
    children: [
      {
        name: 'Manager',
        attributes: {
          department: 'Production',
        },
        children: [
          {
            name: 'Foreman',
            attributes: {
              department: 'Fabrication',
            },
            children: [
              {
                name: 'Worker',
              },
            ],
          },
          {
            name: 'Foreman',
            attributes: {
              department: 'Assembly',
            },
            children: [
              {
                name: 'Worker',
              },
            ],
          },
        ],
      },
    ],
  };
  
  export default function OrgChartTree() {
    return (
      // `<Tree />` will fill width/height of its container; in this case `#treeWrapper`.
      <div id="treeWrapper" style={{ width: '50em', height: '20em' }}>
        <Tree data={orgChart} />
      </div>
    );
  }

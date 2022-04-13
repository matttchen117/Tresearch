import React, { useState } from "react";
import Tree from 'react-d3-tree';
import { ContextMenu, MenuItem, ContextMenuTrigger } from "react-contextmenu";

import "./TreeView.css";

function TreeView ({nodes})  {
    const nodeClick = (e) => {
        console.log(e);
    }

    const renderContextMenu = (
        <div className = "tree-context-menu-options">
            <ContextMenu id = "tree-context">
                <MenuItem>Copy</MenuItem>
            </ContextMenu>
        </div>
    );


    const renderTree = (
        <div className="tree-container">
            <ContextMenuTrigger id = "tree-context" className = "tree-context-container" height = "500px">
                <Tree 
                    data = {nodes}
                    rootNodeClassName = "node__root"
                    orientation="vertical"
                    pathFunc={"straight"}
                    onLinkClick = {nodeClick}
                />
            </ContextMenuTrigger>
             
        </div>
    );

    return (
        <div className="tree-wrapper">
            {renderTree}
        </div>
    );
}

export default(TreeView);
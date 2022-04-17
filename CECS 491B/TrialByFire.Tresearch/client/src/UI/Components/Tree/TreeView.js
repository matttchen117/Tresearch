import React, { useState } from "react";
import { TreeEditor, treeRenderedCallback } from 'react-d3-tree-editor';
import { ContextMenu, MenuItem, ContextMenuTrigger } from "react-contextmenu";
import Tagger from "../../../Features/Tagging/Tagger";
import Popup from "../../Popup/Popup";
import TagPopup from "../../Popup/TagPopup";

import "./TreeView.css";
import LoginPopup from "../../Popup/LoginPopup";

class TreeView extends React.Component{
    constructor(props) {
        super(props);
        this.token = sessionStorage.getItem('authorization');
        this.treeRef = null;
        this.treeData = props.nodes;
        this.treeConfig = {
            margin: {
              top: 20,
              right: 120,
              bottom: 20,
              left: 120
            },
            textMargin: 20,
            duration: 750,
            nodeSize: [40, 70]
        }
        this.state = {
            isTaggerOpen: false,
            nodeSelect: -1
        }
    }

    getContextMenu = (e) => {

        return [{
            title: 'Add Node',
            action: () => {
                
            },
            enabled: true
        },
        {
            title: 'Delete Node',
            action: (e) => {
                
            },
            enabled: true
        },
        {
            title: 'Edit Node',
            action: () => {
                
            },
            enabled: true
        },
        {
            title: 'Copy Node',
            action: () => {
                
            },
            enabled: true
        },
        {
            title: 'Edit Tags',
            action: () => {
                var arr = [];
                arr.push(e.attributes.nodeID);
                this.setState({
                    isTaggerOpen: true,
                    nodeSelect: arr
                });
            },
            enabled: true
        },
    ];
    }

    


    render() {

        document.addEventListener('click', (e) => {

            if(e.shiftKey) {
                console.log("SHIFT CLICK");
            }
            
        });

        const treeRenderedCallback = (e) => {
            this.treeRef.expandAllElemenets();
        }

        const  ToggleTagger = () => {
            this.setState({
                isTaggerOpen: false
            })
        }

        const renderTree = (
            <div className = "tree-portal-container">
                <div className= {`${this.state.isTaggerOpen ? "taggerOpen" : "base"}`}>
                    <TreeEditor 
                            treeData = {this.treeData}
                            treeConfig = {this.treeConfig}
                            getContextMenu={this.getContextMenu}
                            onRef = {ref => (this.treeRef = ref)}
                    />
                </div>
            </div>
        );
    
        return (
            <div className="tree-portal-wrapper">
                <div className = "tree-wrapper">
                    {renderTree}
                </div>
                <div className = "tree-tagger-wrapper">
                    {this.state.isTaggerOpen ? <Popup content = {<TagPopup onClick = {ToggleTagger} nodes = {this.state.nodeSelect}/>}/> : null}
                </div>
            </div>
        );
    }
}

export default(TreeView);
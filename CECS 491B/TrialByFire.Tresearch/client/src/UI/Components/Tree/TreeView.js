import React, { useEffect, useState } from "react";
import Tree from "react-d3-tree";
import Tagger from "../../../Features/Tagging/Tagger";
import Popup from "../../Popup/Popup";
import TagPopup from "../../Popup/TagPopup";
import "./TreeView.css";

class TreeView extends React.Component{
    constructor(props) {
        super(props);
        this.token = sessionStorage.getItem('authorization');

        this.treeData = props.nodes;

        this.treeDimensions = this.treeCon

        this.treeConfiguration = {
            orientation: 'vertical',
            totalNodeCount: 4,
            collapsible: false,
            translate: {
                x: window.innerWidth/2,
                y: window.innerHeight/6
            }
        }

        this.menuContextConfiguration = {
            hideOnLeave: true
        }


        this.state = {
            isTaggerOpen: false,
            isPopupOpen: false,
            nodeSelect: -1,
            shiftDown: false,
            shiftCollection: [],
            isShown: false,
            x: 0,
            y: 0
        }
    }

    setShiftDown = (e) => {
        if(e.shiftKey) {
            this.setState( {shiftDown: true});
        }
    }

    setShiftUp = (e) => {
        this.setState( {shiftDown: false});
    }

    componentDidMount(){
        document.addEventListener('keydown', this.setShiftDown);
        document.addEventListener('keyup', this.setShiftUp);
        document.addEventListener('contextmenu', (e) => {
            e.preventDefault();
        });
        console.log(this.treeData);
    }

    componentWillUnmount() {
        document.removeEventListener('keydown', this.setShiftDown);
        document.removeEventListener('keyup', this.setShiftUp);
        document.removeEventListener('contextmenu', (e) => {
            e.preventDefault();
        });
    }

    setShiftCollection = async (e) => {
        var currentState = this.state.shiftCollection;
        if(!currentState.includes(e)){
            this.setState({shiftCollection: [...currentState, e]}, function() {

            })
        }
    }

    EditNodes = (e) => {
        e.stopPropagation();
        this.setState( { isTaggerOpen: true, isShown: false,  x: e.pageX, y: e.pageY})
        console.log(this.state.nodeSelect);
    }

     render() {

        const  ToggleTagger = () => {
            this.setState({
                isTaggerOpen: false
            })
        }

        const  leftClickNode = (e, nodeData) => {
            this.setState( { isShown: false, x: e.pageX, y: e.pageY})
            e.stopPropagation();
            if(this.state.shiftDown){
                var currentState = this.state.shiftCollection;
                console.log(currentState)
                if(!currentState.includes(nodeData.attributes.nodeID)){
                    this.setState({shiftCollection: [...currentState, nodeData.attributes.nodeID]})
                    console.log(this.shiftCollection);
                }
                else{
                    // Up to you but may want to navigate to view node
                }
            } 
        }

        const rightClickNode = (e, nodeData) => {
            this.setState({ nodeSelect: [...this.state.shiftCollection, nodeData.attributes.nodeID], shiftCollection: []});
            this.setState( { isShown: true, x: e.pageX, y: e.pageY})
        }

        const resetShiftCollection = (e) => {
            this.setState( { isShown: false, x: e.pageX, y: e.pageYm, nodeSelect: [], shiftCollection: []})
        }

        const attributes = {
            'data-count': 0,
            className: 'example-multiple-targets well'
        };
        

        const renderNodeWithCustomEvents = ({
            nodeDatum
        }) => (
            <g data-item = {nodeDatum.attributes.nodeID} id = {nodeDatum.attributes.nodeID}>          
                <circle r = "20" onClick = {(e) => leftClickNode(e, nodeDatum) }  data-item = {nodeDatum.attributes.nodeID} onContextMenu = {(e) => rightClickNode(e, nodeDatum) }/>
                <text fill = "black" x = "20" dy = "20" data-item = {nodeDatum.attributes.nodeID}> 
                    {nodeDatum.name}
                </text>       
            </g>
            
        );

        var test = 'Cooking with thumbs';
        var test2 = 'Pretzal Supremacy';

        const nodeSize = { x: 200, y: 200 };

        const renderTree = (
            <div className = "tree-portal-container">
                <div className= {`${this.state.isTaggerOpen ? "taggerOpen" : "base"}`} onClick = {resetShiftCollection} >
                    <Tree 
                            data = {this.treeData} 
                            orientation = {this.treeConfiguration.orientation}
                            collapsible = {this.treeConfiguration.collapsible} 
                            translate = {this.treeConfiguration.translate}
                            renderCustomNodeElement = {(nodeInfo) => renderNodeWithCustomEvents({...nodeInfo})}
                            nodeSize = {nodeSize}
                    />
                    {this.state.isShown && (
                        <div style={{ top: this.state.y, left: this.state.x}}  className="tag-context-menu" >
                            <div className="option" >
                                Add Child
                            </div>
                            <div className="option" >
                                Edit Node
                            </div>
                            <div className="option" onClick = {this.EditNodes}>
                                Edit Tags
                            </div>
                            <div className="option">
                                Delete Node(s)
                            </div>
                        </div>
                    )}
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
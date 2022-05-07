import React from "react";
import Tree from "react-d3-tree";
import Popup from "../../Popup/Popup";
import TagPopup from "../../Popup/TagPopup";
import "./TreeView.css";

class TreeView extends React.Component{
    constructor(props) {
        super(props);

        // Users token
        this.token = sessionStorage.getItem('authorization');

        // Node Data
        this.treeData = props.nodes;

        // Configuration of tree
        this.treeConfiguration = {
            orientation: 'vertical',
            collapsible: false,
            translate: {
                x: window.innerWidth/2,
                y: window.innerHeight/10
            },
            nodeSize: {
                x: 200,
                y: 200
            },
            color: "#344e41", 
            stroke: 'black',
            strokeWidth: "1",
            pathFunc: 'diagonal'
        }

        this.treeOnHighlight = {
            stroke: "#99e2b4",
            strokergb : "rgb(153, 226, 180)",
            strokeWidth: "6"
        }

        // Configuration ot tree context menu
        this.menuContextConfiguration = {
            hideOnLeave: true
        }

        // Initial state of tree view
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

    // Change state when shift key is pressed down
    setShiftDown = (e) => {
        if(e.shiftKey) {
            this.setState( {shiftDown: true});
        }
    }

    // Change state when shift key is released
    setShiftUp = (e) => {
        this.setState( {shiftDown: false});
    }

    // When page load run 
    componentDidMount(){
        document.addEventListener('keydown', this.setShiftDown);
        document.addEventListener('keyup', this.setShiftUp);
        document.addEventListener('contextmenu', (e) => {
            e.preventDefault();
        });
    }

    // When leaving page run
    componentWillUnmount() {
        document.removeEventListener('keydown', this.setShiftDown);
        document.removeEventListener('keyup', this.setShiftUp);
        document.removeEventListener('contextmenu', (e) => {
            e.preventDefault();
        });
    }

    // Set state of shift colletion
    setShiftCollection = async (e) => {
        var currentState = this.state.shiftCollection;
        if(!currentState.includes(e)){
            this.setState({shiftCollection: [...currentState, e]});
        }
    }

    unhighlight = () => {
        var elements = document.querySelectorAll("*");
        for (var i = 0; i < elements.length; i++){
            elements[i].style.stroke = this.treeConfiguration.stroke;
            elements[i].style.strokeWidth = this.treeConfiguration.strokeWidth;
        }
    }

    // User clicks edit 
    EditTags = (e) => {
        e.stopPropagation();
        const currentState = Array.from(new Set(this.state.nodeSelect));
        this.setState( { nodeSelect: currentState, isTaggerOpen: true, isShown: false,  x: e.pageX, y: e.pageY}) 
        this.unhighlight();
    }

     render() {
        const  ToggleTagger = () => {
            
            this.setState({
                isTaggerOpen: false
            })     
        }

        // User left clicks a node
        const  leftClickNode = (e, nodeData) => {
            this.setState( { isShown: false, x: e.pageX, y: e.pageY})
            e.stopPropagation();

            // Check if user is trying to select multiple
            if(this.state.shiftDown){
                var currentState = this.state.shiftCollection;
                if(!currentState.includes(nodeData.nodeID)){
                    handleHighLight(e, nodeData.nodeID);
                    this.setState({shiftCollection: [...currentState, nodeData.nodeID]})
                } else{
                    // Douible click (remove from shift collection)
                    this.setState({shiftCollection: this.state.shiftCollection.filter(x => x != nodeData.nodeID) });
                    handleHighLight(e, nodeData.nodeID);
                }
            } else{
                this.setState({ shiftCollection: []});
                // Up to you but may want to navigate to view node
            }
        }

        // Handle highlight of shift collection
        const handleHighLight = (e, node) => {   
            var currentState = this.state.highlightCollection;
            if(e.target.style.stroke === this.treeOnHighlight.strokergb){
                e.target.style.stroke = this.treeConfiguration.stroke;
                e.target.style.strokeWidth = this.treeConfiguration.strokeWidth;
            } else{
                e.target.style.stroke = this.treeOnHighlight.stroke;
                e.target.style.strokeWidth = this.treeOnHighlight.strokeWidth;
            }   
        }

        // Right click node, open context menu
        const rightClickNode = (e, nodeData) => {
            this.setState({ nodeSelect: [...this.state.shiftCollection, nodeData.nodeID], shiftCollection: []});
            this.setState( { isShown: true, x: e.pageX, y: e.pageY})
        }

        // Clear shift collection
        const resetShiftCollection = (e) => {
            this.setState( { isShown: false, x: e.pageX, y: e.pageYm, nodeSelect: [], shiftCollection: []})
            this.unhighlight(); 
        }

        // Render individual nodes
        const renderNodeWithCustomEvents = ({
            nodeDatum
        }) => (
            <g data-item = {nodeDatum.nodeID} id = {nodeDatum.nodeID}>          
                <circle className = "circle" fill = {this.treeConfiguration.color} stroke = "black" strokeWidth = "1" r = "20" onClick = {(e) => leftClickNode(e, nodeDatum) }  data-item = {nodeDatum.nodeID} onContextMenu = {(e) => rightClickNode(e, nodeDatum) }/>
                <text fill = "black" x = "20" dy = "20" data-item = {nodeDatum.nodeID}> 
                    {nodeDatum.nodeTitle}
                </text>       
            </g>
        );

        // Render user's tree
        const renderTree = (
            <div className = "tree-portal-container">
                <div className= {`${this.state.isTaggerOpen ? "taggerOpen" : "base"}`} onClick = {resetShiftCollection} >
                    {this.treeData.length != 0 ? (
                        <Tree 
                        data = {this.treeData} 
                        orientation = {this.treeConfiguration.orientation}
                        collapsible = {this.treeConfiguration.collapsible} 
                        translate = {this.treeConfiguration.translate}
                        renderCustomNodeElement = {(nodeInfo) => renderNodeWithCustomEvents({...nodeInfo})}
                        nodeSize = {this.treeConfiguration.nodeSize}
                        pathFunc = {this.treeConfiguration.pathFunc}
                       />
                    ): null}
                    {this.state.isShown && (
                        <div style={{ top: this.state.y, left: this.state.x}}  className="tag-context-menu" >
                            <div className="option" >
                                Add Child
                            </div>
                            <div className="option" >
                                Edit Node
                            </div>
                            <div className="option" onClick = {this.EditTags}>
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
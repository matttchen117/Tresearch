import axios from "axios";
import React from "react";
import Tree from "react-d3-tree";
import jwt_decode from "jwt-decode";  
import Popup from "../../Popup/Popup";
import TagPopup from "../../Popup/TagPopup";
import NodePopup from "../../Popup/NodePopup";
import CreateNodePopup from "../../Popup/CreateNodePopup";
import ChangeContentPopup from "../../Popup/ChangeContentPopup";
import "./TreeView.css";
import RateNodePopup from "../../Popup/RateNodePopup";

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
            isPopupOpen: false,
            isNodeViewOpen: false,
            isRateViewOpen: false,
            isEditViewOpen: false,
            copiedNodes: [],
            selectedNode: [],
            selectNodeTotal: [],
            shiftNodeTotalCollection: [],
            isTaggerOpen: false,
            isCreateNodeOpen: false,
            isPopupOpen: false,
            nodeTotalSelect: -1,
            nodeSelect: -1,
            shiftDown: false,
            shiftCollection: [],
            isShown: false,
            isGuest: true,
            isOwner: false,
            x: 0,
            y: 0
        }

        const token = sessionStorage.getItem('authorization');
        
        if(token){
            const decoded = jwt_decode(token);
            const tokenExpiration = decoded.tokenExpiration;
            const now = new Date();

            // Check if expired
            if(now.getTime() > tokenExpiration * 1000){
                localStorage.removeItem('authorization');
                window.location.assign(window.location.origin);
                window.location = '/';
                
            }

            if(decoded.userHash === props.nodes.userHash){
                this.state.isOwner = true;
            } 

            this.state.isGuest = false;

        } else{
            this.state.isGuest = true;
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

    //User clicks Add Child
    CreateNode = (e) => {
        e.stopPropagation();
        const currentState = Array.from(new Set(this.state.nodeSelect));
        this.setState( { nodeSelect: currentState, isCreateNodeOpen: true, isPopupOpen: true, isShown: false,  x: e.pageX, y: e.pageY})
    }

    DeleteNode = (e) => {
        e.stopPropagation();
        const n = {userHash: this.state.nodeSelect[0].userHash,
                        nodeID: this.state.nodeSelect[0].nodeID,
                        parentNodeID: this.state.nodeSelect[0].parentNodeID,
                        nodeTitle: "",
                        summary: "",
                        timeModified: "2022-05-04T21:08:21.714Z",//temp value
                        visibility: true,
                        deleted: false,
                        exactMatch: false,
                        tagScore: 0,
                        ratingScore: 0}
        console.log(n)
        axios.post("https://trialbyfiretresearchwebapi.azurewebsites.net/DeleteNode/deleteNode",n)
        .then(response=> {
            const responseData = Object.values(response.data);
            console.log(responseData);
            window.location.reload();
        })
    }

    EditNode = (e) => {
        e.stopPropagation();
        const currentState = Array.from(new Set(this.state.nodeSelect));
        this.setState( { nodeSelect: currentState, isEditViewOpen: true, isPopupOpen: true, isShown: false,  x: e.pageX, y: e.pageY})
    }

    // User clicks edit
    EditTags = (e) => {
        e.stopPropagation();
        const currentState = Array.from(new Set(this.state.nodeSelect));
        this.setState( { nodeSelect: currentState, isTaggerOpen: true, isPopupOpen: true, isShown: false,  x: e.pageX, y: e.pageY})
        this.unhighlight();
    }

    RateNodes = (e) => {
        e.stopPropagation();
        const currentState = Array.from(new Set(this.state.nodeSelect));
        this.setState({nodeSelect: currentState, isRateViewOpen: true, isPopupOpen: true, isShown: false, x: e.pageX, y: e.pageY})
        this.unhighlight();
    }

    handleEncoded = (e) => {
        var parsedData = e.toString();
        if(parsedData.includes('!')){
            parsedData = parsedData.replaceAll('!', '%21');
        }
        if(parsedData.includes('#')){
            parsedData = parsedData.replaceAll('#', '%23');
        }
        if(parsedData.includes('$')){
            parsedData = parsedData.replaceAll('$', '%24');
        }

        // DO NOT DO % SHOULD NOT BE REPLACED

        if(parsedData.includes('&')){
            parsedData = parsedData.replaceAll('&', '%26');
        }
        if(parsedData.includes('+')){
            parsedData = parsedData.replaceAll('+', '%2B');
        }
        if(parsedData.includes('[')){
            parsedData = parsedData.replaceAll('[', '%5B')
        }
        if(parsedData.includes(']')){
            parsedData = parsedData.replaceAll(']', '%5D')
        }
        return parsedData;
    }

    CopyNodes = (e) => {
        e.stopPropagation();
        const shiftClickedNodes = Array.from(new Set(this.state.nodeSelect));
        console.log(shiftClickedNodes)
        axios.post("https://trialbyfiretresearchwebapi.azurewebsites.net/CopyAndPaste/Copy", shiftClickedNodes)
        .then(response => {
            const responseData = Object.values(response.data);
            this.setState({copiedNodes: responseData});
            sessionStorage.setItem("nodes", JSON.stringify(this.state.copiedNodes));
        })
    }

    PasteNodes = (e) => {
        e.stopPropagation();
        const shiftClickedNodes = Array.from(new Set(this.state.nodeSelect));
        console.log(shiftClickedNodes)

        var nodes = this.handleEncoded(sessionStorage.getItem("nodes"))
        console.log(nodes)


        var nodes = this.handleEncoded(sessionStorage.getItem("nodes"));

        axios.post("https://trialbyfiretresearchwebapi.azurewebsites.net/CopyAndPaste/Paste?nodeIDToPasteTo"+this.state.nodeSelect+"?nodes"+nodes)
        .then(response=> {
            const responseData = Object.values(response.data);
            this.setState({pastedNodes: responseData});
            sessionStorage.setItem("pastednodes", JSON.stringify(this.state.pastedNodes))
        })
    }

    PrivateNodes = (e) => {
        e.stopPropagation();
        const nodesToPrivate = Array.from(new Set(this.state.nodeSelect));
        console.log(nodesToPrivate)
        axios.post("https://trialbyfiretresearchwebapi.azurewebsites.net/PrivateAndPublic/Private", nodesToPrivate)
        .then(response => {
            const responseDate = Object.values(response.data);
            this.setState({privateNodes: responseDate});
            sessionStorage.setItem("privatedNodes", this.state.privateNodes);
        }
        )
    }

    PublicNodes = (e) => {
        e.stopPropagation();
        const nodesToPublic = Array.from(new Set(this.state.nodeSelect));
        console.log(nodesToPublic)
        axios.post("https://trialbyfiretresearchwebapi.azurewebsites.net/PrivateAndPublic/Public", nodesToPublic)
        .then(response => {
            const responseDate = Object.values(response.data);
            this.setState({publicNodes: responseDate});
            sessionStorage.setItem("privatedNodes", this.state.publicNodes);
        }
        )
    }

     render() {
        const  ToggleTagger = () => {

            this.setState({
                isTaggerOpen: false,
                isPopupOpen: false
            })
        }

        const  ToggleEdit = () => {
            window.location.reload(false)
            this.setState({
                isEditViewOpen: false,
                selectedNode: [],
                isPopupOpen: false
            })
        }

        const ToggleCreate = () => {
            this.setState({
                isCreateNodeOpen: false,
                isPopupOpen: false
            })
        }

        const ToggleRatePopup = () => {
            this.setState( {isRateViewOpen: false, isPopupOpen: false})
            this.unhighlight();
        }

        const ToggleNodePopup = () => {
            this.setState({ isNodeViewOpen: false, isPopupOpen: false});
            this.unhighlight();
        }

        // User left clicks a node
        const  leftClickNode = (e, nodeData) => {
            this.setState( { isShown: false, x: e.pageX, y: e.pageY})
            e.stopPropagation();

            // Check if user is trying to select multiple
            if(this.state.shiftDown){
                var currentState = this.state.shiftCollection;
                var currentNodesState = this.state.shiftNodeTotalCollection;
                if(!currentState.includes(nodeData.nodeID)){
                    handleHighLight(e, nodeData.nodeID);
                    this.setState({shiftCollection: [...currentState, nodeData.nodeID], shiftNodeTotalCollection: [...currentNodesState, nodeData]})
                } else{
                    // Douible click (remove from shift collection)
                    this.setState({shiftCollection: this.state.shiftCollection.filter(x => x != nodeData.nodeID), shiftNodeTotalCollection: this.state.shiftCollection.filter(x=> x!= nodeData) });
                    handleHighLight(e, nodeData.nodeID);
                }
            } else{
                this.setState({ shiftCollection: [], nodeSelect: nodeData});
                // Up to you but may want to navigate to view node
                this.setState({ isNodeViewOpen: true, isPopupOpen: true});
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
            this.setState({ selectedNode: nodeData})
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
                <circle className = "circle" fill= {nodeDatum.visibility ? "#344e41 ": "gray"} stroke = "black" strokeWidth = "1" r = "20" onClick = {(e) => leftClickNode(e, nodeDatum) }  data-item = {nodeDatum.nodeID} onContextMenu = {(e) => rightClickNode(e, nodeDatum) }/>
                <text fill = "black" x = "20" dy = "20" data-item = {nodeDatum.nodeID}>
                    {nodeDatum.nodeTitle}
                </text>
            </g>
        );

        // Render user's tree
        const renderTree = (
            <div className = "tree-portal-container">
                <div className= {`${this.state.isPopupOpen ? "taggerOpen" : "base"}`} onClick = {resetShiftCollection} >
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
                    {this.state.isShown && this.state.isOwner && (
                        <div style={{ top: this.state.y, left: this.state.x}}  className="tag-context-menu" >
                            <div className="option" onClick = {this.CreateNode}>
                                Add Child
                            </div>
                            <div className="option" onClick = {this.EditNode}>
                                Edit Node
                            </div>
                            <div className="option" onClick = {this.EditTags}>
                                Edit Tags
                            </div>
                            
                            <div className="option" onClick = {this.CopyNodes}>
                                Copy Node(s)
                            </div>
                            <div className="option" onClick = {this.PasteNodes}>
                                Paste Node(s)
                            </div>
                            <div className="option" onClick = {this.PrivateNodes}>
                                Privatize Node(s)
                            </div>
                            <div className="option" onClick = {this.PublicNodes}>
                                Publicize Node(s)
                            </div>
                            <div className="option" onClick = {this.DeleteNode}>
                                Delete Node(s)
                            </div>
                        </div>
                    )}
                    {this.state.isShown && !this.state.isOwner && this.state.isGuest == false &&  (
                        <div style={{top: this.state.y, left: this.state.x}} className="nonowner-context-menu">
                            <div className="option" onClick={this.CopyNodes}>
                                Copy Nodes
                            </div>
                            <div className="option" onClick={this.RateNodes}>
                                Rate Nodes
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
                    {this.state.isCreateNodeOpen ? <Popup content = {<CreateNodePopup onClick = {ToggleCreate} node = {this.state.nodeSelect}/>}/> : null}
                </div>
                <div className = "node-popup-wrapper">
                    {this.state.isEditViewOpen ? <Popup content = {<ChangeContentPopup onClick={ToggleEdit} node={this.state.selectedNode}/>}/> : null }
                    {this.state.isNodeViewOpen ? <Popup content = {<NodePopup onClick={ToggleNodePopup} node = {this.state.nodeSelect}/>}/> : null }
                </div>
                <div className = "node-rate-wrapper">
                    {this.state.isRateViewOpen ? <Popup content = {<RateNodePopup onClick= {ToggleRatePopup} nodes = {this.state.nodeSelect} />} /> : null }
                </div>
            </div>
        );
    }
}

export default(TreeView);

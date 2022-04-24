import axios from "axios";
import React, { useEffect, useState } from "react";
import NavBar from "../../UI/Navigation/NavBar";
import TreeView from "../../UI/Components/Tree/TreeView";
import { useSearchParams } from "react-router-dom";

class SearchPage extends React.PureComponent{
  constructor(props){
    super(props);

    this.state = {
      page: new URLSearchParams(window.location.search).get('page'),
      nodes: [{}]
    }

    this.defaultNodes = [{
      "rootNode": {
        "children": [
          {
            "children": [
              {
                "children": [
                  {
                    "children": [],
                    "userHash": "AD89551B3BF5021B53AC0C9878DE96EAB72816241C417DDF2FB421BD78B7B7477372245C5EF36FEEE1A5DB096596D170309A904D9D0FDA6FAD4071148AD67C75",
                    "nodeID": 339,
                    "nodeParentID": 5,
                    "nodeTitle": "Querying",
                    "summary": "Performing Operations to the Database",
                    "timeModified": "0001-01-01T00:00:00",
                    "visibility": true,
                    "deleted": false,
                    "exactMatch": false,
                    "tags": null,
                    "tagScore": 0,
                    "ratingScore": 0
                  }
                ],
                "userHash": "AD89551B3BF5021B53AC0C9878DE96EAB72816241C417DDF2FB421BD78B7B7477372245C5EF36FEEE1A5DB096596D170309A904D9D0FDA6FAD4071148AD67C75",
                "nodeID": 5,
                "nodeParentID": 3,
                "nodeTitle": "Intro to SQL",
                "summary": "Best Language",
                "timeModified": "0001-01-01T00:00:00",
                "visibility": true,
                "deleted": false,
                "exactMatch": false,
                "tags": null,
                "tagScore": 0,
                "ratingScore": 0
              }
            ],
            "userHash": "AD89551B3BF5021B53AC0C9878DE96EAB72816241C417DDF2FB421BD78B7B7477372245C5EF36FEEE1A5DB096596D170309A904D9D0FDA6FAD4071148AD67C75",
            "nodeID": 3,
            "nodeParentID": 2,
            "nodeTitle": "Intro to HTML/CSS",
            "summary": "Summary of Hypertext language",
            "timeModified": "0001-01-01T00:00:00",
            "visibility": true,
            "deleted": false,
            "exactMatch": false,
            "tags": null,
            "tagScore": 0,
            "ratingScore": 0
          },
          {
            "children": [],
            "userHash": "AD89551B3BF5021B53AC0C9878DE96EAB72816241C417DDF2FB421BD78B7B7477372245C5EF36FEEE1A5DB096596D170309A904D9D0FDA6FAD4071148AD67C75",
            "nodeID": 4,
            "nodeParentID": 2,
            "nodeTitle": "Intro to Javascript",
            "summary": "Annoying Language",
            "timeModified": "0001-01-01T00:00:00",
            "visibility": true,
            "deleted": false,
            "exactMatch": false,
            "tags": null,
            "tagScore": 0,
            "ratingScore": 0
          }
        ],
        "userHash": "AD89551B3BF5021B53AC0C9878DE96EAB72816241C417DDF2FB421BD78B7B7477372245C5EF36FEEE1A5DB096596D170309A904D9D0FDA6FAD4071148AD67C75",
        "nodeID": 2,
        "nodeParentID": 2,
        "nodeTitle": "Computer Programming",
        "summary": "Summary of Computer Programming",
        "timeModified": "0001-01-01T00:00:00",
        "visibility": true,
        "deleted": false,
        "exactMatch": false,
        "tags": null,
        "tagScore": 0,
        "ratingScore": 0
      }
    }]
  }

  mapper = (nodeData) => {
    return nodeData.map((node) => {
      if(node.rootNode != null){
          if(node.rootNode.children.length > 0){
            node.rootNode.children = this.mapper(node.rootNode.children);
          }
      } else{
        if(node.children.length > 0){
          node.children = this.mapper(node.children);
        }
        else node.children = []; 
      }
      return node;
    })
  }

  setup = () => {
    axios.get("https://localhost:7010/TreeManagement/getNodes?owner=" + this.state.page)
    .then((response => {
      console.log(response.data)
      var temp = Object.values(response.data);
      var temp2 = this.mapper(temp);
      console.log(temp2[0]);
      return temp2[0].rootNode;
      console.log(response.data)
      this.setState( {nodes: this.mapper(Object.values(response.data))})
      //console.log(this.state.nodes[0].rootNode)
      return this.state.nodes[0].rootNode;
    }))
    .catch((err => {
      switch(err.response.status){
          case 503: {
                  console.log("Database offline");
                  //this.setState( {nodes: this.mapper(Object.values(this.defaultNodes))})
          }
          break;
      }
    }))
    return this.state.nodes;
  }

  componentDidMount() {
  }
  
  render () {
    return (
      <div className="Portal-wrapper"> 
        {<NavBar/>}
        <div className = "portal-tree-wrapper">
          <TreeView nodes = {this.setup()} />
        </div>
    </div>
    )
  }
}

export default SearchPage;

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
      nodes: null
    }
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

  setup = (n) => {
    var temp = Object.values(n);
    var temp2 = this.mapper(temp);
    return temp2[0];
  }

  async componentDidMount() {
    //axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');
    const response = await axios.get("https://trialbyfiretresearchwebapi.azurewebsites.net/TreeManagement/getNodes?owner=" + this.state.page)
    if(response !== null && response !== undefined)
    {
      const nodes = this.setup(response.data);
    this.setState({nodes});
    }
  }
  
  render () {
    return (
      <div className="Portal-wrapper"> 
        {<NavBar/>}
        <div className = "portal-tree-wrapper">
          {this.state.nodes && (
            <TreeView nodes = {this.state.nodes} />
          )}
        </div>
    </div>
    )
  }
}

export default SearchPage;

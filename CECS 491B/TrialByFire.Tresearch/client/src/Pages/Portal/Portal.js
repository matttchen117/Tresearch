import React from "react";
import axios from "axios";
import NavBar from "../../UI/Navigation/NavBar";
import TreeView from "../../UI/Components/Tree/TreeView";
import jwt_decode from "jwt-decode";  
import './Portal.css';

class Portal extends React.PureComponent{
  constructor(props){
    super(props);

    this.token = sessionStorage.getItem('authorization');

    this.state = {
      nodes:  null,
      nodesGet: [],
      userHash: ''
    }
  }

  // Map backend return to data usable by react-d3
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

  componentDidMount() {
    const token = this.checkToken();
    if(token != null){
      // Set token header when sending post
      axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');
      axios.get("https://localhost:7010/TreeManagement/getNodes?owner=" + token.userHash)
      .then( res => {
        const nodes = this.setup(res.data);
        this.setState({nodes});
      });
    }  
  }

  // Check JWT Token
  checkToken = () => {
    const token = sessionStorage.getItem('authorization');
    if(token){
        // Token exists, decode and check credentials
        const decoded = jwt_decode(token);
        const tokenExpiration = decoded.tokenExpiration;
        const now = new Date();

        // Check if expired
        if(now.getTime() > tokenExpiration * 1000){
            localStorage.removeItem('authorization');
            window.location.assign(window.location.origin);
            window.location = '/';
        }

        return decoded;
    }else{
        // Token doesn't exist or not valid
        localStorage.removeItem('authorization');
        window.location.assign(window.location.origin);
        window.location = '/';
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

export default Portal;

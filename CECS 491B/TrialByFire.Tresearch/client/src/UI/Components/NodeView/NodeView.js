import axios from "axios";
import async from "pbkdf2/lib/async";
import React, {useState, useEffect } from "react";          // Usestate to initialize data on load, useEffect run on load and every 10s
import NavBar from "../../Navigation/NavBar";
import Rating from "../../Rating/Rating";
import jwt_decode from "jwt-decode";  
import Tag from "../../Tag/Tag";
import "./NodeView.css";

class NodeView extends React.PureComponent {
    constructor(props) {
        super(props);
        this.state = {
            node: props.node,
            rating: null,
            userRating: null,
            canMakeChanges: false,
            token: null,
            tags: []
        }
    }

    componentDidMount() {
        this.GetRatings();
        this.state.token = this.checkToken();
        
        
    }

    checkToken = () => {
        const token = sessionStorage.getItem('authorization');
        if(token){
            // Token exists, decode and check credentials
            const decoded = jwt_decode(token);
            const tokenExpiration = decoded.tokenExpiration;
            const userHash = decoded.userHash;
            const now = new Date();

            // Check if expired
            if(now.getTime() > tokenExpiration * 1000){
                localStorage.removeItem('authorization');
                window.location.assign(window.location.origin);
                window.location = '/';
            }

            if(userHash != this.props.node.userHash){
                this.GetUserRating();
                this.setState({canMakeChanges: true})
            } else{
                this.GetTags();
            }
               

            return decoded;
        }else{
            // Token doesn't exist or not valid
            localStorage.removeItem('authorization');
            if(this.state.node.visibility == false){
                window.location.assign(window.location.origin);
                window.location = '/';
            }
        }
    }

    GetRatings = async () => {
        axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');
        const ID = [this.state.node.nodeID]
        await axios.post("https://localhost:7010/Rate/getRating?", ID)
        .then(response => {
            const r = Math.round(response.data[0].ratingScore * 100) / 100;
            this.setState({rating: r })
            document.getElementById("my-node-rating-id").innerHTML = "Rating: " + r;
            sessionStorage.setItem('authorization', response.headers['authorization']);
        })
        .catch((err => {
        }))
    }

    GetUserRating = async() => {
        axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');
        const ID = [this.state.node.nodeID]
        await axios.post("https://localhost:7010/Rate/getUserNodeRating?nodeID="+ID)
        .then(response => {
            this.setState({userRating: response.data})
            sessionStorage.setItem('authorization', response.headers['authorization']);
        })
        .catch((err => {

        }))
    }

    SetRating = async (rating) => {
        axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');
        console.log(this.state.node);
        await axios.post("https://localhost:7010/Rate/rateNode?rating=" + rating, [this.state.node.nodeID])
        .then(response => {
            this.GetRatings();
            sessionStorage.setItem('authorization', response.headers['authorization']);
        })
        .catch(err => {
            console.log(err.response);
        })
    }


   render() {
    const renderSummary = (
        <div>
            {this.state.node.summary}
        </div>
    )

    const renderTags = (
        <div className="node-view-tagger-container">
            {!this.state.canMakeChanges ? 
                <div>
                    <p>Tags:</p>
                    <ul>
                    {this.state.tags.sort().map(item => 
                        <span key={item}><Tag name = {item}/></span>
                        )}
                    </ul>
                </div>
            : null}

        </div>
    )

    const RenderRatings = () => (
        <div className="rating-container">
            {this.state.rating == 0 ? <div className = "node-view-no-ratings"> no ratings </div>: <div id = "my-node-rating-id"> Rating: {this.state.rating} </div>}
        </div>
    )
    const renderTitle = (
        <div className = "node-title">
            <p>{this.state.node.nodeTitle}
                {this.state.rating != null && <RenderRatings/>}
            </p>
            
        </div>
    )


    const renderSetRatings = (
        <div>
            
            {this.state.canMakeChanges && 
                <div>
                    <p>How would you rate this node?
                    <Rating rating={this.state.userRating} SetRating={this.SetRating} IsEnabled = {this.state.canMakeChanges}/>
                    </p>
                    
                </div>
            }
        </div>
    )

    return (       
        <div className="node-wrapper">
            <div className="node-container">
               <div className="node-info-container">
                    <div className="node-title-container">
                        {renderTitle}
                    </div>
                    {renderTags}
                    
                    <div className="node-summary-container">
                        {renderSummary}
                    </div>
                    <div className="node-rating-container">
                        {this.state.token && this.state.userRating != null && <div>
                            {renderSetRatings}
                        </div>}
                    </div>
               </div>          
            </div>
        </div>
    )
   }
}

export default NodeView;
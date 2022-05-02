import axios from "axios";
import async from "pbkdf2/lib/async";
import React, {useState, useEffect } from "react";          // Usestate to initialize data on load, useEffect run on load and every 10s
import NavBar from "../../Navigation/NavBar";
import Rating from "../../Rating/Rating";
import jwt_decode from "jwt-decode";  
import "./NodeView.css";

class NodeView extends React.PureComponent {
    constructor(props) {
        super(props);
        this.state = {
            node: props.node,
            rating: null,
            canMakeChanges: false,
            token: null
        }
        console.log(props.node);
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

            if(userHash != this.props.node.userHash)
                this.setState({canMakeChanges: true})

            return decoded;
        }else{
            // Token doesn't exist or not valid
            localStorage.removeItem('authorization');
            window.location.assign(window.location.origin);
            window.location = '/';
        }
    }

    GetRatings = async () => {
        axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');

        await axios.post("https://localhost:7010/Rate/getRating?nodeID=" + this.state.node.nodeID)
        .then(response => {
            this.setState({rating: response.data})
        })
        .catch((err => {

        }))
    }

    SetRating = async (rating) => {
        axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');
        console.log(this.state.node);
        await axios.post("https://localhost:7010/Rate/rateNode?nodeID=" + this.state.node.nodeID + "&rating=" + rating)
        .then(response => {
            console.log("rated");
            return true;
        })
        .catch(err => {
            console.log(err.response);
            return false;
        })
    }


   render() {
    
    const renderTitle = (
        <div>
            {this.state.node.nodeTitle}
        </div>
    )

    const renderSummary = (
        <div>
            {this.state.node.summary}
        </div>
    )

    const renderTags = (
        <div>
            
        </div>
    )

    const renderRatings = (
        <div>
            {this.state.token && 
                <Rating rating={this.state.rating} SetRating={this.SetRating} IsEnabled = {this.state.canMakeChanges}/>
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
                    <div className="node-rating-container">
                        {renderRatings}
                    </div>
                    <div className="node-summary-container">
                        {renderSummary}
                    </div>
               </div>          
            </div>
        </div>
    )
   }
}

export default NodeView;
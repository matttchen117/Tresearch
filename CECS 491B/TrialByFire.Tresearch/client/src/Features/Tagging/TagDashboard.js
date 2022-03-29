import React, { Component } from "react";
import {useNavigate} from 'react-router-dom';
import { ContextMenu, MenuItem, ContextMenuTrigger } from "react-contextmenu";
import axios, {AxiosResponse, AxiosError} from 'axios';

import "./TagDashboard.css";
import Button from "../../UI/Button/ButtonComponent";

class TagDashboard extends Component  {
    Data = [
        {name: "acting", count: 10},
        {name: "art", count: 1},
        {name: "agile", count: 22},
        {name: "cooking", count: 30},
        {name: "tennis", count:22},
        {name: "yoga", count: 11}
    ];

    onSubmitHandler = (e) => {
        
    }

    handleClick = (e, data) => {
        console.log(data.item);
    }

    render() {   
        const renderForm = (
            <div className = "tag-dashboard-form-container">
                <form className = "tag-dashboard-form">
                    <input type="text" className = "tag-dashboard-input"/>
                </form>
            </div>
        )
        
        const renderTable = (
            <div className="tag-dashboard-table-container">
                <table className = "tag-dashboard-table">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Count</th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.Data.map(item =>{
                            return(
                                <tr onClick={() => this.onSubmitHandler(item)}>
                                    <td>{item.name}</td>
                                    <td>{item.count}</td>
                                </tr>
                            );
                        })}
                    </tbody>
                </table>
            </div>
        );

        return (
            <div className="tag-dashboard-wrapper">
                <div>
                    <ContextMenuTrigger id = "tag-dashboard-context-menu">
                        <div className="trigger-div">
                           {renderTable}
                        </div>
                    </ContextMenuTrigger>
                    
                    <ContextMenu id="tag-dashboard-context-menu">
                        <MenuItem data={{item: 'item 1'}} onClick={this.handleClick} className = "tag-menu-item"> Edit </MenuItem>
                        <MenuItem divider/>
                        <MenuItem data={{item: 'item 2'}} onClick={this.handleClick} className = "tag-menu-item"> Delete </MenuItem>
                    </ContextMenu>
                </div>
                {renderForm}
                
                
            </div>
        );
    }
}

export default(TagDashboard);
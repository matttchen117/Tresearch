import React from "react";
import axios from 'axios';
import "./AdminPortal.css";

function AdminPortal() {

    const handleLogout = (e) => {
        e.preventDefault();
        axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');
        axios.post('https://trialbyfiretresearchwebapi.azurewebsites.net/Logout/logout', {})
        .then(response => {
            
        }).catch(err => {
              
        })
        sessionStorage.removeItem('authorization');
        window.location = '/';
    }

    const renderSideMenu = (
        <div className = "side-menu-container">
            <ul className = "side-menu">
                <li className = "side-menu-item">
                    <a href="/Admin/TagDashboard" className = "side-menu-link">Tag Dashboard</a>
                </li>
                <li className = "side-menu-item">
                    <a href="/Admin/UsageAnalysisDashboard" className = "side-menu-link">Usage Analysis Dashboard</a>
                </li>
                <li className = "side-menu-item">
                    <a href="/Admin/UserManagement" className = "side-menu-link">User Management</a>
                </li>
                <li className = "side-menu-item">
                    <a href="/Admin/FAQ" className = "side-menu-link">Admin FAQ</a>
                </li>
                <li className = "side-menu-item">
                    <a href="/Settings" className = "side-menu-link">Settings</a>
                </li>
                <li className = "side-menu-item">
                    <a href="" className = "side-menu-link" onClick = {handleLogout}>Logout</a>
                </li>
            </ul>
        </div>
    );

    return (
        <div className="admin-portal-wrapper"> 
            <div className = "side-menu-wrapper">
                {renderSideMenu}
            </div>
        </div>
    );
}

export default AdminPortal;

import React from "react";
import "./AdminPortal.css";

function AdminPortal() {
    const renderSideMenu = (
        <div className = "side-menu-container">
            <ul className = "side-menu">
                <li className = "side-menu-item">
                    <a href="/Admin/TagDashboard" className = "side-menu-link">Tag Dashboard</a>
                </li>
                <li className = "side-menu-item">
                    <a href="/Admin/TagDashboard" className = "side-menu-link">Usage Analysis Dashboard</a>
                </li>
                <li className = "side-menu-item">
                    <a href="/Admin/TagDashboard" className = "side-menu-link">User Management</a>
                </li>
                <li className = "side-menu-item">
                    <a href="/Settings" className = "side-menu-link">Settings</a>
                </li>
                <li className = "side-menu-item">
                    <a href="/Settings" className = "side-menu-link">Logout</a>
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

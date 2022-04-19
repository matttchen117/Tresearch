import React, { useCallback, useState, useEffect } from "react";
import NavBar from "../../UI/Navigation/NavBar";
import "./FAQ.css";


import tagContextMenuPNG from './TreeContextMenu.png';
import tagPNG from './tag.png';

function FAQ() {
    const renderRateFAQ = (
        <div className = "feature-faq-container">
            <h3>Rating</h3>
            <h4>How do I rate a node?</h4>
        </div>
    )

    const renderTagFAQ = (
        <div className = "feature-faq-container">
            <h3>Tagging</h3>
            <h4>How do I add a tag?</h4>
            <p>Create a tag by right clicking the node you want to add, then select edit tags in the context menu.</p>
            <img src = {tagContextMenuPNG}></img>
            <h4>How do I add tags to multiple nodes?</h4>
            <p>Enter shift and left click on all the nodes you want to add, then right click on your final node. Select edit tags in the context menu. 
                The tags shown are shared tags among the group of nodes selected. Any changes made will affect all nodes.
            </p>
            <h4>What tags can I add?</h4>
            <p>
                Tresearch provides a message bank with a large amount of tags that users can select from
                To find a tag, enter the tag you wish to add in the tag search bar.
            </p>
            <h4>Why should I add a tag?</h4>
            <p>Tagging provides a visual description of your node. This is useful for categorizing nodes as well as
                allows others to find your nodes easier.
            </p>
            <h4>How do I delete a tag</h4>
            <p>Simply click on the tag you wish to delete.</p>
            <img src = {tagPNG}></img>
        </div>
    )

    const renderFAQ = (
        <div className = "FAQ-container">
            <h1>FAQ</h1>
                {renderRateFAQ}
                {renderTagFAQ}
        </div>
    );

   
  return (
    <div className="FAQ-wrapper"> 
        {<NavBar/>}
        <div className="FAQ-content-wrappers">
           {renderFAQ}
        </div>
    </div>
  );
}

export default FAQ;

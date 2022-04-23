# Trial By Fire - Tresearch
## Table Of Contents
--------------------------------
- [What is Tresearch?](#what-is-tresearch)
- [Contributors](#contributors)
- [Demo](#demo)
- [Installation](#installation)
- [Features](#features)
  - [Tagging](#tagging)
- [Built With](#built-with)
- [FAQ](#faq)
  - [Tagging](#tagging-1)
- [User Manual](#user-manual)
### What is Tresearch?
--------------------------------
Tresearch is an interactive mind mapping tool for documenting users' learning journey.  Users can create their own knowledge tree(s) to document what they are currently learning and have learned throughout their life. Knowledge trees are made up of nodes that consist of a title,  description/summary, and optional tag. Branches that come off of nodes point to nodes that utilize or require knowledge from the previous node. Users can view any other user's public knowledge trees to see what they are learning or have learned and how. If a user finds another user’s tree or a portion of their tree to be particularly useful or helpful, they can rate a particular node or section of the tree and they can also copy that section over to their own tree. Users can add additional information to their public profiles such as what they are currently learning, and where they are working/what they currently do or have done. Users can utilize a search function in order to find people whose trees contain a certain topic or keyword/phrase, and can also utilize a filter to narrow searches by users who are learning said topic, are using said topic (i.e. in their work or otherwise), by rating, and by tags.

### Contributors
--------------------------------
- Ian Ho-Sing-Loy
- Jessie Lazo
- Matthew Chen (Team Leader)
- Pammy Poor
  - Email: pammypoor@gmail.com
  - Github: https://github.com/pammypoor
- Viet Nguyen

### Demo
--------------------------------
Live demo: https://trialbyfiretresearch.azurewebsites.net/

### Installation
--------------------------------
To run this application, Git, Node.js and npm must be installed. 

```
# Clone Repository. 
$ git clone https://github.com/Drakat7/Tresearch

# Navigate to web api
$ cd "CECS 491B"\TrialByFire.Tresearch\TrialByFire.Tresearch.WebApi

# Navigate to front end client
$ cd "CECS 491B"\TrialByFire.Tresearch\client

# Install dependencies
$ npm install

# run the client
$ npm run start

```

### Features
--------------------------------
#### Tagging
- ##### Description
  - Add or remove from a bank of tags to organize and categorize nodes. 
  - Search tags using an autocomplete drop-down search bar
  - Administrators can create and delete tags from word bank
- #### Design
  - [Business Requirement Documnet](https://github.com/Drakat7/Tresearch/blob/pammy/Documents/BRD.pdf)
    - Scope
      - Adding and Removing tags from one or more nodes that are owned by the user simultaneously
    - Non-Functional Requirements
      - User tree data updated within 5 seconds
      - User tree page visually udpated within 5 seconds
      - Feature will be accessible 90% of the time
      - Text is a minimum of 16 pixels
      - Errors and Success Logged
      - Feature is only visibale and users by Authenticated and Authorized users on tree page
  - [Low Level Design](https://github.com/Drakat7/Tresearch/tree/main/Sequence%20Diagrams/Tagging%20Diagrams)
    - [Add Tag To Node](https://github.com/Drakat7/Tresearch/tree/main/Sequence%20Diagrams/Tagging%20Diagrams/Add%20Node%20Tag)
    - [Create Tag](https://github.com/Drakat7/Tresearch/tree/main/Sequence%20Diagrams/Tagging%20Diagrams/Create%20Tag)
    - [Delete Tag](https://github.com/Drakat7/Tresearch/tree/main/Sequence%20Diagrams/Tagging%20Diagrams/Delete%20Tag)
    - [Get Node Tags](https://github.com/Drakat7/Tresearch/tree/main/Sequence%20Diagrams/Tagging%20Diagrams/Get%20Node%20Tags) 
    - [Get Tags](https://github.com/Drakat7/Tresearch/tree/main/Sequence%20Diagrams/Tagging%20Diagrams/Get%20Tags)
    - [Remove Tag From Node](https://github.com/Drakat7/Tresearch/tree/main/Sequence%20Diagrams/Tagging%20Diagrams/Remove%20Node%20Tag)
  - [Test Plan](https://github.com/Drakat7/Tresearch/blob/main/Documents/Test%20Plan.pdf)
### Built With
--------------------------------
#### Programming Languages
- C#
- CSS/HTML
- Javascript
- SQL
#### Back-End
- [7ZIP](https://www.7-zip.org/) - Open source compression library
- [Azure Cloud Hosting](https://azure.microsoft.com/en-us/services/cloud-services/#pricing) - Cloud host for WebAPI and Front-End Client.
- [Azure Sql Database](https://azure.microsoft.com/en-us/products/azure-sql/database/) - Azure persistent data store
- [Dapper](https://github.com/DapperLib/Dapper) - Object relational mapping library
- [.NET 6.0](https://dotnet.microsoft.com/en-us/learn/dotnet/what-is-dotnet) - Backend framework
- [SendGrid API](https://sendgrid.com/) - Email API used to send confirmation and recovery links.
- [xUnit](https://xunit.net/) - Testing tool for .NET Framework
#### Front-End
- [Axios](https://axios-http.com/docs/intro) - HTTP Client for node.js and the browser. Used to make requests to backend.
- [Nivo](https://nivo.rocks/) - React data visualization component.
- [PBKDF2](https://www.npmjs.com/package/pbkdf2) - Library proviiding PBKDF2 hashing algorithm.
- [React.js](https://reactjs.org/) - Javascript library used for building user interfaces
- [react-contextmenu](https://www.npmjs.com/package/react-contextmenu) - React context menu component
- [react-router-dom](https://v5.reactrouter.com/web/guides/quick-start) - React routing library with DOM aware components
- [react-select](https://react-select.com/home) - multiselect, dropdown, autocomplete search input 
#### IDE
- VS Code 
- Visual Studio 2022
#### Documentation Technologies
- [Diagrams.net](https://www.diagrams.net/) - Used to create low level design documents
- [Draw.chat](https://draw.chat/) - Used to create visuals for decision trees
- [Github](https://github.com/) - Version control system managing documents and code
#### Communication Technologies
- [Discord](https://discord.com/) - Meeting and messaging service for group communication.
- [Microsoft Outlook](https://outlook.live.com/owa/) - Communication between instructor and group.

### FAQ
--------------------------------
#### Tagging
- What are tags?
  - Tags provide a visual description of a node. 
  - Tags are useful when categorizing nodes as well as allows users to find public nodes easier.
- Adding a node tag
  - Right click on node and select "Edit Tags" in the context menu. Select tag from search bar. 
- Adding tag to multiple nodes
  - Shift and left click on all the nodes to tag and then right click on your final node. Select edit tags in the context menu. Select tag from search bar
  - The tags displayed are shared tags among the groups of nodes selected. Any changes made will affect all nodes selected.
- Searching for a tag
  - Click on the search bar and search for the tag or scroll through the list of tags available
  - Search bar will show autocomplete tags
- Remove a tag from node(s)
  - Click on the tag.
- Adding a tag to the tag bank
  - As an administrator, navigate to the tag dashboard. Enter the tag name in the input field and click "Add Tag".
- Deleting a tag from the tag bank
  - As an administrator, navigate to to the tag dashboard. Find the tag in the tag table and click the delete button located next to the name.

### User Manual
--------------------------------
#### Tagging

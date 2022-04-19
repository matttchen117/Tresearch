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

# Navigaate to front end client
$ cd "CECS 491B"\TrialByFire.Tresearch\client

# Install dependencies
$ npm install

# run the client
$ npm run start

```

### Features
--------------------------------
#### Tagging
- Add or remove from a bank of tags to organize and categorize nodes. 
- Search tags using an autocomplete drop-down search bar
- Administrators can create and delete tags from word bank

### Built With
--------------------------------
#### Back-End
- [7ZIP](https://www.7-zip.org/) - Open source compression library
- [Azure Cloud Hosting](https://azure.microsoft.com/en-us/services/cloud-services/#pricing) - Cloud host for WebAPI and Front-End Client.
- [Azure Sql Database](https://azure.microsoft.com/en-us/products/azure-sql/database/) - Azure persistent data store
- [.NET Frmaework](https://dotnet.microsoft.com/en-us/learn/dotnet/what-is-dotnet) - Backend framework
- [SendGrid API](https://sendgrid.com/) - Email API used to send confirmation and recovery links.
- [xUnit](https://xunit.net/) - Testing tool for .NET Framework
#### Front-End
- [Axios](https://axios-http.com/docs/intro) - HTTP Client for node.js and the browser. Used to make requests to backend.
- [Nivo](https://nivo.rocks/) - React data visualization component.
- [PBKDF2](https://www.npmjs.com/package/pbkdf2) - Library proviiding PBKDF2 hashing algorithm.
- [React.js](https://reactjs.org/) - Javascript library used for building user interfaces
- [react-contextmenu](https://www.npmjs.com/package/react-contextmenu) - React context menu component
- [react-router-dom](https://v5.reactrouter.com/web/guides/quick-start)- React routing library with DOM aware components
### FAQ
--------------------------------
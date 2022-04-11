import axios from "axios";
import React from "react";
import Select from 'react-select';

class SearchTagBar extends React.Component  {
    constructor(props){
        super(props)
        this.state = {
            selectionTags: [], 
            selected: ''
        }
    }

    componentDidMount(){
        this.getTags()
        
    }

    handleChange = (value) =>{
        
    }


    async getTags() {
        const res = await axios.get("https://localhost:7010/Tag/taglist")
        const data = res.data

        const options = data.map(d=> ({
            "value": d,
            "label": d
        }))
        this.setState({selectionTags: options})
    }

    render() {
        return(
            <div>
                
            </div>
        )
    }

}

export default SearchTagBar;
import React from "react"
import { ContextMenu, ContextMenuTrigger, MenuItem, showMenu } from "react-contextmenu";

class EditNodePropertiesForm extends React.PureComponent {
    constructor(props){
        super(props);

        this.state = {
            node: props.node,
            font: props.node.font,
            shape: props.node.shape,
            color: props.node.color,    
            
            fontList: ['Bold', 'Italic'],
            shapeList: ['Circle', 'Square', 'Star'],
            colorList: ['Red', 'Blue', 'Green']
        };

        this.handleFont = this.handleFont.bind(this);
    }

    handleFont = (e) => {
        this.setState({font: e.target.font})
    }

    render() {
       const renderForm = (
            <div className="node-properties-container">
                <form className="node-properties-form">
                    <table>
                        <tr>
                            <th>Node Font</th>
                            <th>
                                <select>
                                    {this.state.fontList.map(lfont => (
                                        <option key={lfont} font={lfont}>
                                            {lfont}
                                        </option>
                                    ))}
                                </select>
                            </th>
                        </tr>
                        <tr>
                            <th>Node Shape</th>
                            <th>

                            </th>
                        </tr>
                        <tr>
                            <th>Node Color</th>
                            <th>

                            </th>
                        </tr>
                    </table>
                </form>
            </div>
        );

        return (
            <div className="node-properties-wrapper">
                <div className="button-container">
                    <h1 className="">Node Properties</h1> 
                </div>
                {renderForm}
            </div>
        )
    }
}

export default(EditNodePropertiesForm)
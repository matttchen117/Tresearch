import React, {useState} from "react";
import './Rating.css';

class Rating extends React.PureComponent{
  constructor(props) {
    super(props);

    this.state = {
      rating: props.rating,
      hover: 0,
      size: [0, 1, 2, 3, 4], 
      isAuthorized: props.IsEnabled
    }

    console.log(props.IsEnabled);

  }
  
  render() {
    const renderRating = (
      <div className="star-container">
        {this.state.size.map(index => {
          return (
            <button
              type="button"
              key={index}
              className={index <= (this.state.hover || this.state.rating) ? "on" : "off"}
              onClick={() => { this.setState({rating: index}); this.props.SetRating(index);} }
              onMouseEnter={() => this.setState({hover: index})}
              onMouseLeave={() => this.setState({hover: this.state.rating})}
              disabled ={!this.state.isAuthorized}
            >
              <span className="star">&#9733;</span>
            </button>
          );
        })}
      </div>
    );
    return(
      <div className="rating-wrapper">
          {renderRating}
      </div>
    )
  }
}

export default Rating;
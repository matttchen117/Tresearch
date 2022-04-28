import React from 'react';
import NavBar from '../../UI/Navigation/NavBar';

const Error404 = () => {
        return (
            <div>
                {<NavBar/>}
                <h1>404 - Page doesn't exist</h1>
            </div>
        );
}

export default Error404;
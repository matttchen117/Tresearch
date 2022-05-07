import { render, screen } from '@testing-library/react';
import Tag from '../../UI/Tag/Tag';
import TagDashboard from './TagDashboard';

test('Create Tag: Unauthenticated', () => {
    // Arrange
    let tagName = "Tag Dashboard Front End Test"
    render(<TagDashboard/>);

    //Act

    //Assert
})

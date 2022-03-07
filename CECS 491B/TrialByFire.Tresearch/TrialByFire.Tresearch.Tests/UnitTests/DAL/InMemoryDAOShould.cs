using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Tests.UnitTests.DAL
{
    public class InMemoryDAOShould : InMemoryTestDependencies
    {
        public InMemoryDAOShould() : base()
        {
            
        }

        [Theory]
        [InlineData(2000, 2, 1, 1, "success")]
        [InlineData(2000, 1, 1, 1, "Fail - Created Nodes Already Exists")]
        public void CreatedNodesCreatedWorks(int year, int month, int day, int count, string expected) {

            // Arrange
            NodesCreated nodesCreated = new NodesCreated(new DateTime(year, month, day), count);

            // Act
            string result = sqlDAO.CreateNodesCreated(nodesCreated);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        //[InlineData(2000, 1, 30, )]
        [InlineData()]
        public void GetCreatedNodesWorks() { 
        
        }

        [Theory]
        [InlineData(2000, 1, 1, 2, "success")]
        [InlineData(2000, 2, 1, 1, "Fail - Created Nodes to Update Does Not Exist in Database")]
        public void UpdateCreatedNodesWorks(int year, int month, int day, int count, string expected) { 
        
            // Arrange
            NodesCreated nodesCreated = new NodesCreated(new DateTime (year, month, day), count);

            // Act
            string result = sqlDAO.UpdateNodesCreated(nodesCreated);

            // Assert
            Assert.Equal(expected , result);

        }

        [Theory]
        [InlineData(2000, 2, 1, 1, "success")]
        [InlineData(2000, 1, 1, 1, "Fail - Daily Logins Already Exists")]
        public void CreateDailyLoginWorks(int year, int month, int day, int count, string expected) {

            // Arrange
            DailyLogin dailyLogin = new DailyLogin(new DateTime(year, month, day), count);

            // Act
            string result = sqlDAO.CreateDailyLogin(dailyLogin);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData()]
        [InlineData()]
        public void GetDailyLoginWorks() { 
        
        }

        [Theory]
        [InlineData(2000, 1, 1, 2, "success")]
        [InlineData(2000, 2, 1, 1, "Fail - Daily Logins to Update Does Not Exist in Database")]
        public void UpdateDailyLoginWorks(int year, int month, int day, int count, string expected) { 
            // Arrange
            DailyLogin dailyLogin = new DailyLogin(new DateTime (year, month, day), count);

            // Act
            string result = sqlDAO.UpdateDailyLogin(dailyLogin);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(2000, 2, 1, 1, "test1", "success")]
        [InlineData(2000, 1, 1, 1, "test0", "Fail - Top Search Already Exists")]
        public void CreateTopSearchWorks(int year, int month, int day, int count, string searchString, string expected)
        {
            // Arrange
            TopSearch topSearch = new TopSearch(new DateTime(year, month, day), searchString, count);

            // Act
            string result = sqlDAO.CreateTopSearch(topSearch);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData()]
        [InlineData()]
        public void GetTopSearchWorks()
        {

        }

        [Theory]
        [InlineData(2000, 1, 1, 2, "test1", "success")]
        [InlineData(2000, 2, 1, 1, "test0", "Fail - Top Search to Update Does Not Exist")]
        public void UpdateTopSearchWorks(int year, int month, int day, int count, string searchString, string expected)
        {
            // Arrange
            TopSearch topSearch = new TopSearch(new DateTime(year, month, day), searchString, count);

            // Act
            string result = sqlDAO.UpdateTopSearch(topSearch);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(2000, 2, 1, 1, "success")]
        [InlineData(2000, 1, 1, 1, "Fail - Daily Registration Already Exists")]
        public void CreateDailyRegistrationWorks(int year, int month, int day, int count, string expected)
        {
            // Arrange
            DailyRegistration dailyRegistration = new DailyRegistration(new DateTime(year, month, day), count);
            
            // Act
            string result = sqlDAO.CreateDailyRegistration(dailyRegistration);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData()]
        [InlineData()]
        public void GetDailyRegistrationWorks()
        {

        }

        [Theory]
        [InlineData(2000, 1, 1, 2, "success")]
        [InlineData(2000, 2, 1, 1, "Fail - Daily Registration to Update Does Not Exist")]
        public void UpdateDailyRegistrationWorks(int year, int month, int day, int count, string expected)
        {
            // Arrange
            DailyRegistration dailyRegistration = new DailyRegistration(new DateTime(year, month, day), count);

            // Act
            string result = sqlDAO.UpdateDailyRegistration(dailyRegistration);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using TrialByFire.Tresearch.Models.Implementations;
<<<<<<< HEAD
using TrialByFire.Tresearch.DAL.Contracts;
using Microsoft.Extensions.DependencyInjection;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Tests.UnitTests.DAL
{
    public class InMemoryDAOShould : TestBaseClass
    {
        private ISqlDAO _sqlDAO;
        private InMemoryDatabase _inMemoryDatabase;
        public InMemoryDAOShould() : base(new string[] { })
        {
            TestServices.AddScoped<ISqlDAO, InMemorySqlDAO>();
            TestProvider = TestServices.BuildServiceProvider();
            _sqlDAO = TestProvider.GetService<ISqlDAO>();
            _inMemoryDatabase = new InMemoryDatabase();
=======

namespace TrialByFire.Tresearch.Tests.UnitTests.DAL
{
    public class InMemoryDAOShould : InMemoryTestDependencies
    {
        public InMemoryDAOShould() : base()
        {

>>>>>>> Working
        }

        [Theory]
        [InlineData(2000, 2, 1, 1, "success")]
        [InlineData(2000, 1, 1, 1, "Fail - Created Nodes Already Exists")]
<<<<<<< HEAD
        public void CreatedNodesCreatedWorks(int year, int month, int day, int count, string expected) {
=======
        public void CreatedNodesCreatedWorks(int year, int month, int day, int count, string expected)
        {
>>>>>>> Working

            // Arrange
            NodesCreated nodesCreated = new NodesCreated(new DateTime(year, month, day), count);

            // Act
<<<<<<< HEAD
            string result = _sqlDAO.CreateNodesCreated(nodesCreated);
=======
            string result = sqlDAO.CreateNodesCreated(nodesCreated);
>>>>>>> Working

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        //[InlineData(2000, 1, 30, )]
        [InlineData()]
<<<<<<< HEAD
        public void GetCreatedNodesWorks() { 
        
=======
        public void GetCreatedNodesWorks()
        {

>>>>>>> Working
        }

        [Theory]
        [InlineData(2000, 1, 1, 2, "success")]
        [InlineData(2000, 2, 1, 1, "Fail - Created Nodes to Update Does Not Exist in Database")]
<<<<<<< HEAD
        public void UpdateCreatedNodesWorks(int year, int month, int day, int count, string expected) { 
        
            // Arrange
            NodesCreated nodesCreated = new NodesCreated(new DateTime (year, month, day), count);

            // Act
            string result = _sqlDAO.UpdateNodesCreated(nodesCreated);

            // Assert
            Assert.Equal(expected , result);
=======
        public void UpdateCreatedNodesWorks(int year, int month, int day, int count, string expected)
        {

            // Arrange
            NodesCreated nodesCreated = new NodesCreated(new DateTime(year, month, day), count);

            // Act
            string result = sqlDAO.UpdateNodesCreated(nodesCreated);

            // Assert
            Assert.Equal(expected, result);
>>>>>>> Working

        }

        [Theory]
        [InlineData(2000, 2, 1, 1, "success")]
        [InlineData(2000, 1, 1, 1, "Fail - Daily Logins Already Exists")]
<<<<<<< HEAD
        public void CreateDailyLoginWorks(int year, int month, int day, int count, string expected) {
=======
        public void CreateDailyLoginWorks(int year, int month, int day, int count, string expected)
        {
>>>>>>> Working

            // Arrange
            DailyLogin dailyLogin = new DailyLogin(new DateTime(year, month, day), count);

            // Act
<<<<<<< HEAD
            string result = _sqlDAO.CreateDailyLogin(dailyLogin);
=======
            string result = sqlDAO.CreateDailyLogin(dailyLogin);
>>>>>>> Working

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData()]
        [InlineData()]
<<<<<<< HEAD
        public void GetDailyLoginWorks() { 
        
=======
        public void GetDailyLoginWorks()
        {

>>>>>>> Working
        }

        [Theory]
        [InlineData(2000, 1, 1, 2, "success")]
        [InlineData(2000, 2, 1, 1, "Fail - Daily Logins to Update Does Not Exist in Database")]
<<<<<<< HEAD
        public void UpdateDailyLoginWorks(int year, int month, int day, int count, string expected) { 
            // Arrange
            DailyLogin dailyLogin = new DailyLogin(new DateTime (year, month, day), count);

            // Act
            string result = _sqlDAO.UpdateDailyLogin(dailyLogin);
=======
        public void UpdateDailyLoginWorks(int year, int month, int day, int count, string expected)
        {
            // Arrange
            DailyLogin dailyLogin = new DailyLogin(new DateTime(year, month, day), count);

            // Act
            string result = sqlDAO.UpdateDailyLogin(dailyLogin);
>>>>>>> Working

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
<<<<<<< HEAD
            string result = _sqlDAO.CreateTopSearch(topSearch);
=======
            string result = sqlDAO.CreateTopSearch(topSearch);
>>>>>>> Working

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
<<<<<<< HEAD
            string result = _sqlDAO.UpdateTopSearch(topSearch);
=======
            string result = sqlDAO.UpdateTopSearch(topSearch);
>>>>>>> Working

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
<<<<<<< HEAD
            
            // Act
            string result = _sqlDAO.CreateDailyRegistration(dailyRegistration);
=======

            // Act
            string result = sqlDAO.CreateDailyRegistration(dailyRegistration);
>>>>>>> Working

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
<<<<<<< HEAD
            string result = _sqlDAO.UpdateDailyRegistration(dailyRegistration);
=======
            string result = sqlDAO.UpdateDailyRegistration(dailyRegistration);
>>>>>>> Working

            // Assert
            Assert.Equal(expected, result);
        }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> Working

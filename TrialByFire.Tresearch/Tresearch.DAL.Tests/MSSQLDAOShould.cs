using TrialByFire.Tresearch.DAL;
using TrialByFire.Tresearch.DomainModels;
using Xunit;

namespace TrialByFire.DAL.Tests
{
    public class MSSQLDAOShould
    {
        [Fact]
        public void GetTheAccount()
        {
            // Triple A Format

            // Arrange
            string sqlConnectionString = @".\Server=LAPTOP-6SF4R1QG;Initial Catalog=TrialByFire.Tresearch; Integrated Security=true";
            MSSQLDAO mssqlDAO = new MSSQLDAO(sqlConnectionString);
            var expected = new Account("bob@gmail.com", "abcdef123456", "User");

            // Act
            var actual = mssqlDAO.GetAccount("bob@gmail.com", "abcdef123456");

            // Assert
            Assert.True(expected.Equals(actual));
        }

        [Fact]
        public void CreateTheAccount()
        {
            // Triple A Format

            // Arrange
            string sqlConnectionString = @".\Server=LAPTOP-6SF4R1QG;Initial Catalog=TrialByFire.Tresearch; Integrated Security=true";
            MSSQLDAO mssqlDAO = new MSSQLDAO(sqlConnectionString);
            Account account = new Account("bob@gmail.com", "abcdef123456", "User");

            // Act
            var actual = mssqlDAO.CreateAccount(account);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void UpdateTheAccount()
        {
            // Triple A Format

            // Arrange
            string sqlConnectionString = @".\Server=LAPTOP-6SF4R1QG;Initial Catalog=TrialByFire.Tresearch; Integrated Security=true";
            MSSQLDAO mssqlDAO = new MSSQLDAO(sqlConnectionString);


            // Act
            var actual = mssqlDAO.UpdateAccount("bob1@gmail.com", "123456abcdef", "larry@gmail.com", "System Admin");

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void DeleteTheAccount()
        {
            // Triple A Format

            // Arrange
            string sqlConnectionString = @".\Server=LAPTOP-6SF4R1QG;Initial Catalog=TrialByFire.Tresearch; Integrated Security=true";
            MSSQLDAO mssqlDAO = new MSSQLDAO(sqlConnectionString);


            // Act
            var actual = mssqlDAO.DeleteAccount("bob@gmail.com");

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void DisableTheAccount()
        {
            // Triple A Format

            // Arrange
            string sqlConnectionString = @".\Server=LAPTOP-6SF4R1QG;Initial Catalog=TrialByFire.Tresearch; Integrated Security=true";
            MSSQLDAO mssqlDAO = new MSSQLDAO(sqlConnectionString);


            // Act
            var actual = mssqlDAO.DisableAccount("larry@gmail.com");

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void EnableTheAccount()
        {
            // Triple A Format

            // Arrange
            string sqlConnectionString = @".\Server=LAPTOP-6SF4R1QG;Initial Catalog=TrialByFire.Tresearch; Integrated Security=true";
            MSSQLDAO mssqlDAO = new MSSQLDAO(sqlConnectionString);


            // Act
            var actual = mssqlDAO.EnableAccount("larry@gmail.com", "larry@gmail.com");

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void StoreThelog()
        {
            // Triple A Format

            // Arrange
            string sqlConnectionString = @".\Server=LAPTOP-6SF4R1QG;Initial Catalog=TrialByFire.Tresearch; Integrated Security=true";
            MSSQLDAO mssqlDAO = new MSSQLDAO(sqlConnectionString);
            Log log = new Log(System.DateTime.Now, "Info", "larry@gmail.com", "DataStore", "This is a test.");

            // Act
            var actual = mssqlDAO.StoreLog(log);

            // Assert
            Assert.True(actual);
        }

        public void ArchiveTheLogs()
        {
            // Triple A Format

            // Arrange
            string sqlConnectionString = @"Server=.\LAPTOP-6SF4R1QG;Initial Catalog=TrialByFire.Tresearch; Integrated Security=true";
            string filePath = @"C:\Users\Matthew Chen\Desktop\Archive Test\";
            string destination = @"C:\Users\Matthew Chen\Desktop\Archive Test\";
            MSSQLDAO mssqlDAO = new MSSQLDAO(sqlConnectionString);

            // Act
            var actual = mssqlDAO.Archive();

            // Assert
            Assert.True(actual);
        }
    }
}
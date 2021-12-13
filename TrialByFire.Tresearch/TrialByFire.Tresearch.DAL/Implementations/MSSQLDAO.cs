using System.Collections.Generic;
using TrialByFire.Tresearch.DomainModels;
using Dapper;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.IO.Compression;

namespace TrialByFire.Tresearch.DAL
{
    public class MSSQLDAO
    {
        public string SqlConnectionString { get; set; }
        public string FilePath { get; set; }
        public string Destination { get; set; }

        public MSSQLDAO()
        {
        }

        public MSSQLDAO(string sqlConnectionString)
        {
            SqlConnectionString = sqlConnectionString;
        }

        public MSSQLDAO(string sqlConnectionString, string filePath, string destination)
        {
            SqlConnectionString = sqlConnectionString;
            FilePath = filePath;
            Destination = destination;
        }

        public Account GetAccount(string username, string passphrase)
        {
            Account userAccount;
            using (var connection = new SqlConnection(SqlConnectionString))
            {
                var readQuery = "SELECT * FROM Accounts WHERE Username = @Username and Passphrase = @Passphrase and Status = 'Enabled'";
                userAccount = connection.QuerySingle<Account>(readQuery, new { Username = username, Passphrase = passphrase });
            }
            return userAccount;
        }

        public bool CreateAccount(Account account)
        {
            int affectedRows;
            using (var connection = new SqlConnection(SqlConnectionString))
            {
                var readQuery = "SELECT COUNT(*) FROM Accounts WHERE Email = @Email";
                var accounts = connection.ExecuteScalar<int>(readQuery, new { Email = account.Email });
                if (accounts > 0)
                {
                    account.Username = account.Username.Insert(account.Username.IndexOf('@'), accounts.ToString());
                }
                var insertQuery = "INSERT INTO Accounts (Email, Username, Passphrase, AuthorizationLevel, Status) " +
                    "VALUES (@Email, @Username, @Passphrase, @AuthorizationLevel, @Status)";
                affectedRows = connection.Execute(insertQuery, account);
            }
            if (affectedRows == 1)
            {
                return true;
            }
            return false;
        }

        public bool UpdateAccount(string username, string newPassPhrase, string newEmail, string newAuthorizationLevel)
        {
            string newUsername = "";
            int affectedRows;
            using (var connection = new SqlConnection(SqlConnectionString))
            {
                string updateQuery = "UPDATE Accounts SET ";
                if (newPassPhrase != null)
                {
                    updateQuery += "PassPhrase = '" + newPassPhrase + "', ";
                }
                if (newEmail != null)
                {
                    var readQuery = "SELECT COUNT(*) FROM Accounts WHERE Email = @Email";
                    var accounts = connection.ExecuteScalar<int>(readQuery, new { Email = newEmail });
                    if (accounts > 0)
                    {
                        newUsername = newEmail.Insert(newEmail.IndexOf('@'), accounts.ToString());
                    }
                    else
                    {
                        newUsername = newEmail;
                    }
                    updateQuery += "Username = '" + newUsername + "', Email = '" + newEmail + "', ";
                }
                if (newAuthorizationLevel != null)
                {
                    updateQuery += "AuthorizationLevel = '" + newAuthorizationLevel + "', ";
                }
                updateQuery = updateQuery.Remove(updateQuery.LastIndexOf(','), 1);
                updateQuery += "WHERE Username = @Username";
                affectedRows = connection.Execute(updateQuery, new { Username = username });
            }
            if (affectedRows == 1)
            {
                return true;
            }
            return false;
        }

        public bool DeleteAccount(string username)
        {
            int affectedRows;
            using (var connection = new SqlConnection(SqlConnectionString))
            {
                var deleteQuery = "DELETE FROM Accounts WHERE Username = @Username";
                affectedRows = connection.Execute(deleteQuery, new { Username = username });
            }
            if (affectedRows == 1)
            {
                return true;
            }
            return false;
        }

        public bool DisableAccount(string username)
        {
            int affectedRows;
            using (var connection = new SqlConnection(SqlConnectionString))
            {
                var disableQuery = "UPDATE Accounts SET Status = 'Disabled' WHERE Username = @Username";
                affectedRows = connection.Execute(disableQuery, new { Username = username });
            }
            if (affectedRows == 1)
            {
                return true;
            }
            return false;
        }

        public bool EnableAccount(string username, string email)
        {
            int affectedRows;
            using (var connection = new SqlConnection(SqlConnectionString))
            {
                var disableQuery = "UPDATE Accounts SET Status = 'Enabled' WHERE Username = @Username and Email = @Email";
                affectedRows = connection.Execute(disableQuery, new { Username = username, Email = email });
            }
            if (affectedRows == 1)
            {
                return true;
            }
            return false;
        }

        public bool StoreLog(Log log)
        {
            int affectedRows;
            using (var connection = new SqlConnection(SqlConnectionString))
            {
                var insertQuery = "INSERT INTO Logs (Timestamp, Level, Username, Category, Description) VALUES (@Timestamp, @Level, @Username, @Category, @Description)";
                affectedRows = connection.Execute(insertQuery, log);
            }
            if (affectedRows == 1)
            {
                return true;
            }
            return false;
        }

        public bool Archive()
        {
            int affectedRows;
            using (var connection = new SqlConnection(SqlConnectionString))
            {
                var readQuery = "SELECT * FORM Logs WHERE Timestamp > @Timestamp";
                var logs = connection.Query<Log>(readQuery, new { Timestamp = System.DateTime.Now }).ToList();
                using(StreamWriter writer = new StreamWriter(FilePath))
                {
                    foreach (Log log in logs)
                    {
                        writer.WriteLine(log.ToString());
                    }
                }
                ZipFile.CreateFromDirectory(FilePath, Destination);
            }
            if (affectedRows == 1)
            {
                return true;
            }
            return false;
        }

    }
}
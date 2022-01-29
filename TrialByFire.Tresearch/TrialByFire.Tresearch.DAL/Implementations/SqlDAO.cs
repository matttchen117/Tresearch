using System.Collections.Generic;
using TrialByFire.Tresearch.DomainModels;
using Dapper;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Configuration;
using Z.Dapper.Plus;

namespace TrialByFire.Tresearch.DAL
{
    public class SqlDAO : ISqlDAO
    {
        public string SqlConnectionString { get; set; }
        public string FilePath { get; set; }
        public string Destination { get; set; }

        public SqlDAO()
        {
            SqlConnectionString = ConfigurationManager.AppSettings.Get("SqlConnectionString");
            FilePath = ConfigurationManager.AppSettings.Get("FilePath");
            Destination = ConfigurationManager.AppSettings.Get("Destination");
        }
        
        public SqlDAO(String s)
        {
            SqlConnectionString = s;
            FilePath = ConfigurationManager.AppSettings.Get("FilePath");
            Destination = ConfigurationManager.AppSettings.Get("Destination");
        }

        public Account GetAccount(Account account)
        {
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                {
                    var query = "SELECT * FROM Tresearch.Accounts WHERE Username = @Username and Passphrase = @Passphrase and Status = 'Enabled'";
                    var userAccount = connection.QuerySingle<Account>(query, new { Username = account.Username, Passphrase = account.Passphrase});
                    return userAccount;
                }
            }catch (Exception ex)
            {
                return account;
            }
        }

        public string CreateAccount(List<Account> accounts)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                {
                    //var readQuery = "SELECT COUNT(*) FROM Accounts WHERE Email = @Email";
                    //var accounts = connection.ExecuteScalar<int>(readQuery, new { Email = account.Email });
                    /**if (accounts > 0)
                    {
                        account.Username = account.Username.Insert(account.Username.IndexOf('@'), accounts.ToString());
                    }**/
                    foreach(Account acc in accounts)
                    {
                        var insertQuery = @"INSERT INTO Tresearch.Accounts (Email, Username, Passphrase, AuthorizationLevel) " +
                        "VALUES (@Email, @Username, @Passphrase, @AuthorizationLevel)";
                        affectedRows = connection.Execute(insertQuery, acc);
                        if (affectedRows == 1)
                        {
                            return "Success";
                        }
                        else
                        {
                            return "CreateAccount failure";
                        }
                    }
                    return "Success";//Not all paths lead had a return value this fixed it
                }             
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public string UpdateAccount(List<Account> accounts)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                {
                    foreach(Account acc in accounts)
                    {
                        string updateQuery = "UPDATE Accounts SET ";
                        affectedRows = connection.Execute(updateQuery);
                        if(affectedRows == 1)
                        {
                            return "Success";
                        }
                        else
                        {
                            return "UpdateAccount failure";
                        }
                    }
                    return "Success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string DeleteAccount(List<Account> accounts)
        {
            try
            {
                using (var connection = new SqlConnection(ConfigurationManager.AppSettings.Get("SqlConnectionString")))
                {
                    foreach( Account acc in accounts)
                    {
                        var deleteQuery = "DELETE FROM Accounts WHERE Username = @Username";
                        var affectedRows = connection.Execute(deleteQuery, new { Username = acc.Username });
                        if (affectedRows == 1)
                        {
                            return "Success";
                        }
                        else
                        {
                            return "DeleteAccount failure";
                        }
                    }
                    return "Success";
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public string DisableAccount(List<Account> accounts)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                {
                    foreach(Account acc in accounts)
                    {
                        var disableQuery = "UPDATE Accounts SET Status = 'Disabled' WHERE Username = @Username";
                        affectedRows = connection.Execute(disableQuery, new { Username = acc.Username });
                        if (affectedRows == 1)
                        {
                            return "Success";
                        }
                        else
                        {
                            return "DisableAccount failure";
                        }
                    }
                    return "Success";
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public string EnableAccount(List<Account> accounts)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                {
                    foreach(Account acc in accounts)
                    {
                        var enableQuery = "UPDATE Accounts SET Status = 'Enabled' WHERE Username = @Username and Email = @Email";
                        affectedRows = connection.Execute(enableQuery, new { Username = acc.Username, Email = acc.Email });
                        if (affectedRows == 1)
                        {
                            return "Success";
                        }
                        else
                        {
                            return "EnableAccount failure";
                        }
                    }
                    return "Success";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string StoreLog(Log log)
        {
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                {
                    string insertQuery = @"INSERT INTO Logs (Timestamp, Level, Username, Category, Description) VALUES (@Timestamp, @Level, @Username, @Category, @Description)";
                    var affectedRows = connection.Execute(insertQuery, log);
                    if (affectedRows == 1)
                    {
                        return "Success";
                    }
                    else
                    {
                        return "StoreLog failure";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string ArchiveLogs(DateTime now)
        {
            try
            {
                if (Directory.Exists(FilePath) && Directory.Exists(Destination))
                {
                    using (var connection = new SqlConnection(SqlConnectionString))
                    {
                        List<Log> _logs = GetLogsOlderThan30Days(now);
                        string readQuery = @"SELECT * FROM Logs WHERE Timestamp > @Timestamp";
                        var logs = connection.Execute(readQuery, new { TimeStamp = now });
                        if (logs == 0) // No logs meet requirement for archiving
                        {
                            return "No logs for archiving";
                        }
                        using (StreamWriter writer = new StreamWriter(FilePath + 
                            $@"\{DateTime.Now.ToString("yyyy-MM")}_Logs.txt"))
                        {
                            foreach (Log log in _logs)
                            {
                                writer.WriteLine(log.ToString());
                            }
                        }
                        string result = CompressLogs(_logs);
                        result = DeleteLogs(now);
                        // Compress file at FilePath and store ZipFile at Destination
                        ZipFile.CreateFromDirectory(FilePath, Destination +
                            $@"\{DateTime.Now.ToString("yyyy-MM")}_Archive.zip"); // New zip every month
                        var deleteQuery = "DELETE FROM Logs WHERE Timestamp <= @Timestamp";
                        var affectedRows = connection.Execute(deleteQuery, new {Timestamp = DateTime.UtcNow.AddDays(-30)});
                        //string result
                        if(affectedRows > 0)
                        {
                            return "success";
                        }
                    }
                }
                else
                {
                    return "ArchiveLogs failure";
                }
                return "ArchiveLogs failure";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CreateUpdateQuery(Account account)
        {
            string newPassPhrase = "";
            Console.WriteLine("Enter newPassPhrase: ");
            newPassPhrase = Console.ReadLine();
            string updateQuery = "";
            if (!newPassPhrase.Equals(""))
            {
                updateQuery += "PassPhrase = '" + newPassPhrase + "', ";
            }
            string newEmail = "";
            Console.WriteLine("Enter newEmail: ");
            newEmail = Console.ReadLine();
            string newUsername = "";
            Console.WriteLine("Enter newUsername: ");
            newUsername = Console.ReadLine();
            string newAuthorizationLevel = "";
            Console.WriteLine("Enter newAuthorizationLevel: ");
            newAuthorizationLevel = Console.ReadLine();
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                {
                    if (!newEmail.Equals(""))
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
                    if (!newAuthorizationLevel.Equals(""))
                    {
                        updateQuery += "AuthorizationLevel = '" + newAuthorizationLevel + "', ";
                    }
                    updateQuery = updateQuery.Remove(updateQuery.LastIndexOf(','), 1);
                    updateQuery += "WHERE Username = @Username";
                    return updateQuery;
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public string StoreOTP(Account user, string otp)
        {

            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                {
                    var storeOTPQuery = @"Insert Tresearch.OTPInstances (username, OTP, time) VALUES (@username, @OTP, @time)";
                    var affectedRows = connection.Execute(storeOTPQuery, new { Username = user.Username }, new { OTP = otp }, new { time = DateTime.Now });
                    if (affectedRows == 1)
                    {
                        return ("Sucess");
                    }
                    else
                    {
                        return ("StoreOTP failure");
                    }
                }
            }
            catch (Exception ex)
            {
                return (ex.Message);
            }
        }

        public List<Log> GetLogsOlderThan30Days(DateTime now)
        {
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                {
                    string query = @"SELECT * FROM Tresearch.Logs WHERE Timestamp < @Timestamp";
                    List<Log> _logs = (List<Log>)connection.Query(query, new { Timestamp = now });
                    if (_logs != null)
                    {
                        return _logs;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string CompressLogs(List<Log> _logs)
        {
            return "Success";
        }

        public string DeleteLogs(DateTime now)
        {
            return "Success";
        }

    }
}
using Dapper;
using System.Data.SqlClient;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.DAL.Implementations
{
    public class SqlDAO : ISqlDAO
    {
        public string SqlConnectionString { get; set; }

        public SqlDAO()
        {
        }

        public bool CreateConfirmationLink(IAccount account, string link)
        {
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                {
                    // Check if account already has an link
                    string email = account.email;
                    var insertQuery = "INSERT INTO confirmation_links (Email, Link) VALUES (@Email, @Link)";
                    int affectedRows = connection.Execute(insertQuery, new { Email = email, Link = link });

                    if (affectedRows == 1)
                        return true;
                    else
                        return false;
                }
            }  catch
            {
                return false;
            }  
        }

        public bool CreateAccount(IAccount account)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                {
                    var readQuery = "SELECT COUNT(*) FROM Accounts WHERE Email = @Email";
                    var accounts = connection.ExecuteScalar<int>(readQuery, new { Email = account.email });
                    if (accounts > 0)
                    {
                        account.username = account.username.Insert(account.username.IndexOf('@'), accounts.ToString());
                    }
                    var insertQuery = "INSERT INTO Accounts (Email, Username, Passphrase, AuthorizationLevel, Status) " +
                        "VALUES (@email, @username, @passphrase, @authorization_level, @Status)";
                    affectedRows = connection.Execute(insertQuery, account);
                }
                if (affectedRows == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}

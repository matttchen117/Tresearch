using Dapper;
using System.Data.SqlClient;
using System.Security.Principal;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.DAL.Implementations
{
    public class SqlDAO : ISqlDAO
    {
        public string SqlConnectionString { get; set; }

        public SqlDAO()
        {
        }

        public bool CreateConfirmationLink(IConfirmationLink _confirmationlink)
        {
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                {
                    var insertQuery = "INSERT INTO confirmation_links (Username, Link, Timestamp) VALUES (@Username, @Link, @Timestamp)";
                    int affectedRows = connection.Execute(insertQuery, _confirmationlink);

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

        public IConfirmationLink GetConfirmationLink(string url)
        {
            string guidString = url.Substring(url.LastIndexOf('/')+1);
            Guid guid = new Guid(url);
            IConfirmationLink _confirmationLink;

            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                {
                    var readQuery = "SELECT * FROM confirmation_links WHERE GUID = @guid";
                    _confirmationLink = connection.QuerySingle<ConfirmationLink>(readQuery, new { Guid = guid });

                }
            } catch
            {
                return null;
            }

            return _confirmationLink;
        }

        public bool ConfirmAccount(IAccount account)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                {
                    var updateQuery = "UPDATE confirmation_links SET confirmed = 1 WHERE Username = @Username and Email = @Email";
                    affectedRows = connection.Execute(updateQuery, new { Username = account.username, Email = account.email });

                }
                if (affectedRows == 1)
                    return true;
                else
                    return false;
            }
            catch
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
                    var insertQuery = "INSERT INTO user_accounts (Email, Username, Passphrase, AuthorizationLevel, Status) " +
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



            

        public string DeleteAccount(IRolePrincipal rolePrincipal)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                {
                    var readQuery = "SELECT * FROM user_accounts WHERE username = @username";
                    var account = connection.ExecuteScalar<int>(readQuery, rolePrincipal.Identity.Name);
                    if(account == 0)
                    {
                        //meaning that there wasn't an account to delete
                        Console.WriteLine("There wasn't an account found with associated username.");
                        //check for result to equal this
                        return "No associated account was found.";
                    }
                    var storedProcedure = "CREATE PROCEDURE dbo.deleteAccount @username varchar(25) AS BEGIN" +
                        "DELETE FROM user_accounts WHERE username = @username;" +
                        "DELETE FROM otp_claims WHERE username = @username;" +
                        "DELETE FROM nodes WHERE account_own = @username;" +
                        "DELETE FROM user_ratings WHERE username = @username;" +
                        "DELETE FROM email_confirmation_links WHERE username = @username;" +
                        "END";

                    affectedRows = connection.Execute(storedProcedure, rolePrincipal.Identity.Name);


                }
                if (affectedRows >= 1)
                {
                    return "success";
                }
                else
                {
                    Console.WriteLine("Couldn't delete account.");
                    return "Error, could not delete account.";
                }
            }
            catch (Exception ex)
            {
                return "Exception occurred";
            }

        }

    }
}

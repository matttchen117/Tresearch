using Dapper;
using System.Data.SqlClient;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.DAL.Implementations
{
    public class SqlDAO : ISqlDAO
    {
        private string SqlConnectionString { get; }

        public SqlDAO()
        {
        }

        public bool CreateConfirmationLink(IConfirmationLink _confirmationlink)
        {
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                {
                    var insertQuery = "INSERT INTO confirmation_links (Username, Guid, Timestamp) VALUES (@Username, @Guid, @Timestamp)";
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

        public bool DeleteConfirmationLink(IConfirmationLink confirmationLink)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                {
                    var deleteQuery = "DELETE FROM confirmation_links WHERE @Username=username and @Guid=guid and @Timestamp=Timestamp";
                    affectedRows = connection.Execute(deleteQuery, confirmationLink);
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

        public List<KPI> LoadKPIs(DateTime now)
        {

        }

    }
}

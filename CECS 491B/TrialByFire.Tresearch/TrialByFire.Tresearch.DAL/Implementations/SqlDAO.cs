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

    }
}

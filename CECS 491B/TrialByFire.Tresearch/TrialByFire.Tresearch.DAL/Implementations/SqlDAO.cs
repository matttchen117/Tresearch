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

        public bool CreateConfirmationLink(IConfirmationLink confirmationlink)
        {
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                {
                    var insertQuery = "INSERT INTO confirmation_links (Username, Guid, Timestamp) VALUES (@Username, @Guid, @Timestamp)";
                    int affectedRows = connection.Execute(insertQuery, confirmationlink);

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
                    var updateQuery = "UPDATE confirmation_links SET confirmed = 1 WHERE Username = " +
                        "@Username and Email = @Email";
                    affectedRows = connection.Execute(updateQuery, new { Username = account.Username, 
                        Email = account.Email });

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
                    var accounts = connection.ExecuteScalar<int>(readQuery, new { Email = account.Email });
                    if (accounts > 0)
                    {
                        account.Username = account.Username.Insert(account.Username.IndexOf('@'), accounts.ToString());
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
                    if (account == 0)
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



        public string VerifyAccount(IAccount account)
        {
            throw new NotImplementedException();
        }

        public List<string> Authenticate(IOTPClaim otpClaim)
        {
            throw new NotImplementedException();
        }
        public string VerifyAuthorized(IRolePrincipal rolePrincipal, string requiredRole)
        {
            throw new NotImplementedException();
        }

        public IOTPClaim GetOTPClaim(IOTPClaim otpClaim)
        {
            throw new NotImplementedException();
        }

        public string StoreOTP(IOTPClaim otpClaim)
        {
            throw new NotImplementedException();
        }

        public List<IKPI> LoadKPI(DateTime now)
        {
            throw new NotImplementedException();
        }



        public string CreateNodesCreated(INodesCreated nodesCreated)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                {
                    var insertQuery = @"INSERT INTO Tresearch.NodesCreated (node_creation_date, node_creation_count)
Values (@node_creation_date, @node_creation_count)";

                    affectedRows = connection.Execute(insertQuery,
                                    new { node_creation_date = nodesCreated.nodeCreationDate,
                                        node_creation_count = nodesCreated.nodeCreationCount
                                    });
                }
                if (affectedRows == 1)
                {
                    return "Created Nodes Successfully Inserted";
                }
                else
                {
                    return "Created Nodes Not Inserted";
                }
            }
            catch (Exception ex)
            {
                return "Fail";
            }
        }

        public INodesCreated GetNodesCreated(DateTime nodeCreationDate)
        {
            INodesCreated nodesCreated;

            using (var connection = new SqlConnection(SqlConnectionString))
            {
                var selectQuery = "SELECT * FROM Tresearch.nodes_created" +
                                  "WHERE _node_creation_date >= @node_creation_date - 30";

                nodesCreated = connection.QuerySingle<NodesCreated>(selectQuery, new {node_creation_date = nodeCreationDate});
            }

            return nodesCreated;
        }

        public string UpdateNodesCreated(INodesCreated nodesCreated)
        {
            using (var connection = new SqlConnection(SqlConnectionString))
            {
                var updateQuery = @"UPDATE Tresearch.nodes_created (nodes_created_date, nodes_created_count)" +
                                    "VALUES (@nodes_created_date, @nodes_created_count)";

                var _result = connection.Execute(updateQuery,
                            new { nodes_created_date = nodesCreated.nodeCreationDate,
                                  nodes_created_count = nodesCreated.nodeCreationCount
                            }
                            );
            }

            return "Node Created Successfully Updated";
        }



        public string CreateDailyLogin(IDailyLogin dailyLogin)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                {
                    var insertQuery = @"INSERT INTO Tresearch.DailyLogins (login_date, login_count)
                                        Values (@loginDate, @loginCount)";           
                    affectedRows = connection.Execute(insertQuery,
                                        new { login_date = dailyLogin.loginDate,
                                            login_count = dailyLogin.loginCount
                                        });
                }
                if (affectedRows == 1)
                {
                    return "Daily Login Successfully Created";
                }
                else
                {
                    return "Daily Login Creation Failed";
                }
            }
            catch (Exception ex)
            {
                return "Fail";
            }
        }

        public IDailyLogin GetDailyLogin(DateTime loginDate)
        {
            IDailyLogin dailyLogin;

            using (var connection = new SqlConnection(SqlConnectionString))
            {
                var selectQuery = "SELECT * FROM Tresearch.daily_logins" +
                                    "WHERE _loginDate >= @login_date - 30";

                dailyLogin = connection.QuerySingle<DailyLogin>(selectQuery, new { login_date = loginDate });
            }

            return dailyLogin;
        }

        public string UpdateDailyLogin(IDailyLogin dailyLogin)
        {
            IDailyLogin logins;

            using (var connection = new SqlConnection(SqlConnectionString))
            {
                var updateQuery = @"UPDATE Tresearch.daily_logins (login_date, login_count) " + 
                                    "VALUES (@login_date, @login_count)";

                logins = connection.QuerySingle<DailyLogin>(updateQuery, new {login_date = dailyLogin.loginDate, 
                                                                              login_count = dailyLogin.loginCount
                                                                          });
            }

            return "Daily Login Update Successful";
        }



        public string CreateTopSearch(ITopSearch topSearch)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                {
                    var insertQuery = @"INSERT INTO Tresearch.TopSearch (top_search_date, top_search_string, top_search_countl)" +
                                        "Values (@top_search_date, @top_search_string, @top_search_count)";
                    affectedRows = connection.Execute(insertQuery,
                                        new { top_search_date = topSearch.topSearchDate,
                                            top_search_string = topSearch.searchString,
                                            top_search_count = topSearch.searchCount
                                        });
                }
                if (affectedRows == 1)
                {
                    return "Top Search Creation Successful";
                }
                else
                {
                    return "Top Search Creation Failed";
                }
            }
            catch (Exception ex)
            {
                return "Fail";
            }
        }

        public ITopSearch GetTopSearch(DateTime topSearchDate)
        {
            ITopSearch topSearch;
            
            using (var connection = new SqlConnection(SqlConnectionString))
            {
                var selectQuery = "SELECT * FROM Tresearch.top_search" +
                                    "WHERE topSearchDate >= @top_search_date - 30";
                topSearch = connection.QuerySingle<TopSearch>(selectQuery, new {top_search_date = topSearchDate});
            }

            return topSearch;
        }

        public string UpdateTopSearch(ITopSearch topSearch)
        {
            using (var connection = new SqlConnection(SqlConnectionString))
            {
                var updateQuery = @"UPDATE Tresearch.top_search (top_search_date, search_string, search_count)" +
                                    "VALUES (@top_search_date, @search_string, @search_count)";

                var _result = connection.Execute(updateQuery,
                                                    new{ top_search_date = topSearch.topSearchDate,
                                                         search_string = topSearch.searchCount,
                                                         search_count = topSearch.searchCount
                                                    });
            }

            return "Top Search Update Successful";
        }



        public string CreateDailyRegistration(IDailyRegistration dailyRegistration)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                {
                    var insertQuery = @"INSERT INTO Tresearch.DailyRegistrations (registration_date, registration_countl)" + 
                                        "Values (@registrationDate, @registrationCount)";
                   affectedRows = connection.Execute(insertQuery,
                                    new
                                    {
                                        registration_date = dailyRegistration.registrationDate,
                                        registration_count = dailyRegistration.registrationCount
                                    });
                }
                if (affectedRows == 1)
                {
                    return "Daily Registration Creation Successful";
                }
                else
                {
                    return "Daily Registration Creation Failed";
                }
            }
            catch (Exception ex)
            {
                return "Fail";
            }
        }

        public IDailyRegistration GetDailyRegistration(DateTime dailyRegistrationDate)
        {
            IDailyRegistration dailyRegistration;

            using (var connection = new SqlConnection(SqlConnectionString))
            {
                var selectQuery = "SELECT * FROM Tresearch.daily_registrations" +
                                    "WHERE _registrationDate >= @registration_date - 30";

                dailyRegistration = connection.QuerySingle<DailyRegistration>(selectQuery, new { registration_date = dailyRegistrationDate });
            }

            return dailyRegistration;
        }

        public string UpdateDailyRegistration(IDailyRegistration dailyRegistration)
        {
            using (var connection = new SqlConnection(SqlConnectionString))
            {
                var updateQuery = @"UPDATE Tresearch.daily_registrations (registration_date, registration_count)" +
                                    "VALUES (@registration_date, @registration_count)";

                var result = connection.Execute(updateQuery,
                                new { registration_date = dailyRegistration.registrationDate });
            }

            return "Daily Registration Update Successful";
        }
    }
}

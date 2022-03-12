using Dapper;
using System.Data.SqlClient;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Exceptions;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.DAL.Implementations
{
    public class SqlDAO : ISqlDAO
    {
        private string _sqlConnectionString { get; }
        private IMessageBank _messageBank;

        public SqlDAO(IMessageBank messageBank)
        {
            _sqlConnectionString = "Data Source=tresearchstudentserver.database.windows.net;Initial Catalog=tresearchStudentServer;User ID=tresearchadmin;Password=CECS491B!;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            _messageBank = messageBank;
        }

        public SqlDAO(string sqlConnectionString, IMessageBank messageBank)
        {
            _sqlConnectionString = sqlConnectionString;
            _messageBank = messageBank;
        }

        public List<string> CreateConfirmationLink(IConfirmationLink _confirmationlink)
        {
            List<string> result = new List<string>();
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionString))
                {
                    var insertQuery = "INSERT INTO dbo.EmailConfirmationLinks (username, GUID, timestamp) VALUES (@Username, @UniqueIdentifier, @Datetime)";
                    int affectedRows = connection.Execute(insertQuery, _confirmationlink);

                    if (affectedRows == 1)
                        result.Add("Success - Confirmation Link added to database");
                    else
                        result.Add("Failed - Email already has confirmation link");
                }
            }
            catch (Exception ex)
            {
                result.Add("Failed - SQLDAO " + ex);
            }
            return result;
        }

        public IConfirmationLink GetConfirmationLink(string url)
        {

            string guidString = url.Substring(url.LastIndexOf('=') + 1);
            //Guid guid = new Guid(guidString);


            IConfirmationLink _confirmationLink = new ConfirmationLink();

            try
            {
                using (var connection = new SqlConnection(_sqlConnectionString))
                {

                    var readQuery = "SELECT username FROM dbo.EmailConfirmationLinks WHERE GUID = @guid";
                    _confirmationLink.Username = connection.ExecuteScalar<string>(readQuery, new { guid = guidString });
                    readQuery = "SELECT GUID FROM dbo.EmailConfirmationLinks WHERE GUID = @guid";
                    _confirmationLink.UniqueIdentifier = connection.ExecuteScalar<Guid>(readQuery, new { guid = guidString });
                    readQuery = "SELECT datetime FROM dbo.EmailConfirmationLinks WHERE GUID = @guid";
                    _confirmationLink.Datetime = connection.ExecuteScalar<DateTime>(readQuery, new { guid = guidString });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return _confirmationLink;
            }

            return _confirmationLink;
        }


        public List<string> ConfirmAccount(IAccount account)
        {
            List<string> results = new List<string>();
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionString))
                {
                    var updateQuery = "UPDATE dbo.Accounts SET confirmation = 1 WHERE email = @Email and username = @Username";
                    affectedRows = connection.Execute(updateQuery, account);

                }
                if (affectedRows == 1)
                    results.Add("Success - Account confirmed in database");
                else
                    results.Add("Failed - Account doesn't exist in database");
            }
            catch
            {
                results.Add("Failed - SQLDAO  could not be confirm account in database");
            }
            return results;
        }
        public bool DeleteConfirmationLink(IConfirmationLink confirmationLink)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionString))
                {
                    var deleteQuery = "DELETE FROM dbo.EmailConfirmationLinks WHERE @Username=username and @Guid=guid and @Timestamp=Timestamp";
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

        public List<string> CreateAccount(IAccount account)
        {
            List<string> results = new List<string>();
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionString))
                {
                    var readQuery = "SELECT COUNT(*) FROM dbo.Accounts WHERE Email = @Email";
                    var accounts = connection.ExecuteScalar<int>(readQuery, new { Email = account.Email });

                    if (accounts > 0)
                    {
                        results.Add("Failed - Account already exists in database");
                        return results;
                    }
                    var insertQuery = "INSERT INTO dbo.Accounts (Username, Email, Passphrase, AuthorizationLevel, AccountStatus, Confirmed) " +
                        "VALUES (@Username, @Email, @Passphrase, @AuthorizationLevel, @AccountStatus, @Confirmed)";

                    affectedRows = connection.Execute(insertQuery, account);
                }
                if (affectedRows == 1)
                    results.Add("Success - Account created in database");
                else
                    results.Add("Failed - Account not created in database");
            }
            catch (Exception ex)
            {
                results.Add("Failed - " + ex);
            }
            return results;
        }

        public IAccount GetUnconfirmedAccount(string email)
        {
            IAccount account = new Account();
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionString))
                {
                    var readQuery = "SELECT username FROM dbo.user_accounts WHERE email = @Email and authorization_level = 'User'";
                    string username = connection.ExecuteScalar<string>(readQuery, new { Email = email });
                    readQuery = "SELECT passphrase FROM dbo.user_accounts WHERE email = @Email and authorization_level = 'User'";
                    string passphrase = connection.ExecuteScalar<string>(readQuery, new { Email = email });
                    readQuery = "SELECT account_status FROM dbo.user_accounts WHERE email = @Email and authorization_level = 'User'";
                    bool status = connection.ExecuteScalar<bool>(readQuery, new { Email = email });
                    account = new Account(email, username, passphrase, "User", status, false);
                    return account;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return null;
            }

        }


        public List<string> RemoveConfirmationLink(IConfirmationLink confirmationLink)
        {
            List<string> results = new List<string>();
            int affectedRows;

            try
            {
                using (var connection = new SqlConnection(_sqlConnectionString))
                {
                    var deleteQuery = "DELETE FROM dbo.confirmation_links WHERE GUID = @guid";
                    affectedRows = connection.Execute(deleteQuery, new { guid = confirmationLink.UniqueIdentifier });
                }
                if (affectedRows == 1)
                    results.Add("Success - Confirmation Link removed from database");
                else
                    results.Add("Failed - Confirmation link unable to be removed from database");
            }
            catch (Exception ex)
            {
                results.Add("Failed - Confirmation link not removed in database" + ex);
            }
            return results;
        }
        public string DeleteAccount(IRolePrincipal rolePrincipal)
        {

            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionString))
                {
                    var readQuery = "SELECT * FROM Accounts WHERE Username = @username AND AuthorizationLevel = @role";
                    var account = connection.ExecuteScalar<int>(readQuery, new { username = rolePrincipal.RoleIdentity.Username, role = rolePrincipal.RoleIdentity.AuthorizationLevel });
                    if (account == 0)
                    {
                        return _messageBank.ErrorMessages["notFoundOrAuthorized"];
                    }
                    else
                    {
                        var storedProcedure = "CREATE PROCEDURE dbo.deleteAccount @username varchar(25) AS BEGIN" +
                            "DELETE FROM Accounts WHERE Username = @username;" +
                            "DELETE FROM OTPClaims WHERE Username = @username;" +
                            "DELETE FROM Nodes WHERE account_own = @username;" +
                            "DELETE FROM UserRatings WHERE Username = @username;" +
                            "DELETE FROM EmailConfirmationLinks WHERE username = @username;" +
                            "END";

                        affectedRows = connection.Execute(storedProcedure, rolePrincipal.RoleIdentity.Username);
                    }


                }

                if (affectedRows >= 1)
                {
                    return _messageBank.SuccessMessages["generic"];
                }
                else
                {
                    return _messageBank.ErrorMessages["notFoundOrAuthorized"];
                }
            }
            catch (AccountDeletionFailedException adfe)
            {
                return adfe.Message;
            }

        }

        public string VerifyAccount(IAccount account)
        {
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionString))
                {
                    string query = "SELECT * FROM Accounts WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel";
                    IAccount dbAccount = connection.QueryFirst<Account>(query, new
                    {
                        Username = account.Username,
                        AuthorizationLevel = account.AuthorizationLevel
                    });
                    if (dbAccount == null)
                    {
                        return _messageBank.ErrorMessages["notFoundOrAuthorized"];
                    }
                    else if (dbAccount.Confirmed == false)
                    {
                        return _messageBank.ErrorMessages["notConfirmed"];
                    }
                    else if (dbAccount.AccountStatus == false)
                    {
                        return _messageBank.ErrorMessages["notFoundOrEnabled"];
                    }
                    else
                    {
                        if (account.Passphrase.Equals(dbAccount.Passphrase))
                        {
                            return _messageBank.SuccessMessages["generic"];
                        }
                        else
                        {
                            return _messageBank.ErrorMessages["badNameOrPass"];
                        }
                    }
                }
            }
            catch (AccountCreationFailedException acfe)
            {
                return acfe.Message;
            }
            catch (InvalidOperationException ioe)
            {
                return _messageBank.ErrorMessages["notFoundOrEnabled"];
            }
            catch (Exception ex)
            {
                return "Database: " + ex.Message;
            }
        }

        public List<string> Authenticate(IOTPClaim otpClaim)
        {
            List<string> results = new List<string>();
            int affectedRows = 0;
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionString))
                {
                    string query = "SELECT * FROM Accounts WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel";
                    IAccount dbAccount = connection.QueryFirst<Account>(query, new
                    {
                        Username = otpClaim.Username,
                        AuthorizationLevel = otpClaim.AuthorizationLevel
                    });
                    if (dbAccount.Confirmed == false)
                    {
                        results.Add(_messageBank.ErrorMessages["notConfirmed"]);
                        return results;
                    }
                    if (dbAccount.AccountStatus == false)
                    {
                        results.Add(_messageBank.ErrorMessages["notFoundOrEnabled"]);
                        return results;
                    }
                    query = "SELECT * FROM OTPClaims WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel";
                    IOTPClaim dbOTPClaim = connection.QueryFirst<OTPClaim>(query, new
                    {
                        Username = otpClaim.Username,
                        AuthorizationLevel = otpClaim.AuthorizationLevel
                    });
                    if (dbOTPClaim == null)
                    {
                        results.Add(_messageBank.ErrorMessages["accountNotFound"]);
                        return results;
                    }
                    if (!otpClaim.OTP.Equals(dbOTPClaim.OTP))
                    {
                        int failCount = ++dbOTPClaim.FailCount;
                        if (failCount >= 5)
                        {
                            query = "UPDATE Accounts SET AccountStatus = 0 WHERE " +
                            "Username = @Username AND AuthorizationLevel = @AuthorizationLevel";
                            affectedRows = connection.Execute(query, new
                            {
                                Username = otpClaim.Username,
                                AuthorizationLevel = otpClaim.AuthorizationLevel,
                            });
                            if (affectedRows != 1)
                            {
                                results.Add(_messageBank.ErrorMessages["accountDisableFail"]);
                                return results;
                            }
                            else
                            {
                                results.Add(_messageBank.ErrorMessages["tooManyFails"]);
                                return results;
                            }
                        }
                        query = "UPDATE OTPClaims SET FailCount = @FailCount WHERE " +
                        "Username = @Username AND AuthorizationLevel = @AuthorizationLevel";
                        affectedRows = connection.Execute(query, new
                        {
                            Username = otpClaim.Username,
                            AuthorizationLevel = otpClaim.AuthorizationLevel,
                            FailCount = dbOTPClaim.FailCount++
                        });
                        if (affectedRows != 1)
                        {
                            results.Add(_messageBank.ErrorMessages["accountNotFound"]);
                            return results;
                        }
                        else
                        {
                            results.Add(_messageBank.ErrorMessages["badNameOrOTP"]);
                            return results;
                        }
                    }
                    else if ((otpClaim.TimeCreated >= dbOTPClaim.TimeCreated) && (otpClaim.TimeCreated <= dbOTPClaim.TimeCreated.AddMinutes(2)))
                    {
                        results.Add(_messageBank.SuccessMessages["generic"]);
                        results.Add($"username:{otpClaim.Username},authorizationLevel:{otpClaim.AuthorizationLevel}");
                        return results;
                    }
                    else
                    {
                        results.Add(_messageBank.ErrorMessages["otpExpired"]);
                        return results;
                    }
                }
            }
            catch (OTPClaimCreationFailedException occfe)
            {
                results.Add(occfe.Message);
                return results;
            }
            catch (InvalidOperationException ioe)
            {
                results.Add(_messageBank.ErrorMessages["notFoundOrEnabled"]);
                return results;
            }
            catch (Exception ex)
            {
                results.Add("Database: " + ex.Message);
                return results;
            }
        }
        public string VerifyAuthorized(IRolePrincipal rolePrincipal, string requiredAuthLevel)
        {
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionString))
                {
                    string query = "SELECT * FROM Accounts WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel";
                    IAccount dbAccount = connection.QueryFirst<Account>(query, new
                    {
                        Username = rolePrincipal.RoleIdentity.Username,
                        AuthorizationLevel = rolePrincipal.RoleIdentity.AuthorizationLevel
                    });
                    if (dbAccount.Confirmed == false)
                    {
                        return _messageBank.ErrorMessages["notConfirmed"];
                    }
                    else if (dbAccount.AccountStatus == false)
                    {
                        return _messageBank.ErrorMessages["notFoundOrEnabled"];
                    }
                    else
                    {
                        if (dbAccount.AuthorizationLevel.Equals("admin") ||
                            dbAccount.AuthorizationLevel.Equals(requiredAuthLevel))
                        {
                            return _messageBank.SuccessMessages["generic"];
                        }
                        else
                        {
                            return _messageBank.ErrorMessages["notFoundOrAuthorized"];
                        }
                    }
                }
            }
            catch (AccountCreationFailedException acfe)
            {
                return acfe.Message;
            }
            catch (InvalidOperationException ioe)
            {
                return _messageBank.ErrorMessages["notFoundOrAuthorized"];
            }
            catch (Exception ex)
            {
                return "Database: " + ex.Message;
            }
        }

        public string StoreOTP(IOTPClaim otpClaim)
        {
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionString))
                {
                    string query = "SELECT * FROM OTPClaims WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel";
                    IOTPClaim dbOTPClaim = connection.QueryFirst<OTPClaim>(query, new
                    {
                        Username = otpClaim.Username,
                        AuthorizationLevel = otpClaim.AuthorizationLevel
                    });
                    if (dbOTPClaim == null)
                    {
                        return _messageBank.ErrorMessages["notFound"];
                    }
                    else
                    {
                        int failCount = dbOTPClaim.FailCount;
                        if (otpClaim.TimeCreated > dbOTPClaim.TimeCreated.AddDays(1))
                        {
                            failCount = 0;
                        }
                        query = "UPDATE OTPClaims SET OTP = @OTP,TimeCreated = @TimeCreated, " +
                        "FailCount = @FailCount WHERE Username = @Username AND AuthorizationLevel = @AuthorizationLevel";
                        var affectedRows = connection.Execute(query, new
                        {
                            Username = otpClaim.Username,
                            AuthorizationLevel = otpClaim.AuthorizationLevel,
                            OTP = otpClaim.OTP,
                            TimeCreated = otpClaim.TimeCreated,
                            FailCount = otpClaim.FailCount
                        });
                        if (affectedRows != 1)
                        {
                            return _messageBank.ErrorMessages["otpFail"];
                        }
                        else
                        {
                            return _messageBank.SuccessMessages["generic"];
                        }
                    }
                }
            }
            catch (OTPClaimCreationFailedException occfe)
            {
                return occfe.Message;
            }
            catch (InvalidOperationException ioe)
            {
                return _messageBank.ErrorMessages["notFoundOrEnabled"];
            }
            catch (Exception ex)
            {
                return "Database: " + ex.Message;
            }
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
                using (var connection = new SqlConnection(_sqlConnectionString))
                {
                    var insertQuery = @"INSERT INTO Tresearch.NodesCreated (node_creation_date, node_creation_count)
Values (@node_creation_date, @node_creation_count)";

                    affectedRows = connection.Execute(insertQuery,
                                    new
                                    {
                                        node_creation_date = nodesCreated.nodeCreationDate,
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

            using (var connection = new SqlConnection(_sqlConnectionString))
            {
                var selectQuery = "SELECT * FROM Tresearch.nodes_created" +
                                  "WHERE _node_creation_date >= @node_creation_date - 30";

                nodesCreated = connection.QuerySingle<INodesCreated>(selectQuery, new { node_creation_date = nodeCreationDate });
            }

            return nodesCreated;
        }

        public string UpdateNodesCreated(INodesCreated nodesCreated)
        {
            using (var connection = new SqlConnection(_sqlConnectionString))
            {
                var updateQuery = @"UPDATE Tresearch.nodes_created (nodes_created_date, nodes_created_count)" +
                                    "VALUES (@nodes_created_date, @nodes_created_count)";

                var _result = connection.Execute(updateQuery,
                            new
                            {
                                nodes_created_date = nodesCreated.nodeCreationDate,
                                nodes_created_count = nodesCreated.nodeCreationCount
                            }
                            );
            }

            return "Node Created Successfully Updated";
        }



        public string CreateDailyLogins(IDailyLogin dailyLogin)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionString))
                {
                    var insertQuery = @"INSERT INTO Tresearch.DailyLogins (login_date, login_count)
                                        Values (@loginDate, @loginCount)";
                    affectedRows = connection.Execute(insertQuery,
                                        new
                                        {
                                            login_date = dailyLogin.loginDate,
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

            using (var connection = new SqlConnection(_sqlConnectionString))
            {
                var selectQuery = "SELECT * FROM Tresearch.daily_logins" +
                                    "WHERE _loginDate >= @login_date - 30";

                dailyLogin = connection.QuerySingle<IDailyLogin>(selectQuery, new { login_date = loginDate });
            }

            return dailyLogin;
        }

        public string UpdateDailyLogin(IDailyLogin dailyLogin)
        {
            IDailyLogin logins;

            using (var connection = new SqlConnection(_sqlConnectionString))
            {
                var updateQuery = @"UPDATE Tresearch.daily_logins (login_date, login_count) " +
                                    "VALUES (@login_date, @login_count)";

                logins = connection.QuerySingle<IDailyLogin>(updateQuery, new
                {
                    login_date = dailyLogin.loginDate,
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
                using (var connection = new SqlConnection(_sqlConnectionString))
                {
                    var insertQuery = @"INSERT INTO Tresearch.TopSearch (top_search_date, top_search_string, top_search_countl)" +
                                        "Values (@top_search_date, @top_search_string, @top_search_count)";
                    affectedRows = connection.Execute(insertQuery,
                                        new
                                        {
                                            top_search_date = topSearch.topSearchDate,
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

            using (var connection = new SqlConnection(_sqlConnectionString))
            {
                var selectQuery = "SELECT * FROM Tresearch.top_search" +
                                    "WHERE topSearchDate >= @top_search_date - 30";
                topSearch = connection.QuerySingle<ITopSearch>(selectQuery, new { top_search_date = topSearchDate });
            }

            return topSearch;
        }

        public string UpdateTopSearch(ITopSearch topSearch)
        {
            using (var connection = new SqlConnection(_sqlConnectionString))
            {
                var updateQuery = @"UPDATE Tresearch.top_search (top_search_date, search_string, search_count)" +
                                    "VALUES (@top_search_date, @search_string, @search_count)";

                var _result = connection.Execute(updateQuery,
                                                    new
                                                    {
                                                        top_search_date = topSearch.topSearchDate,
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
                using (var connection = new SqlConnection(_sqlConnectionString))
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

            using (var connection = new SqlConnection(_sqlConnectionString))
            {
                var selectQuery = "SELECT * FROM Tresearch.daily_registrations" +
                                    "WHERE _registrationDate >= @registration_date - 30";

                dailyRegistration = connection.QuerySingle<IDailyRegistration>(selectQuery, new { registration_date = dailyRegistrationDate });
            }

            return dailyRegistration;
        }

        public string UpdateDailyRegistration(IDailyRegistration dailyRegistration)
        {
            using (var connection = new SqlConnection(_sqlConnectionString))
            {
                var updateQuery = @"UPDATE Tresearch.daily_registrations (registration_date, registration_count)" +
                                    "VALUES (@registration_date, @registration_count)";

                var result = connection.Execute(updateQuery,
                                new { registration_date = dailyRegistration.registrationDate });
            }

            return "Daily Registration Update Successful";
        }

        public string CreateDailyLogin(IDailyLogin dailyLogin)
        {
            throw new NotImplementedException();
        }
    }
}
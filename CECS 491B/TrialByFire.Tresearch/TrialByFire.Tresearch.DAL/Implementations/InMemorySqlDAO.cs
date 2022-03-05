using Dapper;
using System;
using System.Data.SqlClient;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.DAL.Implementations
{
    public class InMemorySqlDAO : ISqlDAO
    { 

        public InMemoryDatabase inMemoryDatabase;

        public InMemorySqlDAO()
        {
            inMemoryDatabase = new InMemoryDatabase();
        }

        public bool CreateAccount(IAccount _account)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(sqlConnectionString))
                {
                    var readQuery = "SELECT COUNT(*) FROM Accounts WHERE Email = @Email";
                    var accounts = connection.ExecuteScalar<int>(readQuery, new { Email = _account.email });
                    if (accounts > 0)
                    {
                        account.username = account.username.Insert(account.username.IndexOf('@'), accounts.ToString());
                    }
                    var insertQuery = "INSERT INTO user_accounts (Email, Username, Passphrase, AuthorizationLevel, Status) " +
                        "VALUES (@email, @username, @passphrase, @authorization_level, @Status)";
                    affectedRows = connection.Execute(insertQuery, _account);
                }
                if (affectedRows == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

                using (var connection = new SqlConnection(sqlConnectionString))
                {
                    var insertQuery = @"INSERT INTO Tresearch.OTP (OTP)
                                        Value (@OTP)";

                    var affectedRows = connection.Execute(insertQuery, _otp : OTP);
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

        public bool CreateUpdateOTP(IOTPClaim _otpclaim)
        {
            using(var connection = new SQLConnection(SqlConnectionString))
            {
                var updateQuery = @"UPDATE Tresearch.otp_claims (username, otp, time_created, fail_count)" +
                                    "Values (@username, @otp, @time_created, @fail_count)";

                var affectedRows = connection.Execute(updateQuery,
                                    new {username = _otp.username}, new {otp = _otp.otp},
                                    new {time_created = _otp.time_created},
                                    new {fail_count = _otp.fail_count})
            }
        }



        public bool CreateNodesCreated(INodesCreated _nodesCreated)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                { 
                    var insertQuery = @"INSERT INTO Tresearch.NodesCreated (node_creation_date, node_creation_count)
Values (@node_creation_date, @node_creation_count)";

                    affectedRows = var affectedRows = connection.Execute(insertQuery,
                                    new{node_creation_date = _nodesCreated.node_creation_date},
                                    new {node_creation_count = _nodesCreated.node_creation_count});
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

        public List<INodesCreated> GetNodesCreated(DateTime _nodeCreationDate)
        {
            using(var connection = new SQLConnection(sqlConnectionString))
            {
                var selectQuery = "SELECT * FROM Tresearch.nodes_created" + 
                                  "WHERE _node_creation_date >= @node_creation_date - 30";

                var _result = connection.Execute(selectQuery, _nodeCreationDate)
            }
        }

        public bool UpdateNodesCreated(INodesCreated _nodesCreated)
        {
            using(var connection = new SQLConnection(sqlConnectionString))
            {
                var updateQuery = @"UPDATE Tresearch.nodes_created (nodes_created_date, nodes_created_count)" +
                                    "VALUES (@nodes_created_date, @nodes_created_count)";

                var _result = connection.Execute(updateQuery,
                            new {nodes_created_date = _nodesCreated.nodes_created_date},
                            new{nodes_created_count = nodesCreated.nodes_created_count);
            }
        }



        public bool CreateDailyLogins(IDailyLogin _dailyLogin)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                { 
                    var insertQuery = @"INSERT INTO Tresearch.DailyLogins (login_date, login_countl)
                                        Values (@loginDate, @loginCount)"                        "VALUES (@email, @username, @passphrase, @authorization_level, @Status)";
                    var affectedRows = connection.Execute(insertQuery,
                                        new{login_date = _dailyLogin.login_date},
                                        new {login_count = _dailyLogin.login_count})
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

        public List<DailyLogin> GetDailyLogin(DateTime _loginDate)
        {
            using(var connection = new SQLConnection(sqlConnectionString))
            {
                var selectQuery = "SELECT * FROM Tresearch.daily_logins" +
                                    "WHERE _loginDate >= @login_date - 30";

                var _result = connection.Execute(selectQuery, _loginDate : Date);
            }
        }

        public bool UpdateDailyLogin(IDailyLogin _dailyLogin)
        {
            using(var connection = new SQLConnection(sqlConnectionString))
            {
                var updateQuery = @"UPDATE Tresearch.daily_logins (login_date, login_count) "
                                    "VALUES (@login_date, @login_count)";

                var _result = connection.Execute(updateQuery, 
                                        new {login_date = _dailyLogin.login_date})
            }
        }



        public bool CreateTopSearch(ITopSearch _topSearch)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                { 
                   var insertQuery = @"INSERT INTO Tresearch.TopSearch (top_search_date, top_search_string, top_search_countl)
                                        Values (@top_search_date, @top_search_string, @top_search_count)"
                    var affectedRows = connection.Execute(insertQuery,
                                        new{top_search_date = _TopSearch.top_search_date},
                                        new {top_search_string = _TopSearch.top_search_string},
                                        new {top_search_count = _TopSearch.top_search_count})
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

        public List<ITopSearch> GetTopSearch(DateTime _topSearchDate)
        {
            using(var connection = new SQLConnection(sqlConnectionString))
            {
                var selectQuery = "SELECT * FROM Tresearch.top_search" + 
                                    "WHERE _topSearchDate >= @top_search_date - 30";
                var _result = connection.Execute(selectQuery, _topSearchDate)
            }
        }

        public bool UpdateTopSearch(ITopSearch _topSearch)
        {
            using(var connection = new SQLConnection(sqlConnectionString))
            {
                var updateQuery = @"UPDATE Tresearch.top_search (top_search_date, search_string, search_count)" +
                                    "VALUES (@top_search_date, @search_string, @search_count)";

                var _result = connection.Execute(updateQuery,
                                                    new {top_search_date = _top_search.top_search_date}
                                                    new {search_string = _top_search.search_string}
                                                    new {search_count = _top_search.search_count})
            }
        }



        public bool CreateDailyRegistration(IDailyRegistration _dailyRegistration)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(SqlConnectionString))
                { 
                   var insertQuery = @"INSERT INTO Tresearch.DailyRegistrations (registration_date, registration_countl)
                                        Values (@registrationDate, @registrationCount)"
                   var affectedRows = connection.Execute(insertQuery,
                                    new{registration_date = _dailyRegistration.registration_date},
                                    new {registration_count = _dailyRegistration.registration_count});
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

        public List<DailyRegistration> GetDailyRegistration(DateTime _dailyRegistrationTime)
        {
            using (var connection = new SqlConnection(SqlConnectionString))
            {
                var selectQuery = "SELECT * FROM Tresearch.daily_registrations" +
                                    "WHERE _registrationDate >= @registration_date - 30";

                var _result = connection.Execute(selectQuery, _registrationDate);
            }
        }

        public bool UpdateDailyRegistration(IDailyRegistration _dailyRegistration)
        {
            using(var connection = new SQLConnection(sqlConnectionString))
            {
                var updateQuery = @"UPDATE Tresearch.daily_registrations (registration_date, registration_count)" +
                                    "VALUES (@registration_date, @registration_count)";

                var _result = connection.Execute(updateQuery,
                                new {registration_date = _registrationDate.registration_date})
            }
        }

        public string VerifyAccount(IAccount account)
        {
            IAccount dbAccount = inMemoryDatabase.Accounts[inMemoryDatabase.Accounts.IndexOf(account)];
            if(dbAccount != null)
            {
                if(dbAccount.status != false)
                {
                    return "success";
                }
                return "Database: The account has been disabled.";
            }
            return "Data: Incorrect Username or Passphrase.";
        }

        public List<string> Authenticate(IOTPClaim otpClaim)
        {
            List<string> results = new List<string>();
            IAccount account = new Account(otpClaim.Username);
            if(inMemoryDatabase.Accounts[inMemoryDatabase.Accounts.IndexOf(account)].status != false)
            {
                IOTPClaim dbOTPClaim = inMemoryDatabase.OTPClaims[inMemoryDatabase.OTPClaims.IndexOf(otpClaim)];
                if (dbOTPClaim != null)
                {
                    if (otpClaim.OTP.Equals(dbOTPClaim.OTP))
                    {
                        if (otpClaim.TimeCreated <= dbOTPClaim.TimeCreated.AddMinutes(2))
                        {
                            if (inMemoryDatabase.Accounts.Contains(account))
                            {
                                IRoleIdentity roleIdentity = new RoleIdentity(true, account.username, account.authorizationLevel);
                                IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
                                string isNotAuthenticated = VerifyNotAuthenticated(rolePrincipal);
                                if (isNotAuthenticated.Equals("success"))
                                {
                                    inMemoryDatabase.RolePrincipals.Add(rolePrincipal);
                                    results.Add("success");
                                    results.Add($"username:{roleIdentity.Name},role{roleIdentity.Role}");
                                    return results;
                                }
                                else
                                {
                                    results.Add(isNotAuthenticated);
                                }
                            }
                            else
                            {
                                results.Add("Database: Incorrect Username or OTP.");
                            }
                        }
                        else
                        {
                            inMemoryDatabase.OTPClaims[inMemoryDatabase.OTPClaims.IndexOf(otpClaim)].FailCount++;
                            if (inMemoryDatabase.OTPClaims[inMemoryDatabase.OTPClaims.IndexOf(otpClaim)].FailCount >= 3)
                            {
                                inMemoryDatabase.Accounts[inMemoryDatabase.Accounts.IndexOf(account)].status = false;
                                results.Add("Data: Too many failed attempts have occurred. Account has been disabled.");
                            }
                            else
                            {
                                results.Add("Data: The OTP has expired. Please request a new one.");
                            }
                        }
                    }
                    else
                    {
                        results.Add("Data: Incorrect Username or OTP.");
                    }
                }
                else
                {
                    results.Add("Database: No corresponding OTP Claim was found in the database.");
                }
            }
            else
            {
                results.Add("Database: The account has been disabled.");
            }
            return results;
        }

        public string VerifyAuthenticated(IRolePrincipal rolePrincipal)
        {
            if(inMemoryDatabase.RolePrincipals.Contains(rolePrincipal))
            {
                return "success";
            }
            return "Database: Your session has expired. Please login and try again.";
        }

        public string VerifyNotAuthenticated(IRolePrincipal rolePrincipal)
        {
            if (!inMemoryDatabase.RolePrincipals.Contains(rolePrincipal))
            {
                return "success";
            }
            return "Database: You are already logged in.";
        }

        public string Authorize(IRolePrincipal rolePrincipal, string requiredRole)
        {
            string result = VerifyAuthenticated(rolePrincipal);
            if (result.Equals("success"))
            {
                if(rolePrincipal.IsInRole(requiredRole))
                {
                    return result;
                }
                return "Data: You are not authorized to perform this operation.";
            }
            return result;
        }

        public string StoreOTP(IOTPClaim otpClaim)
        {
            int index = inMemoryDatabase.OTPClaims.IndexOf(otpClaim);
            if(index >= 0)
            {
                IOTPClaim dbOTPClaim = inMemoryDatabase.OTPClaims[index];
                if(!(otpClaim.TimeCreated >= dbOTPClaim.TimeCreated.AddDays(1)))
                {
                    otpClaim.FailCount = dbOTPClaim.FailCount;
                }
                inMemoryDatabase.OTPClaims[index] = otpClaim;
                return "success";
            }
            return "Database: The account does not exist.";
        }

        public List<KPI> LoadKPI(DateTime now)
        {
            List<KPI> kpiList; 
        }

        public string CreateLoginKPI()
        {
            //For sake of unit testing, assume that there are no duplicates and every instance of the 
            //LoginKPI is a representation 
        }
    }
}

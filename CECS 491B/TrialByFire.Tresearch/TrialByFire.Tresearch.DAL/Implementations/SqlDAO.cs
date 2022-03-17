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

        /*public List<IKPI> LoadKPI(DateTime now)
        {
            List<IKPI> kpiList = new List<IKPI>();
            kpiList.Add(GetViewKPI());
            kpiList.Add(GetViewDurationKPI());
            kpiList.Add(GetNodeKPI(now));
            kpiList.Add(GetLoginKPI(now));
            kpiList.Add(GetRegistrationKPI(now));
            kpiList.Add(GetSearchKPI(now));
            return kpiList;
        }*/


        //1/6
        public async Task<IViewKPI> GetViewKPI()
        {
            IViewKPI vKPI = new ViewKPI();
            try
            {
                IList<View> views = GetAllViews();
                if (views.Count == 0)
                {
                    vKPI.result = "No Database Entires";
                }
                int n = views.Count;
                for(int i = 1; i <= 5; i++){
                    vKPI.views.Add(views[(n-i)]);
                }
                vKPI.result = "Success";
                return vKPI;
            }
            catch (Exception ex)
            {
                vKPI.result = ex.Message;
                return vKPI;
            }
        }

        //2/6
        public async Task<IViewDurationKPI> GetViewDurationKPI()
        {
            IViewDurationKPI vDKPI = new ViewDurationKPI();
            try
            {
                IList<View> views = GetAllViews();
                if(views.Count == 0)
                {
                    vDKPI.result = "No Database Entries";
                    return vDKPI;
                }
                int n = views.Count;
                for(int i = 1; i < 5; i++)
                {
                    vDKPI.views.Add(views[(n - i)]);
                }
                vDKPI.result = "Success";
                return vDKPI;
            }
            catch(Exception ex)
            {
                vDKPI.result = ex.Message;
                return vDKPI;
            }
        }

        /*public INodeKPI GetNodeKPI(DateTime now)
        {
            INodeKPI nodeKPI = new NodeKPI();
            IList<INodesCreated> nC = GetNodesCreated(now);
            if (nC.Count == 0)
            {
                nodeKPI.result = "Error";
                return nodeKPI;
            }
            for (int i = 1; i < nC.Count; i++)
            {
                nodeKPI.nodesCreated.Add(nC[(nC.Count - 1)]);
            }
            nodeKPI.result = "success";
            return nodeKPI;
        }*/

        /*public INodeKPI GetNodeKPI(DateTime now)
        {
            INodeKPI nodeKPI = new NodeKPI();
            int counter = 1;
            INodesCreated nCreated = GetNodesCreated(now);
            if (nCreated.nodeCreationCount == -1)
            {
                nodeKPI.result = "Error";
                return nodeKPI;
            }
            while((counter < 29) && (nCreated.nodeCreationCount != -1))
            {
                nodeKPI.nodesCreated.Add(nCreated);
                DateTime past = now.AddDays((counter * -1));
                nCreated = GetNodesCreated(past);
                counter++;
            }
            nodeKPI.result = "success";
            return nodeKPI;
        }*/

        //3/6
        public async Task<INodeKPI> GetNodeKPI(DateTime now)
        {
            INodeKPI nKPI = new NodeKPI();
            try
            {
                IList<NodesCreated> nodes = GetNodesCreated(now);
                if(nodes.Count == 0)
                {
                    nKPI.result = "No Database Entries";
                    return nKPI;
                }
                foreach(var x in nodes)
                {
                    nKPI.nodesCreated.Add(x);
                }
                nKPI.result = "Success";
                return nKPI;
            }
            catch(Exception ex)
            {
                nKPI.result = ex.Message;
                return nKPI;
            }
        }

        /*//4/6
        public async Task<ILoginKPI> GetLoginKPI(DateTime now)
        {
            ILoginKPI loginKPI = new LoginKPI();
            int counter = 1;
            IList<IailyLogin> dLogin = GetDailyLogin(now);
            if (dLogin.loginCount == -1)
            {
                loginKPI.result = "Error";
                return loginKPI;
            }
            while ((counter <= 90) && (dLogin.loginCount != -1))
            {
                loginKPI.dailyLogins.Add(dLogin);
                DateTime past = now.AddDays((counter * -1));
                dLogin = GetDailyLogin(past);
                counter++;
            }
            loginKPI.result = "success";
            return loginKPI;
        }*/

        //4/6
        public async Task<ILoginKPI> GetLoginKPI(DateTime now)
        {
            ILoginKPI lKPI = new LoginKPI();
            try
            {
                IList<DailyLogin> logins = GetDailyLogin(now);
                if(logins.Count == 0)
                {
                    lKPI.result = "No Database Entries";
                    return lKPI;
                }
                foreach(var x in logins)
                {
                    lKPI.dailyLogins.Add(x);
                }
                lKPI.result = "Success";
                return lKPI;
            }
            catch(Exception ex)
            {
                lKPI.result = ex.Message;
                return lKPI;
            }
        }

        /*public IRegistrationKPI GetRegistrationKPI(DateTime now)
        {
            IRegistrationKPI registrationKPI = new RegistrationKPI();
            int counter = 1;
            IDailyRegistration dRegistration = GetDailyRegistration(now);
            if (dRegistration.registrationCount == -1)
            {
                registrationKPI.result = "Error";
                return registrationKPI;
            }
            while ((counter <= 90) && (dRegistration.registrationCount != -1))
            {
                registrationKPI.dailyRegistrations.Add(dRegistration);
                DateTime past = now.AddDays((counter * -1));
                dRegistration = GetDailyRegistration(past);
                counter++;
            }
            registrationKPI.result = "success";
            return registrationKPI;
        }*/
        //5/6
        public async Task<IRegistrationKPI> GetRegistrationKPI(DateTime now)
        {
            IRegistrationKPI rKPI = new RegistrationKPI();
            try
            {
                IList<DailyRegistration> registrations = GetDailyRegistration(now);
                if(registrations.Count == 0)
                {
                    rKPI.result = "No Database Entries";
                    return rKPI;
                }
                foreach(var x in registrations)
                {
                    rKPI.dailyRegistrations.Add(x);
                }
                rKPI.result = "Success";
                return rKPI;
            }
            catch(Exception ex)
            {
                rKPI.result = ex.Message;
                return rKPI;
            }
        }

        /*//6/6
        public ISearchKPI GetSearchKPI(DateTime now)
        {
            ISearchKPI searchKPI = new SearchKPI();
            int counter = 1;
            ITopSearch sCreated = GetTopSearch(now);//Initial Check to see if InMemoryDatabase is not empty
            List<ITopSearch> preSort = new List<ITopSearch>();
            if (sCreated.searchCount == -1)
            {
                searchKPI.result = "Error";
                return searchKPI;
            }

            while ((counter <= 28) && (sCreated.searchCount != -1))
            {
                preSort.Add(sCreated);
                DateTime past = now.AddDays((counter * -1));
                sCreated = GetTopSearch(past);
                counter++;
            }

            List<ITopSearch> afterSort = preSort.OrderBy(x => x.searchCount).ToList();
            int n = (afterSort.Count);
            for (int i = 1; i <= 5 || i < n; i++)
            {
                Console.WriteLine(n);
                searchKPI.topSearches.Add(afterSort[(n - i)]);
            }
            searchKPI.result = "success";
            return searchKPI;
        }*/

        //6/6
        public async Task<ISearchKPI> GetSearchKPI(DateTime now)
        {
            ISearchKPI sKPI = new SearchKPI();
            try
            {
                IList<TopSearch> searches = GetTopSearch(now);
                if(searches.Count == 0)
                {
                    sKPI.result = "No Database Entries";
                    return sKPI;
                }
                IList<TopSearch> sorted = searches.OrderBy(x => x.searchCount).ToList();
                int n = sorted.Count;
                for(int i = 1; i <= 5 || i < n; i++)
                {
                    sKPI.topSearches.Add(sorted[(n-i)]);
                }
                sKPI.result = "Success";
                return sKPI;
            }
            catch(Exception ex)
            {
                sKPI.result = ex.Message;
                return sKPI;
            }
        }


        public string CreateNodesCreated(INodesCreated nodesCreated)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionString))
                {
                    var insertQuery = @"INSERT INTO tresearchStudentServer.dbo.NodesCreated (nodesCreatedDate, nodesCreatedCount) VALUES (@nodesCreatedDate, @nodesCreatedCount)";

                    affectedRows = connection.Execute(insertQuery,
                                    new
                                    {
                                        nodesCreatedDate = nodesCreated.nodeCreationDate,
                                        nodesCreatedCount = nodesCreated.nodeCreationCount
                                    });
                }
                 
                return _messageBank.SuccessMessages["generic"];
            }
            catch (Exception ex)
            {
                return _messageBank.ErrorMessages["createdNodesExists"];
            }
        }

        public List<NodesCreated> GetNodesCreated(DateTime nodesCreatedDate)
        {
            using (var connection = new SqlConnection(_sqlConnectionString))
            {
                var selectQuery = "SELECT * FROM tresearchStudentServer.dbo.nodesCreated WHERE nodesCreatedDate BETWEEN DATEADD(day, -30, @nodeCreationDate) AND @nodeCreationDate";

                return connection.Query<NodesCreated>(selectQuery, new { nodeCreationDate = nodesCreatedDate }).ToList();
            }
        }

        public string UpdateNodesCreated(INodesCreated nodesCreated)
        {
            using (var connection = new SqlConnection(_sqlConnectionString))
            {
                string updateQuery = @"UPDATE tresearchStudentServer.dbo.nodesCreated SET nodesCreatedCount = @nodesCreatedCount WHERE nodesCreatedDate = @nodesCreatedDate";

                int rowsAffected = connection.Execute(updateQuery,
                            new
                            {
                                nodesCreatedDate = nodesCreated.nodeCreationDate,
                                nodesCreatedCount = nodesCreated.nodeCreationCount
                            }
                            );
                if (rowsAffected == 1)
                {
                    return _messageBank.SuccessMessages["generic"];
                }
                else
                {
                    return _messageBank.ErrorMessages["createdNodesNotExists"];
                }
            }
        }



        public string CreateDailyLogin(IDailyLogin dailyLogin)
        {
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionString))
                {
                    string insertQuery = @"INSERT INTO tresearchStudentServer.dbo.dailyLogins (loginDate, loginCount)
                                        Values (@loginDate, @loginCount)";
                    int affectedRows = connection.Execute(insertQuery,
                                        new
                                        {
                                            loginDate = dailyLogin.loginDate,
                                            loginCount = dailyLogin.loginCount
                                        });
                }
                return _messageBank.SuccessMessages["generic"];
            }
            catch (Exception ex)
            {
                return _messageBank.ErrorMessages["dailyLoginsExists"];
            }
        }

        public List<DailyLogin> GetDailyLogin(DateTime getDate)
        {
            using (var connection = new SqlConnection(_sqlConnectionString))
            {
                var selectQuery = "SELECT * FROM tresearchStudentServer.dbo.dailyLogins WHERE loginDate BETWEEN DATEADD(day, -30, @loginDate) AND @loginDate";

                return connection.Query<DailyLogin>(selectQuery, new { loginDate = getDate }).ToList();
            }
        }

        //Done
        public IList<View> GetAllViews()
        {
            using (var connection = new SqlConnection(_sqlConnectionString))
            {
                //return connection.Execute<IView>("SELECT DateCreated, ViewName, Visits, AverageDuration from dbo.ViewTable").ToList();
                var selectQuery = "SELECT DateCreated, ViewName, Visits, AverageDuration FROM dbo.ViewTable";
                List<View> results = connection.Query<View>(selectQuery).ToList();
                return results;
            }
        }

        public string UpdateDailyLogin(IDailyLogin dailyLogin)
        {
            using (var connection = new SqlConnection(_sqlConnectionString))
            {
                string updateQuery = @"UPDATE tresearchStudentServer.dbo.dailyLogins SET loginCount = @loginCount WHERE loginDate = @loginDate";

                int rowsAffected = connection.Execute(updateQuery, new
                {
                    loginDate = dailyLogin.loginDate,
                    loginCount = dailyLogin.loginCount
                });

                if (rowsAffected == 1)
                {
                    return _messageBank.SuccessMessages["generic"];
                }
                else
                {
                    return _messageBank.ErrorMessages["dailyLoginsNotExists"];
                }
            }
        }



        public string CreateTopSearch(ITopSearch topSearch)
        {
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionString))
                {
                    string insertQuery = @"INSERT INTO tresearchStudentServer.dbo.TopSearches (topSearchDate, topSearchString, topSearchCount) VALUES (@topSearchDate, @topSearchString, @topSearchCount)";

                    int affectedRows = connection.Execute(insertQuery,
                                        new
                                        {
                                            topSearchDate = topSearch.topSearchDate,
                                            topSearchString = topSearch.searchString,
                                            topSearchCount = topSearch.searchCount
                                        });
                }

                return _messageBank.SuccessMessages["generic"];
            }
            catch (Exception ex)
            {
                return _messageBank.ErrorMessages["topSearchesExists"];
            }
        }

        public List<TopSearch> GetTopSearch(DateTime getTopSearchDate)
        {
            using (var connection = new SqlConnection(_sqlConnectionString))
            {
                string selectQuery = "SELECT * FROM tresearchStudentServer.dbo.topSearches WHERE topSearchDate BETWEEN DATEADD(day, -30, @topSearchDate) AND @topSearchDate;";

                return connection.Query<TopSearch>(selectQuery, new { topSearchDate = getTopSearchDate }).ToList();
            }
        }

        public string UpdateTopSearch(ITopSearch topSearch)
        {
            using (var connection = new SqlConnection(_sqlConnectionString))
            {
                string updateQuery = @"UPDATE tresearchStudentServer.dbo.topSearches SET topSearchCount = @topSearchCount, topSearchString = @topSearchString WHERE topSearchDate = @topSearchDate;";

                int rowsAffected = connection.Execute(updateQuery,
                                                    new
                                                    {
                                                        topSearchDate = topSearch.topSearchDate,
                                                        topSearchString = topSearch.searchCount,
                                                        topSearchCount = topSearch.searchCount
                                                    });
                if (rowsAffected == 1)
                {
                    return _messageBank.SuccessMessages["generic"];
                }
                else
                {
                    return _messageBank.ErrorMessages["topSearchesNotExists"];
                }
            }
        }



        public string CreateDailyRegistration(IDailyRegistration dailyRegistration)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionString))
                {
                    var insertQuery = @"INSERT INTO tresearchStudentServer.dbo.dailyRegistrations (registrationDate, registrationCount) VALUES (@registrationDate, @registrationCount)";
                    affectedRows = connection.Execute(insertQuery,
                                     new
                                     {
                                         registrationDate = dailyRegistration.registrationDate,
                                         registrationCount = dailyRegistration.registrationCount
                                     });
                }
                return _messageBank.SuccessMessages["generic"];
            }
            catch (Exception ex)
            {
                return _messageBank.ErrorMessages["dailyRegistrationsExists"];
            }
        }

        public string CreateView(IView view)
        {
            int affectedRows;
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionString))
                {
                    var insertQuery = @"INSERT INTO dbo.ViewTable (DateCreated, ViewName, Visits, AverageDuration)" +
                        "Values (@DateCreated, @ViewName, @Visits, @AverageDuration)";
                    affectedRows = connection.Execute(insertQuery, new
                    {
                        DateCreated = view.date,
                        ViewName = view.viewName,
                        Visits = view.visits,
                        AverageDuration = view.averageDuration
                    });
                }
                if (affectedRows == 1)
                {
                    return "View Creation Successful";
                }
                else
                {
                    return "View Creation Failed";
                }
            }
            catch (Exception ex)
            {
                return "Fail";
            }
        }

        public List<DailyRegistration> GetDailyRegistration(DateTime getRegistrationDate)
        {
            using (var connection = new SqlConnection(_sqlConnectionString))
            {
                var selectQuery = "SELECT * FROM tresearchStudentServer.dbo.dailyRegistrations WHERE registrationDate BETWEEN DATEADD(day, -30, @registrationDate) AND @registrationDate";

                return connection.Query<DailyRegistration>(selectQuery, new { registrationDate = getRegistrationDate }).ToList();
            }
        }

        public string UpdateDailyRegistration(IDailyRegistration dailyRegistration)
        {
            using (var connection = new SqlConnection(_sqlConnectionString))
            {
                string updateQuery = @"UPDATE tresearchStudentServer.dbo.dailyRegistrations SET registrationCount = @registrationCount WHERE registrationDate = @registrationDate";

                int rowsAffected = connection.Execute(updateQuery,
                                new
                                {
                                    registrationDate = dailyRegistration.registrationDate,
                                    registrationCount = dailyRegistration.registrationCount
                                });

                if (rowsAffected == 1)
                {
                    return _messageBank.SuccessMessages["generic"];
                }
                else
                {
                    return _messageBank.ErrorMessages["dailyRegistrationsNotExists"];
                }
            }
        }
    }
}
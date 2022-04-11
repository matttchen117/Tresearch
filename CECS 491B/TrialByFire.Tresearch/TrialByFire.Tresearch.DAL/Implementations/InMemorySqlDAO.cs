using Dapper;
using System;
using System.Data.SqlClient;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Exceptions;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.DAL.Implementations
{
    public class InMemorySqlDAO : ISqlDAO
    {

        public InMemoryDatabase InMemoryDatabase { get; set; }
        private IMessageBank _messageBank { get; }

        public InMemorySqlDAO()
        {
            InMemoryDatabase = new InMemoryDatabase();
            _messageBank = new MessageBank();
        }

       /* public List<IKPI> LoadKPI(DateTime now)
        {
            throw new NotImplementedException();
        }

        INodesCreated ISqlDAO.GetNodesCreated(DateTime nodeCreationDate)
        {
            throw new NotImplementedException();
        }

        IDailyLogin ISqlDAO.GetDailyLogin(DateTime nodeCreationDate)
        {
            throw new NotImplementedException();
        }

        ITopSearch ISqlDAO.GetTopSearch(DateTime nodeCreationDate)
        {
            throw new NotImplementedException();
        }

        IDailyRegistration ISqlDAO.GetDailyRegistration(DateTime nodeCreationDate)
        {
            throw new NotImplementedException();
        }*/

        public async Task<int> LogoutAsync(IAccount account, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            int index = InMemoryDatabase.Accounts.IndexOf(account);
            if(index != -1)
            {
                InMemoryDatabase.Accounts[index].Token = null;
            }
            if(InMemoryDatabase.Accounts[index].Token == null)
            {
                return 1;
            }
            return 0;
        }
        public async Task<string> StoreLogAsync(ILog log, CancellationToken cancellationToken = default)
        {
            InMemoryDatabase.Logs.Add(log);
            return await _messageBank.GetMessage(IMessageBank.Responses.generic).ConfigureAwait(false);
        }
        public async Task<int> VerifyAccountAsync(IAccount account, 
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            int index = InMemoryDatabase.Accounts.IndexOf(account);
            if (index != -1)
            {
                IAccount dbAccount = InMemoryDatabase.Accounts[index];
                if (dbAccount.Confirmed != false)
                {
                    if (dbAccount.AccountStatus != false)
                    {
                        return 1;
                    }
                    return 3;
                }
                return 2;
            }
            return 0;
        }

        public async Task<int> AuthenticateAsync(IOTPClaim otpClaim, string jwtToken,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<string> results = new List<string>();
            IAccount account = new Account(otpClaim.Username, otpClaim.AuthorizationLevel);
            // Find account in db
            int index = InMemoryDatabase.Accounts.IndexOf(account);
            if (index >= 0)
            {
                IAccount dbAccount = InMemoryDatabase.Accounts[index];
                // find otp claim in db
                index = InMemoryDatabase.OTPClaims.IndexOf(otpClaim);
                IOTPClaim dbOTPClaim = InMemoryDatabase.OTPClaims[index];
                // if otps do not match
                if (!otpClaim.OTP.Equals(dbOTPClaim.OTP))
                {
                    // increment fail count
                    ++InMemoryDatabase.OTPClaims[InMemoryDatabase.OTPClaims.IndexOf(otpClaim)].FailCount;
                    // if fail count is 5 or more, disable account
                    if (InMemoryDatabase.OTPClaims[InMemoryDatabase.OTPClaims.IndexOf(otpClaim)].FailCount >= 5)
                    {
                        return 4;
                    }
                    else
                    {
                        return 3;
                    }
                }
                // check that the otp was entered within 2 minutes of being created
                if ((otpClaim.TimeCreated >= dbOTPClaim.TimeCreated) && (otpClaim.TimeCreated <= dbOTPClaim.TimeCreated.AddMinutes(2)))
                {
                    InMemoryDatabase.Accounts[index].Token = jwtToken;
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 0;
            }
        }

        public async Task<int> StoreOTPAsync(IAccount account, IOTPClaim otpClaim, 
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            int index = InMemoryDatabase.Accounts.IndexOf(account);
            if (index >= 0)
            {
                if(account.Passphrase.Equals(InMemoryDatabase.Accounts[index].Passphrase))
                {
                    index = InMemoryDatabase.OTPClaims.IndexOf(otpClaim);
                    if (index >= 0)
                    {
                        IOTPClaim dbOTPClaim = InMemoryDatabase.OTPClaims[InMemoryDatabase.OTPClaims.IndexOf(otpClaim)];
                        if (!(otpClaim.TimeCreated >= dbOTPClaim.TimeCreated.AddDays(1)))
                        {
                            otpClaim.FailCount = dbOTPClaim.FailCount;
                        }
                        InMemoryDatabase.OTPClaims[index] = otpClaim;
                        return 1;
                    }
                    return 3;
                }
                return 2;
            }
            return 0;
        }


        public string DeleteAccount()
        {
            bool accountExists = false;
            string accountName = Thread.CurrentPrincipal.Identity.Name;
            string accountRole = Thread.CurrentPrincipal.IsInRole("admin") ? "admin" : "user";
            try
            {
                for (int i = 0; i < InMemoryDatabase.Accounts.Count; i++)
                {
                    if ((InMemoryDatabase.Accounts[i].Username.Equals(accountName)) && (InMemoryDatabase.Accounts[i].AuthorizationLevel.Equals(accountRole)))
                    {
                        accountExists = true;
                        InMemoryDatabase.Accounts.RemoveAt(i);
                        break;
                    }
                }
                if (accountExists)
                {
                    for (int i = 0; i < InMemoryDatabase.OTPClaims.Count; i++)
                    {
                        if (InMemoryDatabase.OTPClaims[i].Username.Equals(accountName))

                        {
                            InMemoryDatabase.OTPClaims.RemoveAt(i);
                            break;
                        }
                    }
                    for (int i = 0; i < InMemoryDatabase.Nodes.Count; i++)
                    {
                        if (InMemoryDatabase.Nodes[i].accountOwner.Equals(accountName))
                        {
                            InMemoryDatabase.Nodes.RemoveAt(i);
                        }
                    }
                    for (int i = 0; i < InMemoryDatabase.Ratings.Count; i++)
                    {
                        if (InMemoryDatabase.Ratings[i].username.Equals(accountName))
                        {
                            InMemoryDatabase.Ratings.RemoveAt(i);
                        }
                    }
                    for (int i = 0; i < InMemoryDatabase.ConfirmationLinks.Count; i++)
                    {
                        if (InMemoryDatabase.ConfirmationLinks[i].Username.Equals(accountName))
                        {
                            InMemoryDatabase.ConfirmationLinks.RemoveAt(i);
                            break;
                        }
                    }
                    return _messageBank.SuccessMessages["generic"];
                }
                else
                {
                    return _messageBank.ErrorMessages["accountNotFound"];
                }
            }
            catch (AccountDeletionFailedException adfe)
            {
                return adfe.Message;
            }

        }




        public async Task<string> CreateAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (InMemoryDatabase.Accounts.Contains(account))
                    return _messageBank.GetMessage(IMessageBank.Responses.accountAlreadyCreated).Result;

                InMemoryDatabase.Accounts.Add(account);
                if (cancellationToken.IsCancellationRequested)
                {
                    InMemoryDatabase.Accounts.Remove(account);
                    throw new OperationCanceledException();
                }
                
                return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
            }
            catch (OperationCanceledException)
            {
                // No rollback necessary
                throw;
            }
            catch(Exception ex)
            {
                return _messageBank.GetMessage(IMessageBank.Responses.accountCreateFail).Result;
            }

        }

        public async Task<Tuple<IAccount, string>> GetAccountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            IAccount nullAccount = null;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                for (int i = 0; i < InMemoryDatabase.Accounts.Count(); i++)
                    if (email.Equals(InMemoryDatabase.Accounts[i].Email))
                        return Tuple.Create(InMemoryDatabase.Accounts[i], _messageBank.GetMessage(IMessageBank.Responses.generic).Result);
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();
                return Tuple.Create(nullAccount, _messageBank.GetMessage(IMessageBank.Responses.accountNotFound).Result);
            }
            catch (OperationCanceledException)
            {
                //No rollback necessary
                throw;
            }
            catch(Exception ex)
            {
                return Tuple.Create(nullAccount, "500: Database: " + ex.Message);
            }
        }

        public async Task<string> RemoveConfirmationLinkAsync(IConfirmationLink confirmationLink, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                InMemoryDatabase.ConfirmationLinks.Remove(confirmationLink);
                if (cancellationToken.IsCancellationRequested)
                {
                    string rollbackResult = await CreateConfirmationLinkAsync(confirmationLink);
                    if (rollbackResult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                        return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                    else
                        throw new OperationCanceledException();
                }
                return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
            }
            catch (OperationCanceledException)
            {
                //No rollback necessary
                throw;
            }
            catch (Exception ex)
            {
                return "500: Database: " + ex.Message;
            }
        }

        public async Task<string> UpdateAccountToUnconfirmedAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                for (int i = 0; i < InMemoryDatabase.Accounts.Count(); i++)
                {
                    if (email == InMemoryDatabase.Accounts[i].Email && authorizationLevel == InMemoryDatabase.Accounts[i].AuthorizationLevel)
                    {
                        InMemoryDatabase.Accounts[i].Confirmed = false;
                        return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
                    }
                }
                return _messageBank.GetMessage(IMessageBank.Responses.accountNotFound).Result;
            }
            catch (OperationCanceledException)
            {
                //No rollback necessary
                throw;
            }
            catch (Exception ex)
            {
                return "500: Database: " + ex.Message;
            }
        }
        public async Task<string> UpdateAccountToConfirmedAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                for(int i = 0; i < InMemoryDatabase.Accounts.Count(); i++)
                {
                    if (email == InMemoryDatabase.Accounts[i].Email && authorizationLevel == InMemoryDatabase.Accounts[i].AuthorizationLevel)
                    {
                        InMemoryDatabase.Accounts[i].Confirmed = true;
                        return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
                    }
                }
                return _messageBank.GetMessage(IMessageBank.Responses.accountNotFound).Result;
            }
            catch (OperationCanceledException)
            {
                //No rollback necessary
                throw;
            }
            catch (Exception ex)
            {
                return "500: Database: " + ex.Message;
            }
        }
        public async Task<string> CreateConfirmationLinkAsync(IConfirmationLink confirmationLink, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                InMemoryDatabase.ConfirmationLinks.Add(confirmationLink);
                if (cancellationToken.IsCancellationRequested)
                {
                    string rollbackResult = await RemoveConfirmationLinkAsync(confirmationLink);
                    if (rollbackResult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                        return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                    else
                        throw new OperationCanceledException();
                }
                return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
            }
            catch (OperationCanceledException)
            {
                //No rollback necessary
                throw;
            }
            catch (Exception ex)
            {
                return "500: Database: " + ex.Message;
            }
        }

        public async Task<Tuple<IConfirmationLink, string>> GetConfirmationLinkAsync(string guid, CancellationToken cancellationToken = default(CancellationToken))
        {
            IConfirmationLink nullLink = null;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                string guidString = guid.Substring(guid.LastIndexOf('=') + 1);
                Guid toFind = new Guid(guidString);
                for (int i = 0; i < InMemoryDatabase.ConfirmationLinks.Count(); i++)
                    if (toFind.Equals(InMemoryDatabase.ConfirmationLinks[i].GUIDLink))
                        return Tuple.Create(InMemoryDatabase.ConfirmationLinks[i], _messageBank.GetMessage(IMessageBank.Responses.generic).Result);
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();
                return Tuple.Create(nullLink, _messageBank.GetMessage(IMessageBank.Responses.confirmationLinkNotFound).Result);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return Tuple.Create(nullLink, "500: Database: " + ex.Message);
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

        //1
        public async Task<IViewKPI> GetViewKPIAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            IViewKPI viewKPI = new ViewKPI();
            try
            {
                List<IView> ordered = InMemoryDatabase.Views.OrderBy(x => x.visits).ToList();
                if (ordered.Count == 0)
                {
                    viewKPI.result = "No Database Entries";
                }
                int n = ordered.Count;
                for (int i = 1; i <= 5; i++)
                {
                    viewKPI.views.Add(ordered[(n - i)]);
                }
                viewKPI.result = "success";
                return viewKPI;
            }
            catch(Exception ex)
            {
                viewKPI.result = ("500: Database: " + ex.Message);
                return viewKPI;
            }
        }

        //2
        public async Task<IViewDurationKPI> GetViewDurationKPIAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            IViewDurationKPI viewDurationKPI = new ViewDurationKPI();
            try
            {
                List<IView> ordered = InMemoryDatabase.Views.OrderBy(x => x.averageDuration).ToList();
                if (ordered.Count == 0)
                {
                    viewDurationKPI.result = "Error";
                    return viewDurationKPI;
                }
                int n = ordered.Count;
                for (int i = 1; i < 5; i++)
                {
                    viewDurationKPI.views.Add(ordered[(n - 1)]);
                }
                viewDurationKPI.result = "success";
                return viewDurationKPI;
            }
            catch(Exception ex)
            {
                viewDurationKPI.result = ("500: Database: " + ex.Message);
                return viewDurationKPI;
            }
        }

        public async Task<INodeKPI> GetNodeKPIAsync(DateTime now, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            INodeKPI nodeKPI = new NodeKPI();
            try
            {
                List<NodesCreated> nCreated = await GetNodesCreatedAsync(now, cancellationToken).ConfigureAwait(false);//Initial Check to see if InMemoryDatabase is not empty
                if (nCreated.Count == 0)
                {
                    nodeKPI.result = "Error";
                    return nodeKPI;
                }
                foreach (var x in nCreated)
                {
                    nodeKPI.nodesCreated.Add(x);
                }
                nodeKPI.result = "success";
                return nodeKPI;
            }
            catch(Exception ex)
            {
                nodeKPI.result = ("500: Database: " + ex.Message);
                return nodeKPI;
            }
        }

        //4
        public async Task<ILoginKPI> GetLoginKPIAsync(DateTime now, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ILoginKPI loginKPI = new LoginKPI();
            try
            {
                List<DailyLogin> dLogin = await GetDailyLoginAsync(now, cancellationToken).ConfigureAwait(false);
                if (dLogin.Count == 0)
                {
                    loginKPI.result = "Error";
                    return loginKPI;
                }
                foreach (var x in dLogin)
                {
                    loginKPI.dailyLogins.Add(x);
                }
                loginKPI.result = "success";
                return loginKPI;
            }
            catch(Exception ex)
            {
                loginKPI.result = ("500: Databaes: " + ex.Message);
                return loginKPI;
            }
        }

        //5
        public async Task<IRegistrationKPI> GetRegistrationKPIAsync(DateTime now, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            IRegistrationKPI registrationKPI = new RegistrationKPI();
            try
            {
                List<DailyRegistration> dRegistration = await GetDailyRegistrationAsync(now, cancellationToken).ConfigureAwait(false);
                if (dRegistration.Count == 0)
                {
                    registrationKPI.result = "Error";
                    return registrationKPI;
                }
                foreach (var x in dRegistration)
                {
                    registrationKPI.dailyRegistrations.Add(x);
                }
                registrationKPI.result = "success";
                return registrationKPI;
            }
            catch(Exception ex)
            {
                registrationKPI.result = ("500: Database: " + ex.Message);
                return registrationKPI;
            }
        }

        //6
        public async Task<ISearchKPI> GetSearchKPIAsync(DateTime now, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ISearchKPI searchKPI = new SearchKPI();
            try
            {
                List<TopSearch> sCreated = await GetTopSearchAsync(now, cancellationToken).ConfigureAwait(false);//Initial Check to see if InMemoryDatabase is not empty
                if (sCreated.Count == 0)
                {
                    searchKPI.result = "Error";
                    return searchKPI;
                }
                List<TopSearch> sorted = sCreated.OrderBy(x => x.searchCount).ToList();
                int n = (sorted.Count);
                for (int i = 1; i <= 5 || i < n; i++)
                {
                    Console.WriteLine(n);
                    searchKPI.topSearches.Add(sorted[(n - i)]);
                }
                searchKPI.result = "success";
                return searchKPI;
            }
            catch(Exception ex)
            {
                searchKPI.result = ("500: Database: " + ex.Message);
                return searchKPI;
            }
        }

        public string CreateNodesCreated(INodesCreated nodesCreated)
        {
            // Check whether the NodesCreated object exists already
            foreach (INodesCreated nodesCreated1 in InMemoryDatabase.NodesCreated)
            {
                if (nodesCreated1.nodeCreationDate == nodesCreated.nodeCreationDate)
                {
                    return _messageBank.ErrorMessages["createdNodesExists"];
                }
            }

            InMemoryDatabase.NodesCreated.Add(nodesCreated);

            return _messageBank.SuccessMessages["generic"];

        }

        public async Task<List<NodesCreated>> GetNodesCreatedAsync(DateTime nodeCreationDate, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<NodesCreated> nodeResult = new List<NodesCreated>();
            try
            {
                foreach (INodesCreated nodesCreated in InMemoryDatabase.NodesCreated)
                {
                    if (nodeCreationDate <= nodesCreated.nodeCreationDate && nodeCreationDate >= nodeCreationDate.Date.AddDays(-30))
                    {
                        nodeResult.Add((NodesCreated)nodesCreated);
                    }
                }
                return nodeResult;
            }
            catch(Exception ex)
            {
                return nodeResult;
            }
        }

        public string UpdateNodesCreated(INodesCreated nodesCreated)
        {
            for (int i = 0; i < InMemoryDatabase.NodesCreated.Count; i++)
            {
                if (InMemoryDatabase.NodesCreated[i].nodeCreationDate == nodesCreated.nodeCreationDate)
                {
                    InMemoryDatabase.NodesCreated[i] = nodesCreated;

                    return _messageBank.SuccessMessages["generic"];
                }
            }

            return _messageBank.ErrorMessages["createdNodeNotExist"];
        }



        public string CreateDailyLogin(IDailyLogin dailyLogin)
        {
            // Check whether the daily login already exists in the database
            foreach (IDailyLogin dailyLogin1 in InMemoryDatabase.DailyLogins)
            {
                if (dailyLogin1.loginDate == dailyLogin.loginDate)
                {
                    return _messageBank.ErrorMessages["dailyLoginsExists"];
                }
            }

            InMemoryDatabase.DailyLogins.Add(dailyLogin);

            return _messageBank.SuccessMessages["generic"];
        }

        public async Task<List<DailyLogin>> GetDailyLoginAsync(DateTime loginDate, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
           List<DailyLogin> dailyLoginResults = new List<DailyLogin>();
            try
            {
                foreach (IDailyLogin dailyLogin1 in InMemoryDatabase.DailyLogins)
                {
                    if (dailyLogin1.loginDate <= loginDate && dailyLogin1.loginDate >= loginDate.Date.AddDays(-30))
                    {
                        dailyLoginResults.Add((DailyLogin)dailyLogin1);
                    }
                }
                return dailyLoginResults;
            }
            catch(Exception ex)
            {
                return dailyLoginResults;
            }
        }

        public string UpdateDailyLogin(IDailyLogin dailyLogin)
        {
            for (int i = 0; i < InMemoryDatabase.DailyLogins.Count; i++)
            {
                if (InMemoryDatabase.DailyLogins[i].loginDate == dailyLogin.loginDate)
                {
                    InMemoryDatabase.DailyLogins[i] = dailyLogin;

                    return "success";
                }
            }

            return _messageBank.ErrorMessages["dailyLoginNotExist"];
        }



        public string CreateTopSearch(ITopSearch topSearch)
        {
            foreach (ITopSearch topSearch1 in InMemoryDatabase.TopSearches)
            {
                if (topSearch1.topSearchDate == topSearch.topSearchDate)
                {
                    return _messageBank.ErrorMessages["topSearchExists"];
                }
            }

            InMemoryDatabase.TopSearches.Add(topSearch);

            return _messageBank.SuccessMessages["generic"];
        }

        public async Task<List<TopSearch>> GetTopSearchAsync(DateTime topSearchDate, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<TopSearch> topSearchResult = new List<TopSearch>();
            try
            {
                foreach (ITopSearch topSearch in InMemoryDatabase.TopSearches)
                {
                    if (topSearch.topSearchDate <= topSearchDate && topSearch.topSearchDate >= topSearchDate.Date.AddDays(-30))
                    {
                        topSearchResult.Add((TopSearch)topSearch);
                    }
                }
                return topSearchResult;
            }
            catch
            {
                return topSearchResult;
            }
        }

        public string UpdateTopSearch(ITopSearch topSearch)
        {
            for (int i = 0; i < InMemoryDatabase.TopSearches.Count; i++)
            {
                if (topSearch.topSearchDate == InMemoryDatabase.TopSearches[i].topSearchDate)
                {
                    InMemoryDatabase.TopSearches[i] = topSearch;

                    return _messageBank.SuccessMessages["generic"];
                }
            }

            return _messageBank.ErrorMessages["topSearchNotExist"];
        }



        public string CreateDailyRegistration(IDailyRegistration dailyRegistration)
        {
            foreach (IDailyRegistration dailyRegistration1 in InMemoryDatabase.DailyRegistrations)
            {
                if (dailyRegistration1.registrationDate == dailyRegistration.registrationDate)
                {
                    return _messageBank.ErrorMessages["dailyRegistrationExists"];
                }
            }

            InMemoryDatabase.DailyRegistrations.Add(dailyRegistration);

            return _messageBank.SuccessMessages["generic"];
        }

        public async Task<List<DailyRegistration>> GetDailyRegistrationAsync(DateTime dailyRegistrationDate, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<DailyRegistration> dailyRegistrationResults = new List<DailyRegistration>();
            try
            {
                foreach (IDailyRegistration dailyRegistration in InMemoryDatabase.DailyRegistrations)
                {
                    if (dailyRegistration.registrationDate <= dailyRegistrationDate && dailyRegistration.registrationDate >= dailyRegistrationDate.Date.AddDays(-30))
                    {
                        dailyRegistrationResults.Add((DailyRegistration)dailyRegistration);
                    }
                }
                return dailyRegistrationResults;
            }
            catch
            {
                return dailyRegistrationResults;
            }
        }

        public string UpdateDailyRegistration(IDailyRegistration dailyRegistration)
        {
            for (int i = 0; i < InMemoryDatabase.DailyRegistrations.Count; i++)
            {
                if (InMemoryDatabase.DailyRegistrations[i].registrationDate == dailyRegistration.registrationDate)
                {
                    InMemoryDatabase.DailyRegistrations[i] = dailyRegistration;

                    return _messageBank.SuccessMessages["generic"];
                }
            }

            return _messageBank.ErrorMessages["dailyRegistrationNotExist"];
        }


        public string CreateView(IView view)
        {
            foreach (IView view1 in InMemoryDatabase.Views)
            {
                if(view1.date == view.date)
                {
                    return "View Already Exists in the Database";
                }
            }

            InMemoryDatabase.Views.Add(view);
            return "View Successfully Added to the Database";
        }

        public async Task<List<View>> GetAllViewsAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<View> viewList = new List<View>();
            return viewList;
        }

        public async Task<Tuple<IRecoveryLink, string>> GetRecoveryLinkAsync(string guid, CancellationToken cancellationToken)
        {
            IRecoveryLink nullLink = null;
            return Tuple.Create(nullLink, "200");
        }

        public async Task<Tuple<int, string>> GetTotalRecoveryLinksAsync(string email, string authorizationLevel, CancellationToken cancellationToken)
        {
            return Tuple.Create(-1, "500");
        }

        public async Task<Tuple<int, string>> RemoveAllRecoveryLinksAsync(string email, string authorizationLevel, CancellationToken cancellationToken)
        {
            return Tuple.Create(-1, "500");
        }

        public async Task<string> RemoveRecoveryLinkAsync(IRecoveryLink recoveryLink, CancellationToken cancellationToken)
        {
            return "500";
        }

        public async Task<string> EnableAccountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            return "500";
        }

        public async Task<string> DisableAccountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            return "500";
        }

        public async Task<string> CreateRecoveryLinkAsync(IRecoveryLink recoveryLink, CancellationToken cancellationToken = default(CancellationToken))
        {
            return "500";
        }

        public async Task<string> IncrementRecoveryLinkCountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            return "500";
        }

        public async Task<string> DecrementRecoveryLinkCountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            return "500";
        }

        public async Task<int> GetRecoveryLinkCountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            return -1;
        }

        public async Task<string> AddTagToNodesAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return "200";
        }

        public async Task<string> RemoveTagFromNodeAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return "200";
        }

        public async Task<Tuple<List<string>, string>> GetNodeTagsAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Tuple.Create(new List<string>(), "200");
        }

        public async Task<string> CreateTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return "200";
        }

        public async Task<Tuple<List<string>, string>> GetTagsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Tuple.Create(new List<string>(), "200");
        }

        public async Task<string> RemoveTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return "200";
        }

        public async Task<string> CreateNodeAsync(INode node, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                bool nodeExists = false;
                foreach (var n in InMemoryDatabase.Nodes)
                {
                    if (node.nodeID == n.nodeID)
                    {
                        nodeExists = true;
                    }
                }
                if (nodeExists)
                {
                    return _messageBank.GetMessage(IMessageBank.Responses.nodeAlreadyExists).Result;
                }

                InMemoryDatabase.Nodes.Add(node);
                if (cancellationToken.IsCancellationRequested)
                {
                    InMemoryDatabase.Nodes.Remove(node);
                    throw new OperationCanceledException();
                }

                return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return _messageBank.GetMessage(IMessageBank.Responses.createNodeFail).Result;
            }
        }

        public async Task<string> DeleteNodeAsync(long nodeID, long parentID, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                bool nodeExists = false;
                Node targetNode;
                foreach (Node n in InMemoryDatabase.Nodes)
                {
                    if (nodeID == n.nodeID)
                    {
                        nodeExists = true;
                    }
                }

                if (!nodeExists)
                {
                    return _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound).Result;
                }

                List<Node> children = new List<Node>();
                foreach (Node c in InMemoryDatabase.Nodes)
                {
                    if (c.parentNodeID == nodeID)
                    {
                        children.Add(c);
                    }
                }
                foreach (Node n in children)
                {
                    n.parentNodeID = parentID;
                }

                foreach (Node n in InMemoryDatabase.Nodes)
                {
                    if (nodeID == n.nodeID)
                    {
                        InMemoryDatabase.Nodes.Remove(n);
                    }
                }
                return _messageBank.GetMessage(IMessageBank.Responses.deleteNodeSuccess).Result;

            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return "500: Database: " + ex.Message;
            }
        }

        public async Task<Tuple<INode, string>> GetNodeAsync(long nID, CancellationToken cancellationToken = default)
        {
            INode nullNode = null;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                Node node;
                foreach (Node n in InMemoryDatabase.Nodes)
                {
                    if (nID.Equals(n.nodeID))
                    {
                        INode temp;
                        temp = n;
                        return Tuple.Create(temp, _messageBank.GetMessage(IMessageBank.Responses.generic).Result);
                    }
                }
                if (cancellationToken.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }
                return Tuple.Create(nullNode, _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound).Result);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return Tuple.Create(nullNode, "500: Database: " + ex.Message);
            }
        }

        Task<string> ISqlDAO.UpdateNodeAsync(INode updatedNode, INode previousNode, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<Tuple<List<INode>, string>> ISqlDAO.GetNodeChildren(long nID, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

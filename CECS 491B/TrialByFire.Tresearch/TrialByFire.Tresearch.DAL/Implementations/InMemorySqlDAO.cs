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

        public async Task<IResponse<IEnumerable<Node>>> SearchForNodeAsync(ISearchInput searchInput)
        {
            try
            {
                IList<Node> nodes = new List<Node>();
                foreach(Node n in InMemoryDatabase.Nodes)
                {
                    if(n.Visibility != false && n.Deleted != true)
                    {
                        if (n.NodeTitle.Contains(searchInput.Search, StringComparison.OrdinalIgnoreCase))
                        {
                            n.Tags = InMemoryDatabase.NodeTags.Where(nt => nt.NodeID == n.NodeID).ToList();
                            n.RatingScore = InMemoryDatabase.NodeRatings.Where(nr => nr.NodeID == n.NodeID).Sum(nr => nr.Rating);
                            nodes.Add(n);
                        }
                    }
                }
                return new SearchResponse<IEnumerable<Node>>("", nodes, 200, true);
            }
            catch (Exception ex)
            {
                return new SearchResponse<IEnumerable<Node>>(await _messageBank.GetMessage(
                    IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 400, false);
            }
        }

        public async Task<string> GetUserHashAsync(IAccount account, CancellationToken cancellationToken = default)
        {
            foreach(UserHashObject u in InMemoryDatabase.UserHashTable)
            {
                if(u.UserID.Equals(account.Username) && u.UserRole.Equals(account.AuthorizationLevel))
                {
                    return u.UserHash;
                }
            }
            return null;
        }

        public async Task<int> StoreLogAsync(ILog log, string destination, CancellationToken cancellationToken = default)
        {
            switch(destination)
            {
                case "AnalyticLogs":
                    InMemoryDatabase.AnalyticLogs.Add(log);
                    if (InMemoryDatabase.AnalyticLogs.Contains(log))
                    {
                        return 1;
                    }
                    break;
                case "ArchiveLogs":
                    InMemoryDatabase.ArchiveLogs.Add(log);
                    if (InMemoryDatabase.ArchiveLogs.Contains(log))
                    {
                        return 1;
                    }
                    break;
                default:
                    return 0;
            };
            return 0;
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

        public async Task<int> AuthenticateAsync(IAuthenticationInput authenticationInput,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<string> results = new List<string>();
            IAccount account = new UserAccount(authenticationInput.OTPClaim.Username, 
                authenticationInput.OTPClaim.AuthorizationLevel);
            // Find account in db
            int index = InMemoryDatabase.Accounts.IndexOf(account);
            if (index >= 0)
            {
                IAccount dbAccount = InMemoryDatabase.Accounts[index];
                // find otp claim in db
                index = InMemoryDatabase.OTPClaims.IndexOf(authenticationInput.OTPClaim);
                IOTPClaim dbOTPClaim = InMemoryDatabase.OTPClaims[index];
                // if otps do not match
                if (!authenticationInput.OTPClaim.OTP.Equals(dbOTPClaim.OTP))
                {
                    // increment fail count
                    ++InMemoryDatabase.OTPClaims[InMemoryDatabase.OTPClaims.IndexOf(authenticationInput.OTPClaim)].FailCount;
                    // if fail count is 5 or more, disable account
                    if (InMemoryDatabase.OTPClaims[InMemoryDatabase.OTPClaims.IndexOf(authenticationInput.OTPClaim)].FailCount >= 5)
                    {
                        return 4;
                    }
                    else
                    {
                        return 3;
                    }
                }
                // check that the otp was entered within 2 minutes of being created
                if ((authenticationInput.OTPClaim.TimeCreated >= dbOTPClaim.TimeCreated) && (authenticationInput.OTPClaim.TimeCreated <= dbOTPClaim.TimeCreated.AddMinutes(2)))
                {
                    InMemoryDatabase.Accounts[index].Token = authenticationInput.UserAccount.Token;
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

        public async Task<string> IsAuthorizedToMakeNodeChangesAsync(List<long> nodeIDs, string userHash, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Check if node exists
                foreach (long nodeID in nodeIDs)
                {
                    bool isValid = false;
                    int count = -1;
                    foreach(Node node in InMemoryDatabase.Nodes)
                    {
                        if (node.NodeID == nodeID)
                        {
                            isValid = true;
                            count = InMemoryDatabase.Nodes.IndexOf(node);
                            if (!node.UserHash.Equals(userHash))
                                return await _messageBank.GetMessage(IMessageBank.Responses.notAuthorized);
                        }  
                    }

                    if(!isValid)
                        return await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound).ConfigureAwait(false);
                }

                return await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess).ConfigureAwait(false);

            }
            catch(OperationCanceledException ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false) + ex.Message;
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
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


        public async Task<string> GetAmountOfAdminsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            int adminsLeft = InMemoryDatabase.Accounts.Count();

            if(adminsLeft > 1)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.getAdminsSuccess).ConfigureAwait(false);
            }

            return await _messageBank.GetMessage(IMessageBank.Responses.lastAdminFail).ConfigureAwait(false);

            

        }

        //Can optimize all the for loops
        public async Task<string> DeleteAccountAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();


            string accountName = Thread.CurrentPrincipal.Identity.Name;
            string accountRole = Thread.CurrentPrincipal.IsInRole("admin") ? "admin" : "user";
            IAccount account = new UserAccount(accountName, accountRole);
            try
            {
                if (InMemoryDatabase.Accounts.Contains(account))
                {
                    for (int j = 0; j < InMemoryDatabase.OTPClaims.Count; j++)
                    {
                        if (InMemoryDatabase.OTPClaims[j].Username.Equals(accountName))
                        {
                            InMemoryDatabase.OTPClaims.RemoveAt(j);
                            break;
                        }
                    }
                    for (int j = 0; j < InMemoryDatabase.Nodes.Count; j++)
                    {
                        if (InMemoryDatabase.Nodes[j].UserHash.Equals(accountName))
                        {
                            if (InMemoryDatabase.NodeTags[j].NodeID.Equals(InMemoryDatabase.Nodes[j].NodeID))
                            {
                                InMemoryDatabase.NodeTags.RemoveAt(j);
                            }
                            InMemoryDatabase.Nodes.RemoveAt(j);
                        }
                    }
                    for (int j = 0; j < InMemoryDatabase.NodeRatings.Count; j++)
                    {
                        if (InMemoryDatabase.NodeRatings[j].UserHash.Equals(accountName))
                        {
                            InMemoryDatabase.NodeRatings.RemoveAt(j);
                        }
                    }
                    for (int j = 0; j < InMemoryDatabase.ConfirmationLinks.Count; j++)
                    {
                        if (InMemoryDatabase.ConfirmationLinks[j].Username.Equals(accountName))
                        {
                            InMemoryDatabase.ConfirmationLinks.RemoveAt(j);
                            break;
                        }
                    }

                    InMemoryDatabase.Accounts.Remove(account);
                    return await _messageBank.GetMessage(IMessageBank.Responses.accountDeletionSuccess).ConfigureAwait(false);

                }
                
                else
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.accountNotFound).ConfigureAwait(false);
                }

            }


            catch (AccountDeletionFailedException adfe)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.accountDeleteFail).ConfigureAwait(false);
            }

        }

        public async Task<string> DeleteAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string accountName = account.Username;

            try
            {
                if (InMemoryDatabase.Accounts.Contains(account))
                {
                    for (int j = 0; j < InMemoryDatabase.OTPClaims.Count; j++)
                    {
                        if (InMemoryDatabase.OTPClaims[j].Username.Equals(accountName))
                        {
                            InMemoryDatabase.OTPClaims.RemoveAt(j);
                            break;
                        }
                    }
                    for (int j = 0; j < InMemoryDatabase.Nodes.Count; j++)
                    {
                        if (InMemoryDatabase.Nodes[j].UserHash.Equals(accountName))
                        {
                            if (InMemoryDatabase.NodeTags[j].NodeID.Equals(InMemoryDatabase.Nodes[j].NodeID))
                            {
                                InMemoryDatabase.NodeTags.RemoveAt(j);
                            }
                            InMemoryDatabase.Nodes.RemoveAt(j);
                        }
                    }
                    for (int j = 0; j < InMemoryDatabase.Ratings.Count; j++)
                    {
                        if (InMemoryDatabase.Ratings[j].Username.Equals(accountName))
                        {
                            InMemoryDatabase.Ratings.RemoveAt(j);
                        }
                    }
                    for (int j = 0; j < InMemoryDatabase.ConfirmationLinks.Count; j++)
                    {
                        if (InMemoryDatabase.ConfirmationLinks[j].Username.Equals(accountName))
                        {
                            InMemoryDatabase.ConfirmationLinks.RemoveAt(j);
                            break;
                        }
                    }

                    InMemoryDatabase.Accounts.Remove(account);
                    return await _messageBank.GetMessage(IMessageBank.Responses.accountDeletionSuccess).ConfigureAwait(false);

                }

                else
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.accountNotFound).ConfigureAwait(false);
                }

            }


            catch (AccountDeletionFailedException adfe)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.accountDeleteFail).ConfigureAwait(false);
            }

        }


        public async Task<string> CreateOTPAsync(string username, string authorizationLevel, int failCount, CancellationToken cancellationToken = default(CancellationToken))
        {
            return "200";
        }


        public async Task<Tuple<int, string>> CreateAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if(InMemoryDatabase.Accounts.Contains(account))
                    return Tuple.Create(InMemoryDatabase.Accounts.Count-1, await _messageBank.GetMessage(IMessageBank.Responses.accountAlreadyCreated));

                InMemoryDatabase.Accounts.Add(account);

                if (cancellationToken.IsCancellationRequested)
                {
                    InMemoryDatabase.Accounts.Remove(account);
                    throw new OperationCanceledException();
                }

                return Tuple.Create(-1,await _messageBank.GetMessage(IMessageBank.Responses.generic));
            }
            catch (OperationCanceledException)
            {
                // No rollback necessary
                throw;
            }
            catch(Exception ex)
            {
                return Tuple.Create(-1,_messageBank.GetMessage(IMessageBank.Responses.accountCreateFail).Result);
            }

        }

        public async Task<Tuple<IAccount, string>> GetAccountAsync(string email, string authorizationLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            IAccount nullAccount = null;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                for (int i = 0; i < InMemoryDatabase.Accounts.Count(); i++)
                    if (email.Equals(InMemoryDatabase.Accounts[i].Username))
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
                    if (email == InMemoryDatabase.Accounts[i].Username && authorizationLevel == InMemoryDatabase.Accounts[i].AuthorizationLevel)
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
                    if (email == InMemoryDatabase.Accounts[i].Username && authorizationLevel == InMemoryDatabase.Accounts[i].AuthorizationLevel)
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
                if (!InMemoryDatabase.Accounts.Contains(new UserAccount(confirmationLink.Username, "doesnt matter", confirmationLink.AuthorizationLevel, true, false)))
                    return await _messageBank.GetMessage(IMessageBank.Responses.accountNotFound);
                if (InMemoryDatabase.ConfirmationLinks.Contains(confirmationLink))
                    return await _messageBank.GetMessage(IMessageBank.Responses.confirmationLinkExists);

                InMemoryDatabase.ConfirmationLinks.Add(confirmationLink);
                if (cancellationToken.IsCancellationRequested)
                {
                    string rollbackResult = await RemoveConfirmationLinkAsync(confirmationLink);
                    if (rollbackResult != _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                        return await _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed);
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

        /// <summary>
        ///     Adds tag to list of node(s)
        /// </summary>
        /// <param name="nodeIDs">List of node IDs</param>
        /// <param name="tagName">Tag name</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status result</returns>
        public async Task<string> AddTagAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Check if tag is null or empty
                if (tagName == null || tagName.Equals("") || tagName.Trim().Equals(""))
                    return await _messageBank.GetMessage(IMessageBank.Responses.tagNameInvalid);

                // Check if node list is null or empty
                if (nodeIDs == null || nodeIDs.Count() <= 0 )
                    return await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound);

                // Check if tag exists
                if (!InMemoryDatabase.Tags.Contains(new Tag(tagName, 0)))
                    return await _messageBank.GetMessage(IMessageBank.Responses.tagDoesNotExist);

                // Add tag to nodes
                for (int i = 0; i  < nodeIDs.Count; i++)
                {
                    INodeTag nodeTag = new NodeTag(nodeIDs[i], tagName);

                    if (!InMemoryDatabase.NodeTags.Contains(nodeTag))
                        InMemoryDatabase.NodeTags.Add(nodeTag);
                }

                return await _messageBank.GetMessage(IMessageBank.Responses.tagAddSuccess);
            }
            catch(OperationCanceledException ex)
            {
                // Rollback handled
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }        
        }

        /// <summary>
        ///     Removes a tag from list of node(s)
        /// </summary>
        /// <param name="nodeIDs">List of node IDs</param>
        /// <param name="tagName">Tag name</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status result</returns>
        public async Task<string> RemoveTagAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Check if tag name is null, empty string or all space
            if (tagName == null || tagName.Equals("") || tagName.Trim().Equals(""))
                return await _messageBank.GetMessage(IMessageBank.Responses.tagNameInvalid).ConfigureAwait(false);
            // Check if node list is null or empty
            if (nodeIDs == null || nodeIDs.Count() <= 0)
                return await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound).ConfigureAwait(false);

            for (int i = 0; i  < nodeIDs.Count; i++)
            {
                INodeTag nodeTag = new NodeTag(nodeIDs[i], tagName);
                if (!InMemoryDatabase.NodeTags.Contains(nodeTag))
                    InMemoryDatabase.NodeTags.Remove(nodeTag);
            }

            return await _messageBank.GetMessage(IMessageBank.Responses.tagRemoveSuccess);
        }

        /// <summary>
        ///     Retrieves a list of shared tags from a list of node(s) in in memory database. List with single node will return all tags.
        /// </summary>
        /// <param name="nodeIDs">List of node ID(s)</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>List of tags and string status result</returns>
        public async Task<Tuple<List<string>, string>> GetNodeTagsAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();


                List<string> tags = new List<string>();

                // Check tag count
                if (nodeIDs == null ||nodeIDs.Count() <= 0)
                    return Tuple.Create(tags, await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound));

                for (int i = 0; i < nodeIDs.Count; i++)
                {

                    List<string> results = new List<string>();
                    for (int j = 0; j < InMemoryDatabase.NodeTags.Count; j++)
                    {
                        if (nodeIDs[i].Equals(InMemoryDatabase.NodeTags[j].NodeID))
                            results.Add(InMemoryDatabase.NodeTags[j].TagName);
                    }
                    if (nodeIDs[i] == nodeIDs.First())
                    {
                        tags = results;
                    }
                    tags = tags.Intersect(results).ToList();
                }

                if (cancellationToken.IsCancellationRequested)
                    return Tuple.Create(new List<string>(), await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested));

                return Tuple.Create(tags, await _messageBank.GetMessage(IMessageBank.Responses.tagGetSuccess));
            }
            catch (OperationCanceledException)
            {
                // Rollback handled
                return Tuple.Create(new List<string>(), await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested));
            }
            catch (Exception ex)
            {
                return Tuple.Create(new List<string>(), await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message);
            }
        }

        /// <summary>
        ///     Creates a tag in in memory tag bank
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <param name="count">Number of nodes tagged</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status result</returns>
        public async Task<string> CreateTagAsync(string tagName, int count,CancellationToken cancellationToken = default(CancellationToken))
        {

            try
            {
                // Throw if cancellation is requested
                cancellationToken.ThrowIfCancellationRequested();

                //Check input
                if (tagName == null || tagName.Equals("") || tagName.Trim().Equals(""))
                    return await _messageBank.GetMessage(IMessageBank.Responses.tagNameInvalid);

                // Check tag count
                if (count < 0)
                    return await _messageBank.GetMessage(IMessageBank.Responses.tagCountInvalid);

                ITag tag = new Tag(tagName, count);

                // Check if tag already exists in tag bank
                if (InMemoryDatabase.Tags.Contains(tag))
                    return await _messageBank.GetMessage(IMessageBank.Responses.tagDuplicate);
               
                // Add Tag to In Memory Bank
                InMemoryDatabase.Tags.Add(tag);

                return await _messageBank.GetMessage(IMessageBank.Responses.tagCreateSuccess);

            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }    
        }

        /// <summary>
        ///     Retrieves list of tags from tag bank
        /// </summary>
        /// <param name="cancellationToken">Cnacellation Token</param>
        /// <returns>List of tags and string status result</returns>
        public async Task<Tuple<List<ITag>, string>> GetTagsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            List<ITag> tags = InMemoryDatabase.Tags.AsList<ITag>();
            return Tuple.Create(tags, await _messageBank.GetMessage(IMessageBank.Responses.tagGetSuccess));
        }


        /// <summary>
        ///  Deletes tag from in memory tag bank
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status code</returns>
        public async Task<string> DeleteTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                // Throw if cancellation is requested
                cancellationToken.ThrowIfCancellationRequested();
                
                // Check input
                if (tagName == null || tagName.Equals("") || tagName.Trim().Equals(""))
                    return await _messageBank.GetMessage(IMessageBank.Responses.tagNameInvalid);

                ITag tag = new Tag(tagName);

                // Remove tag from nodes
                for (int i = 0; i < InMemoryDatabase.NodeTags.Count(); i++)
                    if (InMemoryDatabase.NodeTags[i].TagName.Equals(tagName))
                        InMemoryDatabase.NodeTags.RemoveAt(i);

                // Remove tag from bank
                if (InMemoryDatabase.Tags.Contains(tag))
                    InMemoryDatabase.Tags.Remove(tag);

                return await _messageBank.GetMessage(IMessageBank.Responses.tagDeleteSuccess);

            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }        
        }

        public async Task<string> RemoveUserIdentityFromHashTable(string email, string authorizationLevel, string hashedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            return "200";
        }
        public async Task<string> CreateUserHashAsync(int ID, string hashedEmail, CancellationToken cancellationToken = default(CancellationToken))
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
                    if(node.NodeID == n.NodeID)
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
                    if (nodeID == n.NodeID)
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
                    if (c.ParentNodeID == nodeID)
                    {
                        children.Add(c);
                    }
                }
                foreach (Node n in children)
                {
                    n.ParentNodeID = parentID;
                }

                foreach (Node n in InMemoryDatabase.Nodes)
                {
                    if (nodeID == n.NodeID)
                    {
                        n.Deleted = true;
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
                foreach(Node n in InMemoryDatabase.Nodes){
                    if (nID.Equals(n.NodeID))
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

        public Task<Tuple<List<INode>, string>> GetNodesAsync(string userHash, string accountHash, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<string> RateNodeAsync(string userHash, long nodeID, int rating, CancellationToken cancellationToken = default(CancellationToken))
        {
            InMemoryDatabase.NodeRatings.Add(new NodeRating(userHash, nodeID, rating));
            return await _messageBank.GetMessage(IMessageBank.Responses.userRateSuccess);
        }

        public async Task<Tuple<List<double>, string>> GetNodeRatingAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken))
        {
            List<double> ratings = new List<double>();
            
            return Tuple.Create(ratings, await _messageBank.GetMessage(IMessageBank.Responses.getRateSuccess));
        }

        public async Task<string> UpdateAccountAsync(IAccount account, IAccount updatedAccount, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                //Throw Cancellation Exception if token requests cancellation
                cancellationToken.ThrowIfCancellationRequested();

                if (InMemoryDatabase.Accounts.Contains(account))
                {
                    int index = InMemoryDatabase.Accounts.IndexOf(account);
                    InMemoryDatabase.Accounts[index].Username = updatedAccount.Username;
                    InMemoryDatabase.Accounts[index].AuthorizationLevel = updatedAccount.AuthorizationLevel;
                    InMemoryDatabase.Accounts[index].Confirmed = updatedAccount.Confirmed;
                    InMemoryDatabase.Accounts[index].AccountStatus = updatedAccount.AccountStatus;
                }
                else
                {
                    return await _messageBank.GetMessage(IMessageBank.Responses.accountNotFound);
                }

                return await _messageBank.GetMessage(IMessageBank.Responses.accountUpdateSuccess);
            }
            catch (OperationCanceledException)
            {
                // Rollback already handled
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }
    }
}

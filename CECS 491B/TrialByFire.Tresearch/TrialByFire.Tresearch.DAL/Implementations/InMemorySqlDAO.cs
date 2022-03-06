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

        public string VerifyAccount(IAccount account)
        {
            int index = InMemoryDatabase.Accounts.IndexOf(account);
            if (index != -1)
            {
                IAccount dbAccount = InMemoryDatabase.Accounts[index];
                if((account.Passphrase != null) && account.Passphrase.Equals(dbAccount.Passphrase))
                {
                    if (dbAccount.Confirmed != false)
                    {
                        if (dbAccount.AccountStatus != false)
                        {
                            return _messageBank.SuccessMessages["generic"];
                        }
                        return _messageBank.ErrorMessages["notFoundOrEnabled"];
                    }
                    return _messageBank.ErrorMessages["notConfirmed"];
                }
                return _messageBank.ErrorMessages["badNameOrPass"];
            }
            return _messageBank.ErrorMessages["notFoundOrEnabled"];
        }

        public List<string> Authenticate(IOTPClaim otpClaim)
        {
            List<string> results = new List<string>();
            try
            {
                IAccount account = new Account(otpClaim.Username, otpClaim.AuthorizationLevel);
                // Find account in db
                int index = InMemoryDatabase.Accounts.IndexOf(account);
                if (index != -1)
                {
                    IAccount dbAccount = InMemoryDatabase.Accounts[index];
                    // check if confirmed
                    if (dbAccount.Confirmed != false)
                    {
                        // check if enabled
                        if (dbAccount.AccountStatus != false)
                        {
                            // find otp claim in db
                            index = InMemoryDatabase.OTPClaims.IndexOf(otpClaim);
                            if (index != -1)
                            {
                                IOTPClaim dbOTPClaim = InMemoryDatabase.OTPClaims[index];
                                // check if otp is same
                                if(otpClaim.OTP.Equals(dbOTPClaim.OTP))
                                {
                                    if (otpClaim.TimeCreated <= dbOTPClaim.TimeCreated.AddMinutes(2))
                                    {
                                        results.Add(_messageBank.SuccessMessages["generic"]);
                                        results.Add($"username:{dbAccount.Username},authorizationLevel:{dbAccount.AuthorizationLevel}");
                                        return results;
                                    }
                                    else
                                    {
                                        InMemoryDatabase.OTPClaims[InMemoryDatabase.OTPClaims.IndexOf(otpClaim)].FailCount++;
                                        if (InMemoryDatabase.OTPClaims[InMemoryDatabase.OTPClaims.IndexOf(otpClaim)].FailCount >= 5)
                                        {
                                            InMemoryDatabase.Accounts[InMemoryDatabase.Accounts.IndexOf(account)].AccountStatus = false;
                                            results.Add(_messageBank.ErrorMessages["tooManyFails"]);
                                            return results;
                                        }
                                        else
                                        {
                                            results.Add(_messageBank.ErrorMessages["otpExpired"]);
                                            return results;
                                        }
                                    }
                                }
                                else
                                {
                                    results.Add(_messageBank.ErrorMessages["badNameOrOTP"]);
                                    return results;
                                }    
                            }
                            else
                            {
                                results.Add(_messageBank.ErrorMessages["badNameOrOTP"]);
                                return results;
                            }
                        }
                        else
                        {
                            results.Add(_messageBank.ErrorMessages["notFoundOrEnabled"]);
                            return results;
                        }
                    }
                    else
                    {
                        results.Add(_messageBank.ErrorMessages["notConfirmed"]);
                        return results;
                    }
                }
                results.Add(_messageBank.ErrorMessages["notFoundOrEnabled"]);
                return results;
            }
            catch (AccountCreationFailedException acfe)
            {
                results.Add(acfe.Message);
                return results;
            }
            catch (OTPClaimCreationFailedException ocfe)
            {
                results.Add(ocfe.Message);
                return results;
            }
        }

        public string VerifyAuthorized(IRolePrincipal rolePrincipal, string requiredAuthLevel)
        {
            try
            {
                IAccount account = new Account(rolePrincipal.RoleIdentity.Name, rolePrincipal.RoleIdentity.AuthorizationLevel);
                // Find account in db
                int index = InMemoryDatabase.Accounts.IndexOf(account);
                if (index != -1)
                {
                    IAccount dbAccount = InMemoryDatabase.Accounts[index];
                    // check if confirmed
                    if (dbAccount.Confirmed != false)
                    {
                        // check if enabled
                        if (dbAccount.AccountStatus != false)
                        {
                            if(dbAccount.AuthorizationLevel.Equals("admin") || dbAccount.AuthorizationLevel.Equals(requiredAuthLevel))
                            {
                                return _messageBank.SuccessMessages["generic"];
                            }
                            else
                            {
                                return _messageBank.ErrorMessages["notAuthorized"];
                            }
                        }
                        else
                        {
                            return _messageBank.ErrorMessages["notFoundOrEnabled"]; 
                        }
                    }
                    else
                    {
                        return _messageBank.ErrorMessages["notConfirmed"];
                    }
                }
                return _messageBank.ErrorMessages["notFoundOrEnabled"];
            }
            catch (AccountCreationFailedException acfe)
            {
                return acfe.Message;
            }
        }

        public IOTPClaim GetOTPClaim(IOTPClaim otpClaim)
        {
            return InMemoryDatabase.OTPClaims[InMemoryDatabase.OTPClaims.IndexOf(otpClaim)];
        }

        public string StoreOTP(IOTPClaim otpClaim)
        {
            string result;
            int index = InMemoryDatabase.OTPClaims.IndexOf(otpClaim);
            if(index >= 0)
            {
                IOTPClaim dbOTPClaim = InMemoryDatabase.OTPClaims[InMemoryDatabase.OTPClaims.IndexOf(otpClaim)];
                IAccount account = new Account(dbOTPClaim.Username, dbOTPClaim.AuthorizationLevel);
                if (!(otpClaim.TimeCreated >= dbOTPClaim.TimeCreated.AddDays(1)))
                {
                    otpClaim.FailCount = dbOTPClaim.FailCount;
                }
                InMemoryDatabase.OTPClaims[index] = otpClaim;
                return _messageBank.SuccessMessages["generic"];
            }
            return _messageBank.ErrorMessages["notFoundOrEnabled"];
        }


        public string DeleteAccount(IRolePrincipal rolePrincipal)
        {
            bool accountExists = false;
            string accountName = rolePrincipal.RoleIdentity.Name;
            string accountRole = rolePrincipal.RoleIdentity.AuthorizationLevel;
            foreach(Account ac in InMemoryDatabase.Accounts)
            {
                if(ac.Username.Equals(accountName) && ac.AuthorizationLevel.Equals(accountRole))
                {
                    accountExists = true; 
                    InMemoryDatabase.Accounts.Remove(ac);   
                }
            }
            if(accountExists)
            {
                for(int i = 0; i < InMemoryDatabase.OTPClaims.Count; i++)
                {
                    if (InMemoryDatabase.OTPClaims[i].Equals(accountName))
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
                    if (InMemoryDatabase.ConfirmationLinks[i].Equals(accountName))
                    {
                        InMemoryDatabase.ConfirmationLinks.RemoveAt(i);
                        break;
                    }
                }
                return _messageBank.SuccessMessages["generic"];
            }
            else
            {
                return _messageBank.ErrorMessages["notFoundOrEnabled"];
            }

        }




        public List<string> CreateAccount(IAccount account)
        {
            List<string> results = new List<string>();
            int numberOfConfirmationsInDatabase = InMemoryDatabase.Accounts.Count();
            InMemoryDatabase.Accounts.Add(account);
            int affectedRows = InMemoryDatabase.Accounts.Count() - numberOfConfirmationsInDatabase;

            if (affectedRows == 1)
                results.Add("Success - Account added to in memomry database");
            else
                results.Add("Failed - Could not add account to in memory database");

            return results;

        }

        public IAccount GetUnconfirmedAccount(string email)
        {
            List<string> results = new List<string>();
            for (int i = 0; i < InMemoryDatabase.Accounts.Count(); i++)
                if (email.Equals(InMemoryDatabase.Accounts[i].Email))
                    return InMemoryDatabase.Accounts[i];
            return null;

        }

        public List<string> RemoveConfirmationLink(IConfirmationLink _confirmationLink)
        {
            List<string> results = new List<string>();
            int numberOfConfirmationsInDatabase = InMemoryDatabase.ConfirmationLinks.Count();
            InMemoryDatabase.ConfirmationLinks.Remove(_confirmationLink);
            int affectedRows = InMemoryDatabase.ConfirmationLinks.Count() - numberOfConfirmationsInDatabase;
            if (affectedRows == -1)
                results.Add("Success - Confirmation link removed from in memory database");
            else
                results.Add("Failed - Confirmation link could not be removed from in memory database");
            return results;
        }

        public List<string> ConfirmAccount(IAccount account)
        {
            List<string> results = new List<string>();
            int indexOfAccount = InMemoryDatabase.Accounts.IndexOf(account);
            if (indexOfAccount == -1)
                results.Add("Failed - Account not found in database");
            else
            {
                InMemoryDatabase.Accounts[indexOfAccount].Confirmed = true;
                results.Add("Success - Account confirmed in database");
            }
            return results;
        }
        public List<string> CreateConfirmationLink(IConfirmationLink _confirmationlink)
        {
            List<string> results = new List<string>();

            int numberOfConfirmationsInDatabase = InMemoryDatabase.ConfirmationLinks.Count();
            InMemoryDatabase.ConfirmationLinks.Add(_confirmationlink);
            int affectedRows = InMemoryDatabase.ConfirmationLinks.Count() - numberOfConfirmationsInDatabase;

            if (affectedRows == 1)
                results.Add("Success - Confirmation link added to in memomry database");
            else
                results.Add("Failed - Could not add confirmation link to in memory database");

            return results;
        }

        public IConfirmationLink GetConfirmationLink(string url)
        {
            string guidString = url.Substring(url.LastIndexOf('=')+1);
            Guid guid = new Guid(guidString);
            for (int i = 0; i < InMemoryDatabase.ConfirmationLinks.Count(); i++)
                if (guid.Equals(InMemoryDatabase.ConfirmationLinks[i].UniqueIdentifier))
                    return InMemoryDatabase.ConfirmationLinks[i];
            return null;
        }

        public List<IKPI> LoadKPI(DateTime now)
        {
            throw new NotImplementedException();
        }


        /*
            Ian's Methods
         */

        public string CreateNodesCreated(INodesCreated nodesCreated)
        {
            // Check whether the NodesCreated object exists already
            foreach (INodesCreated nodesCreated1 in InMemoryDatabase.NodesCreated)
            {
                if(nodesCreated1.nodeCreationDate == nodesCreated.nodeCreationDate)
                {
                    return "Nodes Created with given date already exists";
                }
            }

            InMemoryDatabase.NodesCreated.Add(nodesCreated);

            return "Nodes Created has been added to the database";

        }

        public INodesCreated GetNodesCreated(DateTime nodeCreationDate)
        {
            INodesCreated nodeResult = new NodesCreated();

            foreach (INodesCreated nodesCreated in InMemoryDatabase.NodesCreated)
            {
                if(nodeCreationDate == nodesCreated.nodeCreationDate)
                {
                    return nodesCreated;
                }
            }

            return nodeResult;
        }

        public string UpdateNodesCreated(INodesCreated nodesCreated)
        {
            for(int i = 0; i < InMemoryDatabase.NodesCreated.Count; i++)
            {
                if(InMemoryDatabase.NodesCreated[i].nodeCreationDate == nodesCreated.nodeCreationDate)
                {
                    InMemoryDatabase.NodesCreated[i] = nodesCreated;

                    return "Nodes Created Successfully Updated";
                }
            }

            return "Nodes Created does not exist to be updated";
        }



        public string CreateDailyLogin(IDailyLogin dailyLogin)
        {
            // Check whether the daily login already exists in the database
            foreach(IDailyLogin dailyLogin1 in InMemoryDatabase.DailyLogins)
            {
                if(dailyLogin1.loginDate == dailyLogin.loginDate)
                {
                    return "Daily Login already exists";
                }
            }

            InMemoryDatabase.DailyLogins.Add(dailyLogin);

            return "Daily Login Successfully Added to Database";
        }

        public IDailyLogin GetDailyLogin(DateTime loginDate)
        {
           IDailyLogin dailyLogin = new DailyLogin();

           foreach (IDailyLogin dailyLogin1 in InMemoryDatabase.DailyLogins)
            {
                if(dailyLogin1.loginDate == loginDate)
                {
                    return dailyLogin1;
                }
            }

            return dailyLogin;
        }

        public string UpdateDailyLogin(IDailyLogin dailyLogin)
        {
            for (int i = 0; i < InMemoryDatabase.DailyLogins.Count; i++)
            {
                if(InMemoryDatabase.DailyLogins[i].loginDate == dailyLogin.loginDate)
                {
                    InMemoryDatabase.DailyLogins[i] = dailyLogin;

                    return "Daily Login Successfully Updated";
                }
            }

            return "Daily Login does not exist to be updated";
        }



        public string CreateTopSearch(ITopSearch topSearch)
        {
            foreach (ITopSearch topSearch1 in InMemoryDatabase.TopSearches)
            {
                if(topSearch1.topSearchDate == topSearch.topSearchDate)
                {
                    return "Top Search already exists";
                }
            }

            InMemoryDatabase.TopSearches.Add(topSearch);

            return "Top Search Successfully Added to Database";
        }

        public ITopSearch GetTopSearch(DateTime topSearchDate)
        {
            ITopSearch topSearch1 = new TopSearch();

            foreach(ITopSearch topSearch in InMemoryDatabase.TopSearches)
            {
                if(topSearch.topSearchDate == topSearchDate)
                {
                    return topSearch;
                }
            }

            return topSearch1;
        }

        public string UpdateTopSearch(ITopSearch topSearch)
        {
            for (int i = 0; i < InMemoryDatabase.TopSearches.Count; i++)
            {
                if(topSearch.topSearchDate == InMemoryDatabase.TopSearches[i].topSearchDate)
                {
                    InMemoryDatabase.TopSearches[i] = topSearch;

                    return "Top Search has been successfully updated";
                }
            }

            return "Top Search to be updated does not exist";
        }



        public string CreateDailyRegistration(IDailyRegistration dailyRegistration)
        {
            foreach (IDailyRegistration dailyRegistration1 in InMemoryDatabase.DailyRegistrations)
            {
                if(dailyRegistration1.registrationDate == dailyRegistration.registrationDate)
                {
                    return "Daily Registration Already Exists in the Database";
                }
            }

            InMemoryDatabase.DailyRegistrations.Add(dailyRegistration);

            return "Daily Registration Successfully Added to the Database";
        }

        public IDailyRegistration GetDailyRegistration(DateTime dailyRegistrationDate)
        {
            IDailyRegistration dailyRegistration1 = new DailyRegistration();

            foreach(IDailyRegistration dailyRegistration in InMemoryDatabase.DailyRegistrations)
            {
                if(dailyRegistration.registrationDate == dailyRegistrationDate)
                {
                    return dailyRegistration;
                }
            }

            return dailyRegistration1;
        }

        public string UpdateDailyRegistration(IDailyRegistration dailyRegistration)
        {
            for(int i = 0; i < InMemoryDatabase.DailyRegistrations.Count; i++)
            {
                if(InMemoryDatabase.DailyRegistrations[i].registrationDate == dailyRegistration.registrationDate)
                {
                    InMemoryDatabase.DailyRegistrations[i] = dailyRegistration;

                    return "Daily Registration Successfully Updated";
                }
            }

            return "Daily Registration does not exist to be updated";
        }
    }
}

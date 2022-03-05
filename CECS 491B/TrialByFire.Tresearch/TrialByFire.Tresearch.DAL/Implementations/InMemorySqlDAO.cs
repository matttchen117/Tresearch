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
                        if (dbAccount.Status != false)
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
                IAccount account = new Account(otpClaim.Username);
                // Find account in db
                int index = InMemoryDatabase.Accounts.IndexOf(account);
                if (index != -1)
                {
                    IAccount dbAccount = InMemoryDatabase.Accounts[index];
                    // check if confirmed
                    if (dbAccount.Confirmed != false)
                    {
                        // check if enabled
                        if (dbAccount.Status != false)
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
                                        IRoleIdentity roleIdentity = new RoleIdentity(true, dbAccount.Username, dbAccount.AuthorizationLevel);
                                        IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
                                        InMemoryDatabase.RolePrincipals.Add(rolePrincipal);
                                        results.Add(_messageBank.SuccessMessages["generic"]);
                                        results.Add($"username:{roleIdentity.Name},role:{roleIdentity.Role}");
                                        return results;
                                    }
                                    else
                                    {
                                        InMemoryDatabase.OTPClaims[InMemoryDatabase.OTPClaims.IndexOf(otpClaim)].FailCount++;
                                        if (InMemoryDatabase.OTPClaims[InMemoryDatabase.OTPClaims.IndexOf(otpClaim)].FailCount >= 5)
                                        {
                                            InMemoryDatabase.Accounts[InMemoryDatabase.Accounts.IndexOf(account)].Status = false;
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
            catch (RoleIdentityCreationFailedException ricfe)
            {
                results.Add(ricfe.Message);
                return results;
            }
            catch (RolePrincipalCreationFailedException rpcfe)
            {
                results.Add(rpcfe.Message);
                return results;
            }
        }

        public string VerifyAuthenticated(IRolePrincipal rolePrincipal)
        {
            if(InMemoryDatabase.RolePrincipals.Contains(rolePrincipal))
            {
                return _messageBank.SuccessMessages["generic"];
            }
            return _messageBank.ErrorMessages["notAuthenticated"];
        }
        public string VerifyAuthorized(IRolePrincipal rolePrincipal, string requiredRole)
        {
            if(rolePrincipal.IsInRole("admin") || rolePrincipal.IsInRole(requiredRole))
            {
                return _messageBank.SuccessMessages["generic"];
            }
            return _messageBank.ErrorMessages["notAuthorized"];
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
                IAccount account = new Account(dbOTPClaim.Username);
                if (!(otpClaim.TimeCreated >= dbOTPClaim.TimeCreated.AddDays(1)))
                {
                    otpClaim.FailCount = dbOTPClaim.FailCount;
                }
                InMemoryDatabase.OTPClaims[index] = otpClaim;
                return _messageBank.SuccessMessages["generic"];
            }
            return _messageBank.ErrorMessages["notFoundOrEnabled"];
        }

        public bool CreateAccount(IAccount account)
        {
            throw new NotImplementedException();
        }

        public bool CreateConfirmationLink(IConfirmationLink _confirmationlink)
        {
            throw new NotImplementedException();
        }

        public IConfirmationLink GetConfirmationLink(string url)
        {
            throw new NotImplementedException();
        }

        public List<IKPI> LoadKPI(DateTime now)
        {
            throw new NotImplementedException();
        }
    }
}

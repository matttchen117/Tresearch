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

        public string VerifyAccountEnabled(IAccount account)
        {
            int index = inMemoryDatabase.Accounts.IndexOf(account);
            if(index != -1)
            {
                IAccount dbAccount = inMemoryDatabase.Accounts[index];
                if (dbAccount.Confirmed != false)
                {
                    if (dbAccount.Status != false)
                    {
                        return "success";
                    }
                    return "Database: The account was not found or it has been disabled.";
                }
                return "Database: Please click on the confirmation link that we sent to your email in " +
                    "order to confirm your account.";
            }
            return "Data: Invalid Username or Passphrase. Please try again.";
        }

        public List<string> Authenticate(IOTPClaim otpClaim)
        {
            List<string> results = new List<string>();
            IAccount account = new Account(otpClaim.Username);
            if (inMemoryDatabase.Accounts[inMemoryDatabase.Accounts.IndexOf(account)].Confirmed != false)
            {
                if (inMemoryDatabase.Accounts[inMemoryDatabase.Accounts.IndexOf(account)].Status != false)
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
                                    IRoleIdentity roleIdentity = new RoleIdentity(true, account.Username, account.AuthorizationLevel);
                                    IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
                                    inMemoryDatabase.RolePrincipals.Add(rolePrincipal);
                                    results.Add("success");
                                    results.Add($"username:{roleIdentity.Name},role{roleIdentity.Role}");
                                    return results;
                                }
                                else
                                {
                                    results.Add("Data: Invalid Username or OTP. Please try again.");
                                }
                            }
                            else
                            {
                                inMemoryDatabase.OTPClaims[inMemoryDatabase.OTPClaims.IndexOf(otpClaim)].FailCount++;
                                if (inMemoryDatabase.OTPClaims[inMemoryDatabase.OTPClaims.IndexOf(otpClaim)].FailCount >= 5)
                                {
                                    inMemoryDatabase.Accounts[inMemoryDatabase.Accounts.IndexOf(account)].Status = false;
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
                            results.Add("Data: Invalid Username or OTP. Please try again.");
                        }
                    }
                    else
                    {
                        results.Add("Database: No corresponding OTP Claim was found in the database.");
                    }
                }
                else
                {
                    results.Add("Database: The account was not found or it has been disabled.");
                }
            }
            else
            {
                results.Add("Database: Please click on the confirmation link that we sent to your email in order to " +
                    "confirm your account.");
            }
            return results;
        }

        public string VerifyAuthenticated(IRolePrincipal rolePrincipal)
        {
            if(inMemoryDatabase.RolePrincipals.Contains(rolePrincipal))
            {
                return "success";
            }
            return "Database: No active session found. Please login and try again.";
        }
        public string Authorize(IRolePrincipal rolePrincipal, string requiredRole)
        {
            string result = VerifyAuthenticated(rolePrincipal);
            if (result.Equals("success"))
            {
                if(rolePrincipal.IsInRole("admin") || rolePrincipal.IsInRole(requiredRole))
                {
                    return result;
                }
                return "Data: You are not authorized to perform this operation.";
            }
            return result;
        }

        public IOTPClaim GetOTPClaim(IOTPClaim otpClaim)
        {
            return inMemoryDatabase.OTPClaims[inMemoryDatabase.OTPClaims.IndexOf(otpClaim)];
        }

        public string StoreOTP(IOTPClaim otpClaim)
        {
            string result;
            int index = inMemoryDatabase.OTPClaims.IndexOf(otpClaim);
            if(index >= 0)
            {
                IOTPClaim dbOTPClaim = inMemoryDatabase.OTPClaims[inMemoryDatabase.OTPClaims.IndexOf(otpClaim)];
                IAccount account = new Account(dbOTPClaim.Username);
                if (!(otpClaim.TimeCreated >= dbOTPClaim.TimeCreated.AddDays(1)))
                {
                    otpClaim.FailCount = dbOTPClaim.FailCount;
                }
                inMemoryDatabase.OTPClaims[index] = otpClaim;
                return "success";
            }
            return "Database: The account was not found or it has been disabled.";
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
    }
}

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

        public IOTPClaim GetOTPClaim(IOTPClaim otpClaim)
        {
            return inMemoryDatabase.OTPClaims[inMemoryDatabase.OTPClaims.IndexOf(otpClaim)];
        }

        public string StoreOTP(IOTPClaim otpClaim)
        {
            int index = inMemoryDatabase.OTPClaims.IndexOf(otpClaim);
            if(index >= 0)
            {
                IOTPClaim dbOTPClaim = GetOTPClaim(otpClaim);
                if(!(otpClaim.TimeCreated >= dbOTPClaim.TimeCreated.AddDays(1)))
                {
                    otpClaim.FailCount = dbOTPClaim.FailCount;
                }
                inMemoryDatabase.OTPClaims[index] = otpClaim;
                return "success";
            }
            return "Database: The account does not exist.";
        }
    }
}

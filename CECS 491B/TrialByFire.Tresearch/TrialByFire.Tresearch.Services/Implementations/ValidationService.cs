using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class ValidationService : IValidationService
    {
        public ValidationService()
        {
        }
        public string ValidateInput(Dictionary<string, string> input)
        {
            string result = "";
            string username = "";
            string passphrase = "";
            string otp = "";
            string email = "";
            bool validUsername = false;
            bool validPassphrase = false;
            bool validOTP = false;
            bool validEmail = false;
            bool containsLower = false;
            bool containsUpper = false;
            bool containsNum = false;
            bool containsSpecial = false;
            char[] validSymbols = { '.', ',', '@', '!' };
            if (input.ContainsKey("username") && input.ContainsKey("passphrase"))
            {
                username = input["username"];
                passphrase = input["passphrase"];
                if (username.Length > 8 && passphrase.Length > 8)
                {
                    for (int i = 0; i < username.Length; i++)
                    {
                        if (char.IsUpper(username[i]))
                        {
                            return "Data: Invalid Username or Passphrase. Please try again.";
                        }
                        if (!char.IsLetterOrDigit(username[i]))
                        {
                            if(!validSymbols.Contains(username[i]))
                            {
                                return "Data: Invalid Username or Passphrase. Please try again.";
                            }
                        }
                    }
                    for (int j = 0; j < passphrase.Length; j++)
                    {
                        if (!char.IsLetterOrDigit(passphrase[j]))
                        {
                            if (!validSymbols.Contains(passphrase[j]))
                            {
                                return "Data: Invalid Username or Passphrase. Please try again.";
                            }
                        }
                    }
                    return "success";
                }
                return "Data: Invalid Username or Passphrase. Please try again.";
            }
            if (input.ContainsKey("username") && input.ContainsKey("otp"))
            {
                username = input["username"];
                otp = input["otp"];
                if (username.Length > 8 && passphrase.Length > 8)
                {
                    for (int i = 0; i < username.Length; i++)
                    {
                        if (char.IsUpper(username[i]))
                        {
                            return "Data: Invalid Username or Passphrase. Please try again.";
                        }
                        if (!char.IsLetterOrDigit(username[i]))
                        {
                            if (!validSymbols.Contains(username[i]))
                            {
                                return "Data: Invalid Username or Passphrase. Please try again.";
                            }
                        }
                    }
                    for (int j = 0; j < otp.Length; j++)
                    {
                        if (!char.IsLetterOrDigit(otp[j]))
                        {
                            if (!validSymbols.Contains(otp[j]))
                            {
                                return "Data: Invalid Username or Passphrase. Please try again.";
                            }
                        }
                    }
                    return "success";
                }
                return "Data: Invalid Username or OTP. Please try again.";
            }
            if(input.ContainsKey("email"))
            {
                throw new NotImplementedException();
            }
            return result;
        }
    }
}

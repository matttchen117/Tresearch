using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class ValidationService : IValidationService
    {
        private IMessageBank _messageBank { get; }
        public ValidationService(IMessageBank messageBank)
        {
            _messageBank = messageBank;
        }
        public string ValidateInput(Dictionary<string, string> input)
        {
            string result = "";
            string username = "";
            string passphrase = "";
            string otp = "";
            string email = "";
            int userSpecials = 0;
            char[] validSymbols = { '.', ',', '@', '!' };
            char[] requiredSymbols = { '.', '@' };
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
                            return _messageBank.ErrorMessages["badNameOrPass"];
                        }
                        if (!char.IsLetterOrDigit(username[i]))
                        {
                            if (!validSymbols.Contains(username[i]))
                            {
                                return _messageBank.ErrorMessages["badNameOrPass"];
                            }
                            else
                            {
                                userSpecials++;
                            }
                        }
                    }
                    if (userSpecials < 2)
                    {
                        return _messageBank.ErrorMessages["badNameOrPass"];
                    }
                    for (int j = 0; j < passphrase.Length; j++)
                    {
                        if (!char.IsLetterOrDigit(passphrase[j]))
                        {
                            if (!validSymbols.Contains(passphrase[j]))
                            {
                                return _messageBank.ErrorMessages["badNameOrPass"];
                            }
                        }
                    }
                    return _messageBank.SuccessMessages["generic"];
                }
                return _messageBank.ErrorMessages["badNameOrPass"];
            }
            if (input.ContainsKey("username") && input.ContainsKey("otp"))
            {
                username = input["username"];
                otp = input["otp"];
                if (username.Length > 8 && otp.Length > 8)
                {
                    for (int i = 0; i < username.Length; i++)
                    {
                        if (char.IsUpper(username[i]))
                        {
                            return _messageBank.ErrorMessages["badNameOrOTP"];
                        }
                        if (!char.IsLetterOrDigit(username[i]))
                        {
                            if (!validSymbols.Contains(username[i]))
                            {
                                return _messageBank.ErrorMessages["badNameOrOTP"];
                            }
                            else
                            {
                                userSpecials++;
                            }
                        }
                    }
                    if (userSpecials < 2)
                    {
                        return _messageBank.ErrorMessages["badNameOrOTP"];
                    }
                    for (int j = 0; j < otp.Length; j++)
                    {
                        if (!char.IsLetterOrDigit(otp[j]))
                        {
                            if (!validSymbols.Contains(otp[j]))
                            {
                                return _messageBank.ErrorMessages["badNameOrOTP"];
                            }
                        }
                    }
                    return _messageBank.SuccessMessages["generic"];
                }
                return _messageBank.ErrorMessages["badNameOrOTP"];
            }
            if (input.ContainsKey("email"))
            {
                throw new NotImplementedException();
            }
            return result;
        }
    }
}
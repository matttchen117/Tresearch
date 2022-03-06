using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class RegistrationManager : IRegistrationManager
    {
        public ISqlDAO _sqlDAO { get; set; }
        public ILogService _logService { get; set; }
        public IMailService _mailService { get; set; }
        public IRegistrationService _registrationService { get; set; }

        public IValidationService _validationService { get; set; }

        public IMessageBank _messageBank { get; set; }

        private string defaultAuthorization = "User";

        private int linkActivationLimit = 24;

        public RegistrationManager(ISqlDAO sqlDAO, ILogService logService, IRegistrationService accountService, IMailService mailService, IValidationService validationService, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _mailService = mailService;
            _registrationService = accountService;
            _validationService = validationService;
            _messageBank = messageBank;
        }

        public List<string> CreatePreConfirmedAccount(string email, string passphrase)
        {
            List<string> results = new List<string>();
            try
            {
                //Verify that the user has correct credentials
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("username", email);
                keyValuePairs.Add("otp", passphrase);
                string result = _validationService.ValidateInput(keyValuePairs);
                if (result.Equals(_messageBank.SuccessMessages["generic"]))
                {
                    IAccount _account = new Account(email, email, passphrase, defaultAuthorization, true, false);
                    results.AddRange(_registrationService.CreatePreConfirmedAccount(_account));
                    if (results.Last()[0] == 'F')
                    {
                        results.Add("Failed - Unable to Create Account in Account Manager");
                        return results;
                    }
                }

            }
            catch
            {
                results.Add("Failed - Unable to Create Account in Account Manager");
                return results;
            }
            results.Add("Success - Registration Manager created account");
            return results;
        }

        public List<string> ConfirmAccount(string url)
        {
            List<string> results = new List<string>();
            try
            {

                IConfirmationLink _confirmationLink = _registrationService.GetConfirmationLink(url);
                if (_confirmationLink == null)
                {
                    results.Add("Failed - Registration Manager unable to create confirmation link object");
                    return results;
                }



                IAccount account = _registrationService.GetUserFromConfirmationLink(_confirmationLink);
                if (account.Email == null)
                {
                    results.Add("Failed - Registration Manager could not get Account with tied to confirmation link");
                    return results;
                }

                results.Insert(0, account.Email);

                if (!IsConfirmationLinkValid(_confirmationLink))
                {
                    // Confirmation link is older than 24 days old
                    results.AddRange(_registrationService.RemoveConfirmationLink(_confirmationLink));
                    results.Add("Failed - Registration Manager determined confirmation link to be old");
                    return results;
                }
                else
                {
                    results.AddRange(_registrationService.ConfirmAccount(account));
                    results.AddRange(_registrationService.RemoveConfirmationLink(_confirmationLink));
                    if (results.Last()[0] == 'S')
                        results.Add("Success - Registration Manager confirmed account");
                    else
                        results.Add("Failed - Registration Manager could not confirm account");
                }
            }
            catch (Exception ex)
            {
                results.Add("Failed - Registration Manager " + ex);
            }
            return results;
        }

        public List<string> SendConfirmation(string email, string baseUrl)
        {
            List<string> results = new List<string>();
            try
            {
                results.AddRange(_registrationService.CreateConfirmation(email, baseUrl));
                if (results.Last()[0] == 'F')
                {
                    if (results.First() == "Failed - Email already has confirmation link")
                        results.Add("Failed - Account manager could not create confirmation link. confirmation link already exists");
                    else
                        results.Add("Failed - Registration Manager unable to create confirmation link");
                    return results;
                }
                results.Add(_mailService.SendConfirmation(email, results.First()));
                if (results.Last()[0] == 'F')
                {
                    results.Add("Failed - Registration Manager unable to send confirmation email");
                }
                else
                {
                    results.Add("Success - Registration Manager sent email confirmation");
                }
            }
            catch (Exception ex)
            {
                results.Add("Failed - Account Manager" + ex);
            }

            return results;
        }



        public bool IsConfirmationLinkValid(IConfirmationLink confirmationLink)
        {
            DateTime now = DateTime.Now;
            DateTime yesterday = now.AddDays(-1);
            if (confirmationLink.Datetime <= now && confirmationLink.Datetime >= yesterday)
                return true;
            else
                return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class RegistrationService : IRegistrationService
    {
        private ISqlDAO _sqlDAO { get; set; }
        private ILogService _logService { get; set; }
        private IMessageBank _messageBank { get; set; }

        private int linkActivationLimit = 24;

        public RegistrationService(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
        }


        public async Task<string> CreateAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                string createResult = await _sqlDAO.CreateAccountAsync(account, cancellationToken));
                
                if(cancellationToken.IsCancellationRequested && createResult == _messageBank.GetMessage(IMessageBank.Responses.generic).Result)
                {
                    //Perform Rollback
                }

                return createResult;
            }
            catch (OperationCanceledException)
            {
                //Nothing to rollback
                throw;
            }
            catch (Exception ex)
            {
                return "500: Server: " + ex.Message; 
            }
        }

        public List<string> CreateConfirmation(string email, string baseUrl)
        {
            Guid activationGuid;
            List<string> results = new List<string>();
            string linkUrl;
            try
            {
                activationGuid = Guid.NewGuid();
                linkUrl = $"{baseUrl}{activationGuid}";
                results.Add(linkUrl);
                IConfirmationLink _confirmationLink = new ConfirmationLink(email, activationGuid, DateTime.Now);

                if (_confirmationLink == null)
                {
                    results.Add("Failed - Account service could not create confirmation link");
                    return results;
                }


                results.AddRange(_sqlDAO.CreateConfirmationLink(_confirmationLink));


                if (results.Last()[0] == 'S')
                    results.Add("Success - Account service created confirmation link");
                else
                    results.Add("Failed - Account service could not create confirmation link");
            }
            catch (Exception ex)
            {
                results.Add("Failed - Account service" + ex);

            }
            return results;
        }

        public List<string> ConfirmAccount(IAccount account)
        {
            List<string> results = new List<string>();
            try
            {
                results = _sqlDAO.ConfirmAccount(account);
                if (results.Last()[0] == 'S')
                    results.Add("Success - Account service confirmed account");
                else
                    results.Add("Failed - Account service could not confirm account");
            }
            catch
            {
                results.Add("Failed - Account service could not confirm account");
            }
            return results;
        }

        public IConfirmationLink GetConfirmationLink(string url)
        {
            IConfirmationLink _confirmationLink = _sqlDAO.GetConfirmationLink(url);
            return _confirmationLink;
        }

        public List<string> RemoveConfirmationLink(IConfirmationLink _confirmationLink)
        {
            List<string> results = new List<string>();
            try
            {
                results.AddRange(_sqlDAO.RemoveConfirmationLink(_confirmationLink));
                if (results.Last()[0] == 'S')
                    results.Add("Success - Registration service created confirmation link");
                else
                    results.Add("Failed - Registration service could not create confirmation link");
            }
            catch (Exception ex)
            {
                results.Add("Failed - Account service " + ex);
            }
            return results;
        }

        public IAccount GetUserFromConfirmationLink(IConfirmationLink link)
        {
            IAccount account = new Account();

            try
            {

                account = _sqlDAO.GetUnconfirmedAccount(link.Username);
            }
            catch (Exception ex)
            {

            }
            return account;
        }


    }
}

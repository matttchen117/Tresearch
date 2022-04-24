using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class RateManager: IRateManager
    {
        private IAccountVerificationService _accountVerificationService { get; set; }       //Use to verify account exists, enabled and confirmed. Checks if account is authorized to make changes 
        private ISqlDAO _sqlDAO { get; set; }
        private ILogService _logService { get; set; }
        private IMessageBank _messageBank { get; set; }                                     //Used to send status codes
        private IRateService _rateService { get; set; }                                       //Performs business logic
        private BuildSettingsOptions _options { get; }                                      //Holds webapi key and connection string


        public RateManager(IAccountVerificationService accountVerificationService, ISqlDAO sqlDAO, ILogService logService, IMessageBank messagebank, IRateService rateService, IOptions<BuildSettingsOptions> options)
        {
            _accountVerificationService = accountVerificationService;
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messagebank;
            _rateService = rateService;
            _options = options.Value;
        }

        public async Task<string> RateNodeAsync(long nodeID, int rating, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                //Check if user is authenticated
                if (!Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    //Get user's role
                    string role = "";
                    if (Thread.CurrentPrincipal.IsInRole(_options.User))
                        role = _options.User;
                    else if (Thread.CurrentPrincipal.IsInRole(_options.Admin))
                        role = _options.Admin;
                    else
                        return await _messageBank.GetMessage(IMessageBank.Responses.unknownRole);

                    //UserAccount with user's username and role
                    IAccount account = new UserAccount(Thread.CurrentPrincipal.Identity.Name, role);

                    //Verify if account is enabled and confirmed
                    string resultVerifyAccount = await _accountVerificationService.VerifyAccountAsync(account, cancellationToken);

                    //Check if account is enabled and confirme, if not return error
                    if (!resultVerifyAccount.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess)))
                        return resultVerifyAccount;

                    //Check if account can rate here


                    //Rate Node
                    string result = await _rateService.RateNodeAsync(account, nodeID, rating, cancellationToken);

                    return result;
                }
                else
                    return await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }

        public async Task<Tuple<List<double>, string>> GetNodeRatingAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            { 
                cancellationToken.ThrowIfCancellationRequested();
                if (!Thread.CurrentPrincipal.Identity.Name.Equals("guest"))
                {
                    //Get user's role
                    string role = "";
                    if (Thread.CurrentPrincipal.IsInRole(_options.User))
                        role = _options.User;
                    else if (Thread.CurrentPrincipal.IsInRole(_options.Admin))
                        role = _options.Admin;
                    else
                        return Tuple.Create(new List<double>(), await _messageBank.GetMessage(IMessageBank.Responses.unknownRole));

                    //UserAccount with user's username and role
                    IAccount account = new UserAccount(Thread.CurrentPrincipal.Identity.Name, role);

                    //Verify if account is enabled and confirmed
                    string resultVerifyAccount = await _accountVerificationService.VerifyAccountAsync(account, cancellationToken);

                    //Check if account is enabled and confirme, if not return error
                    if (!resultVerifyAccount.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess)))
                        return Tuple.Create(new List<double>(), resultVerifyAccount);

                    Tuple<List<double>, string> results = await _rateService.GetNodeRatingAsync(nodeIDs, cancellationToken);

                    return results;
                }
                else
                    return Tuple.Create(new List<double>(), await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated));


            }
            catch (Exception ex)
            {
                return  Tuple.Create(new List<double>(), await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message);
            }
        }
    }
}

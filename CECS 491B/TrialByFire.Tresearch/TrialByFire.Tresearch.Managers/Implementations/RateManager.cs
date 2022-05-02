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

        public async Task<IResponse<NodeRating>> RateNodeAsync(long nodeID, int rating, CancellationToken cancellationToken = default(CancellationToken))
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
                        return new RateResponse<NodeRating>(await _messageBank.GetMessage(IMessageBank.Responses.unknownRole), new NodeRating(), 500, false);

                    string userHash = (Thread.CurrentPrincipal.Identity as IRoleIdentity).UserHash;

                    //UserAccount with user's username and role
                    IAccount account = new UserAccount(Thread.CurrentPrincipal.Identity.Name, role);

                    //Verify if account is enabled and confirmed
                    string resultVerifyAccount = await _accountVerificationService.VerifyAccountAsync(account, cancellationToken);

                    //Check if account is enabled and confirme, if not return error
                    if (!resultVerifyAccount.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess)))
                        return new RateResponse<NodeRating>(resultVerifyAccount, new NodeRating(), 500, false);

                    //Verify if account can rate here
                    string resultAuthorized = await _accountVerificationService.VerifyAccountAuthorizedNodeChangesAsync(new List<long> { nodeID } , userHash, cancellationToken);

                    //Check if allowed to rate
                    if(resultAuthorized.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess))) 
                    { 
                        return new RateResponse<NodeRating>(await _messageBank.GetMessage(IMessageBank.Responses.notAuthorized), new NodeRating(), 401, false);
                    }

                    //Rate Node
                    IResponse<NodeRating> result = await _rateService.RateNodeAsync(userHash, nodeID, rating, cancellationToken);

                    return result;
                }
                else
                    return new RateResponse<NodeRating>(await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated), new NodeRating(), 500, false);
            }
            catch (Exception ex)
            {
                return new RateResponse<NodeRating>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException), new NodeRating(), 500, false);
            }
        }

        public async Task<IResponse<double>> GetNodeRatingAsync(long nodeID, CancellationToken cancellationToken = default(CancellationToken))
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

                        return new RateResponse<double>(await _messageBank.GetMessage(IMessageBank.Responses.unknownRole), 0, 500, false);

                    //UserAccount with user's username and role
                    IAccount account = new UserAccount(Thread.CurrentPrincipal.Identity.Name, role);

                    //Verify if account is enabled and confirmed
                    string resultVerifyAccount = await _accountVerificationService.VerifyAccountAsync(account, cancellationToken);

                    //Check if account is enabled and confirme, if not return error
                    if (!resultVerifyAccount.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess)))
                        return new RateResponse<double>(resultVerifyAccount, 0, 500, false);

                    IResponse<double> results = await _rateService.GetNodeRatingAsync(nodeID, cancellationToken);

                    return results;
                }
                else
                    return new RateResponse<double>(await _messageBank.GetMessage(IMessageBank.Responses.notAuthenticated), 0, 401, false);
            }
            catch (Exception ex)
            {
                return new RateResponse<double>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException) + ex.Message, 0, 500, false);
            }
        }
    }
}

using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class TagManager : ITagManager
    {
        private IAccountVerificationService _accountVerificationService { get; set; }
        private ISqlDAO _sqlDAO { get; set; }
        private ILogService _logService { get; set; }
        private IMessageBank _messageBank { get; set; }
        private ITagService _tagService { get; set; }
        private BuildSettingsOptions _options { get; }
        public TagManager(IAccountVerificationService accountVerificationService, ISqlDAO sqlDAO, ILogService logService, IMessageBank messagebank,ITagService tagService, IOptions<BuildSettingsOptions> options)
        {
            _accountVerificationService = accountVerificationService; 
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messagebank;
            _tagService = tagService;
            _options = options.Value;
        }

        /// <summary>
        ///     AddTagToNodesAsync(nodeIDs, tagName)
        ///         Adds a tag to a list of nodes. Account is checked if valid and is authorized to make changes to nodes.
        /// </summary>
        /// <param name="nodeIDs">List of nodes' ids to add tag</param>
        /// <param name="tagName">String tag to add to nodes</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status code</returns>
        public async Task<string> AddTagToNodesAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                //Get user's role
                string role = "";
                if (Thread.CurrentPrincipal.IsInRole(_options.User))
                    role = _options.User;
                else if (Thread.CurrentPrincipal.IsInRole(_options.Admin))
                    role = _options.Admin;
                else
                    return await _messageBank.GetMessage(IMessageBank.Responses.unknownRole);

                //Account with user's username and role
                IAccount account = new Account(Thread.CurrentPrincipal.Identity.Name, role);

                //Verify if account is enabled and confirmed
                string resultVerifyAccount = await _accountVerificationService.VerifyAccountAsync(account, cancellationToken);

                //Check if account is enabled and confirme, if not return error
                if (!resultVerifyAccount.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess)))
                    return resultVerifyAccount;

                //Verify if account is authorized to make changes to Nodes
                string resultVerifyAuthorized = await _accountVerificationService.VerifyAccountAuthorizedNodeChangesAsync(nodeIDs, account, cancellationToken);

                //Check if account is authorized to make changes, if not return error
                if (!resultVerifyAuthorized.Equals(await _messageBank.GetMessage(IMessageBank.Responses.verifySuccess)))
                    return resultVerifyAuthorized;

                //Add Tag to nodes
                string result = await _tagService.AddTagToNodesAsync(nodeIDs, tagName, cancellationToken);

                //Returns status code
                return result;
            }
            catch(Exception ex)
            {
                return _options.UncaughtExceptionMessage + ex.Message;
            }
        }

        public async Task<string> RemoveTagFromNodesAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                string result = await _tagService.RemoveTagFromNodesAsync(nodeIDs, tagName, cancellationToken);
                if (cancellationToken.IsCancellationRequested && result.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic).Result))
                {
                    string rollbackResult = await _tagService.AddTagToNodesAsync(nodeIDs, tagName);
                    if (rollbackResult.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic).Result))
                        throw new OperationCanceledException();
                    else
                        return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                }
                return result;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return _options.UncaughtExceptionMessage + ex.Message;
            }
        }
    
        public async Task<Tuple<List<string>, string>> GetNodeTagsAsync(List<long> nodeIDs, string order, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                Tuple<List<string>, string> result;
                if (order == "asc")
                    result = await _tagService.GetNodeTagsAsync(nodeIDs, cancellationToken);
                else
                    result = await _tagService.GetNodeTagsDescAsync(nodeIDs, cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();
                return result;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return Tuple.Create(new List<string>(), _options.UncaughtExceptionMessage + ex.Message);
            }
        }

        public async Task<string> CreateTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                string result = await _tagService.CreateTagAsync(tagName, 0, cancellationToken);
                if(cancellationToken.IsCancellationRequested && result.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic).Result))
                {
                    string rollbackResult = await _tagService.RemoveTagAsync(tagName);
                    if (rollbackResult.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic).Result))
                        throw new OperationCanceledException();
                    else
                        return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                }
                return result;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return _options.UncaughtExceptionMessage + ex.Message;
            }
        }

        public async Task<string> RemoveTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                string result = await _tagService.RemoveTagAsync(tagName, cancellationToken);
                if (cancellationToken.IsCancellationRequested && result.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic).Result))
                {
                    string rollbackResult = await _tagService.CreateTagAsync(tagName, 0);
                    if (rollbackResult.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic).Result))
                        throw new OperationCanceledException();
                    else
                        return _messageBank.GetMessage(IMessageBank.Responses.rollbackFailed).Result;
                }
                return result;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return _options.UncaughtExceptionMessage + ex.Message;
            }
        }

        public async Task<Tuple<List<ITag>, string>> GetTagsAsync(string order, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                Tuple<List<ITag>, string> result;

                result = await _tagService.GetTagsAsync(cancellationToken);


                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();
                
                return result;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch(Exception ex)
            {
                return Tuple.Create(new List<ITag>(), _options.UncaughtExceptionMessage + ex.Message);
            }
        }

        public async Task<Tuple<List<ITag>, string>> GetPossibleTagsAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                Tuple<List<string>, string> nodeTagsResult = await _tagService.GetNodeTagsAsync(nodeIDs, cancellationToken);
                Tuple<List<ITag>, string> tagsResult = await _tagService.GetTagsAsync(cancellationToken);

                //if(!tagsResult.Item2.Equals(await _messageBank.GetMessage(IMessageBank.Responses.generic)) && !nodeTagsResult.Item2.Equals(await _messageBank.GetMessage(IMessageBank.Responses.generic)))
                    //return Tuple.Create(new List<string>(), await _messageBank.GetMessage(IMessageBank.Responses.tagRetrievalFail));
                
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();

                //Get the difference between the lists
                //List<string> possibleTags = tagsResult.Item1.Except(nodeTagsResult.Item1).ToList();

                return tagsResult;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return Tuple.Create(new List<ITag>(), _options.UncaughtExceptionMessage + ex.Message);
            }
        }
    }
}

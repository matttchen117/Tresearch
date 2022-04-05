using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Services.Implementations
{
    /// <summary>
    ///     Tag Service: Class used to perform business logic regarding a node's tags
    /// </summary>
    public class TagService: ITagService
    {
        private ISqlDAO _sqlDAO { get; set; }

        private ILogService _logService { get; set; }

        private IMessageBank _messageBank { get; set; }

        private BuildSettingsOptions _options { get; }

        /// <summary>
        ///     public RecoveryClass(sqlDAO, logService, messageBank)
        ///         Constructor for Tag service class
        /// </summary>
        /// <param name="sqlDAO"> SQL object to perform changes to database</param>
        /// <param name="logService"> Log service</param>
        /// <param name="messageBank"> Message bank containing status code enumerables</param>
        public TagService(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank, IOptions<BuildSettingsOptions> options)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
            _options = options.Value;
        }

        /// <summary>
        ///     AddTagToNodeAsync(nodeIDs, tagName)
        ///         Adds a tag to node. Node must be owned by user and 
        /// </summary>
        /// <param name="nodeIDs"></param>
        /// <param name="tagName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> AddTagToNodesAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                string role = "";
                if (Thread.CurrentPrincipal.IsInRole(_options.User))
                    role = _options.User;
                else if (Thread.CurrentPrincipal.IsInRole(_options.Admin))
                    role = _options.Admin;
                else
                    return await _messageBank.GetMessage(IMessageBank.Responses.unknownRole).ConfigureAwait(false);

                IAccount account = new Account(Thread.CurrentPrincipal.Identity.Name, role);

                Tuple<bool, string> isAuthorized = await OwnsNode(nodeIDs, account, cancellationToken);

                if (!isAuthorized.Item1)
                    return isAuthorized.Item2;

                if (nodeIDs.Count > 0)
                {
                    string result = await _sqlDAO.AddTagToNodesAsync(nodeIDs, tagName, cancellationToken);

                    if(cancellationToken.IsCancellationRequested && result.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic).Result))
                    {
                        //Perform Rollback
                        string resultRollback = await _sqlDAO.RemoveTagFromNodeAsync(nodeIDs, tagName, cancellationToken);
                        if (resultRollback.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic).Result))
                            throw new OperationCanceledException();
                        else
                            return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
                    }
                    
                    return result;
                }
                else
                    return _messageBank.GetMessage(IMessageBank.Responses.nodeTagNodeDoesNotExist).Result;
            }
            catch (OperationCanceledException)
            {
                throw;
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

                string role = "";
                if (Thread.CurrentPrincipal.IsInRole(_options.User))
                    role = _options.User;
                else if (Thread.CurrentPrincipal.IsInRole(_options.Admin))
                    role = _options.Admin;
                else
                    return await _messageBank.GetMessage(IMessageBank.Responses.unknownRole).ConfigureAwait(false);

                IAccount account = new Account(Thread.CurrentPrincipal.Identity.Name, role);

                Tuple<bool, string> isAuthorized = await OwnsNode(nodeIDs, account, cancellationToken);

                if (!isAuthorized.Item1)
                    return isAuthorized.Item2;

                if (nodeIDs.Count > 0)
                {
                    string result = await _sqlDAO.RemoveTagFromNodeAsync(nodeIDs, tagName, cancellationToken);
                    if (cancellationToken.IsCancellationRequested && result.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic).Result))
                    {
                        //Perform Rollback
                        string resultRollback = await _sqlDAO.AddTagToNodesAsync(nodeIDs, tagName, cancellationToken);
                        if (resultRollback.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic).Result))
                            throw new OperationCanceledException();
                        else
                            return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
                    }
                    return result;
                }
                else
                    return _messageBank.GetMessage(IMessageBank.Responses.nodeTagNodeDoesNotExist).Result;
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

        public async Task<Tuple<List<string>, string>> GetNodeTagsAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken))
        {
            List<string> tags = new List<string>();
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                string role = "";
                if (Thread.CurrentPrincipal.IsInRole(_options.User))
                    role = _options.User;
                else if (Thread.CurrentPrincipal.IsInRole(_options.Admin))
                    role = _options.Admin;
                else
                    return Tuple.Create(new List<string>(), await _messageBank.GetMessage(IMessageBank.Responses.unknownRole).ConfigureAwait(false));

                IAccount account = new Account(Thread.CurrentPrincipal.Identity.Name, role);

                Tuple<bool, string> isAuthorized = await OwnsNode(nodeIDs, account, cancellationToken);

                if (!isAuthorized.Item1)
                    return Tuple.Create(new List<string>(), isAuthorized.Item2);

                if (nodeIDs.Count > 0)
                {
                    Tuple<List<string>, string> result = await _sqlDAO.GetNodeTagsAsync(nodeIDs, cancellationToken);
                    if (cancellationToken.IsCancellationRequested)
                        throw new OperationCanceledException();
                    return result;
                }
                else
                    return Tuple.Create(tags, _messageBank.GetMessage(IMessageBank.Responses.nodeTagNodeDoesNotExist).Result);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return Tuple.Create(tags, _options.UncaughtExceptionMessage + ex.Message);
            }
        }

        public async Task<Tuple<List<string>, string>> GetNodeTagsDescAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken))
        {
            List<string> tags = new List<string>();
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (nodeIDs.Count > 0)
                {
                    Tuple<List<string>, string> result = await _sqlDAO.GetNodeTagsDescAsync(nodeIDs, cancellationToken);
                    if (cancellationToken.IsCancellationRequested)
                        throw new OperationCanceledException();
                    return result;
                }
                else
                    return Tuple.Create(tags, _messageBank.GetMessage(IMessageBank.Responses.nodeTagNodeDoesNotExist).Result);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return Tuple.Create(tags, _options.UncaughtExceptionMessage + ex.Message);
            }
        }

        public async Task<string> CreateTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                string result = await _sqlDAO.CreateTagAsync(tagName, cancellationToken);
                if (cancellationToken.IsCancellationRequested && result.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic).Result))
                {
                    //Perform Rollback
                    string resultRollback = await _sqlDAO.RemoveTagAsync(tagName, cancellationToken);
                    if (resultRollback.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic).Result))
                        throw new OperationCanceledException();
                    else
                        return _messageBank.GetMessage(IMessageBank.Responses.generic).Result;
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

        public async Task<Tuple<List<string>, string>> GetTagsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            List<string> tags = null;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                Tuple<List<string>, string> result = await _sqlDAO.GetTagsAsync(cancellationToken);
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
                return Tuple.Create(tags, _options.UncaughtExceptionMessage + ex.Message);
            }
        }

        public async Task<Tuple<List<string>, string>> GetTagsDescAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            List<string> tags = null;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                Tuple<List<string>, string> result = await _sqlDAO.GetTagsDescAsync(cancellationToken);
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
                return Tuple.Create(tags, _options.UncaughtExceptionMessage + ex.Message);
            }
        }

        public async Task<string> RemoveTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                string result = await _sqlDAO.RemoveTagAsync(tagName, cancellationToken);
                if (cancellationToken.IsCancellationRequested && result.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic).Result))
                {
                    //Perform Rollback
                    string resultRollback = await _sqlDAO.CreateTagAsync(tagName, cancellationToken);
                    if (resultRollback.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic).Result))
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

        public async Task<Tuple<bool, string>> OwnsNode(List<long> nodeID, IAccount account, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                Tuple<bool, string> result = await _sqlDAO.IsAuthorizedToMakeNodeChangesAsync(nodeID, account, cancellationToken);
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
                return Tuple.Create(false, _options.UncaughtExceptionMessage + ex.Message);
            }
        }
    }
}

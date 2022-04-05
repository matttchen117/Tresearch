using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class TagManager : ITagManager
    {
        private ISqlDAO _sqlDAO { get; set; }
        private ILogService _logService { get; set; }
        private IMessageBank _messageBank { get; set; }
        private ITagService _tagService { get; set; }
        private BuildSettingsOptions _options { get; }
        public TagManager(ISqlDAO sqlDAO, ILogService logService, IMessageBank messagebank,ITagService tagService, IOptions<BuildSettingsOptions> options)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messagebank;
            _tagService = tagService;
            _options = options.Value;
        }

        public async Task<string> AddTagToNodesAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                string result = await _tagService.AddTagToNodesAsync(nodeIDs, tagName, cancellationToken);
                if (cancellationToken.IsCancellationRequested && result.Equals(_messageBank.GetMessage(IMessageBank.Responses.generic).Result))
                {
                    string rollbackResult = await _tagService.RemoveTagFromNodesAsync(nodeIDs, tagName);
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
                string result = await _tagService.CreateTagAsync(tagName, cancellationToken);
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
                    string rollbackResult = await _tagService.CreateTagAsync(tagName);
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

        public async Task<Tuple<List<string>, string>> GetTagsAsync(string order, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                Tuple<List<string>, string> result;
                if (order == "asc")
                    result = await _tagService.GetTagsAsync(cancellationToken);
                else
                    result = await _tagService.GetTagsDescAsync(cancellationToken);
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
                return Tuple.Create(new List<string>(), _options.UncaughtExceptionMessage + ex.Message);
            }
        }

        public async Task<Tuple<List<string>, string>> GetPossibleTagsAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                Tuple<List<string>, string> nodeTagsResult = await _tagService.GetNodeTagsAsync(nodeIDs, cancellationToken);
                Tuple<List<string>, string> tagsResult = await _tagService.GetTagsAsync(cancellationToken);

                if(!tagsResult.Item2.Equals(await _messageBank.GetMessage(IMessageBank.Responses.generic)) && !nodeTagsResult.Item2.Equals(await _messageBank.GetMessage(IMessageBank.Responses.generic)))
                    return Tuple.Create(new List<string>(), await _messageBank.GetMessage(IMessageBank.Responses.tagRetrievalFail));
                
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();

                //Get the difference between the lists
                List<string> possibleTags = tagsResult.Item1.Except(nodeTagsResult.Item1).ToList();

                return Tuple.Create(possibleTags, await _messageBank.GetMessage(IMessageBank.Responses.generic));
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
    }
}

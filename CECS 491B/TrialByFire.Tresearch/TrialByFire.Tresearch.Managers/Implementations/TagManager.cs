using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class TagManager : ITagManager
    {
        private ISqlDAO _sqlDAO;
        private ILogService _logService;
        private IMessageBank _messageBank;
        private ITagService _tagService;
        public TagManager(ISqlDAO sqlDAO, ILogService logService, IMessageBank messagebank,ITagService tagService)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messagebank;
            _tagService = tagService;
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
                return "500: Server: " + ex.Message;
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
                return "500: Server: " + ex.Message;
            }
        }
    
        public async Task<Tuple<List<string>, string>> GetNodeTagsAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                Tuple<List<string>, string> result = await _tagService.GetNodeTagsAsync(nodeIDs, cancellationToken);
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
                return Tuple.Create(new List<string>(), "500: Server: " + ex.Message);
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
                return "500: Server: " + ex.Message;
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
                return "500: Server: " + ex.Message;
            }
        }

        public async Task<Tuple<List<string>, string>> GetTagsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                Tuple<List<string>, string> result = await _tagService.GetTagsAsync(cancellationToken);
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
                return Tuple.Create(new List<string>(), "500: Server: " + ex.Message);
            }
            
        }
    }
}

using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    /// <summary>
    ///     Tag Service: Class to perform business logic regarding a node's tags
    /// </summary>
    public class TagService: ITagService
    {
        private ISqlDAO _sqlDAO { get; set; }                               // Perform database operations
        private IMessageBank _messageBank { get; set; }                     // Retrieve status codes

        /// <summary>
        ///     Constructor for Tag service class
        /// </summary>
        /// <param name="sqlDAO"> SQL object to perform changes to database</param>
        /// <param name="messageBank"> Message bank containing status code enumerables</param>
        public TagService(ISqlDAO sqlDAO,  IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _messageBank = messageBank;
        }

        /// <summary>
        ///      Adds a tag to node. Must have at least one node.
        /// </summary>
        /// <param name="nodeIDs">List of nodes' ids to add tag</param>
        /// <param name="tagName">String tag to add to nodes</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status code</returns>
        public async Task<string> AddTagToNodesAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                //Throw exception if cancellation is requested.
                cancellationToken.ThrowIfCancellationRequested();

                // Check if tag is null or empty
                if(tagName != null || tagName.Equals(""))
                {
                    //Check if list contains more than one node
                    if (nodeIDs == null || nodeIDs.Count > 0)
                    {
                        //Add Tag to Node
                        string result = await _sqlDAO.AddTagAsync(nodeIDs, tagName, cancellationToken).ConfigureAwait(false);
                        return result;
                    }
                    else
                    {
                        // No nodes passed in.
                        return await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound).ConfigureAwait(false);
                    }
                }
                else
                {
                    // tag is null
                    return await _messageBank.GetMessage(IMessageBank.Responses.tagNameInvalid).ConfigureAwait(false);
                }
                   
            }
            catch (OperationCanceledException)
            {
                //rollback not necessary
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }

        /// <summary>
        ///     Removes tag from nodes passed in. Must have at least one node
        /// </summary>
        /// <param name="nodeIDs">List of nodes</param>
        /// <param name="tagName">String tag name</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>String status</returns>
        public async Task<string> RemoveTagFromNodesAsync(List<long> nodeIDs, string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                //Throw exception if cancellation is requested.
                cancellationToken.ThrowIfCancellationRequested();

                // Check if tag is null or empty
                if(tagName != null || tagName.Equals(""))
                {
                    //Check if list contains at least one node
                    if (nodeIDs == null || nodeIDs.Count > 0)
                    {
                        //Remove tag from node(s)
                        string result = await _sqlDAO.RemoveTagAsync(nodeIDs, tagName, cancellationToken).ConfigureAwait(false);
                        return result;
                    }
                    else
                    {
                        // list is null or no node passed in
                        return await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound).ConfigureAwait(false);
                    }
                }
                else
                {
                    // Tag is null or empty
                    return await _messageBank.GetMessage(IMessageBank.Responses.tagNameInvalid).ConfigureAwait(false);
                }
                    
            }
            catch (OperationCanceledException)
            {
                //rollback not necessary
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }

        /// <summary>
        ///     Retrieves all tags shared by node(s). If list contains single node, all tags are returned. 
        /// </summary>
        /// <param name="nodeIDs"> List of node IDs</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>List of all tags shared by node(s).</returns>
        public async Task<Tuple<List<string>, string>> GetNodeTagsAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken))
        {
            List<string> tags = new List<string>();
            try
            {
                //Throw exception if cancellation is requested.
                cancellationToken.ThrowIfCancellationRequested();

                // Check if list contains at least one node
                if (nodeIDs == null || nodeIDs.Count > 0)
                {
                    // Retrieve lsit of tags from database
                    Tuple<List<string>, string> result = await _sqlDAO.GetNodeTagsAsync(nodeIDs, cancellationToken).ConfigureAwait(false);
                    return result;
                }
                else
                {
                    // No nodes passed in
                    return Tuple.Create(tags, await _messageBank.GetMessage(IMessageBank.Responses.nodeNotFound).ConfigureAwait(false));
                }
                    
            }
            catch (OperationCanceledException)
            {
                // No rollback necessary
                return Tuple.Create(tags, await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                return Tuple.Create(tags, await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message);
            }
        }

        /// <summary>
        ///  Creates a tag in tag bank.
        /// </summary>
        /// <param name="tagName">String tag name</param>
        /// <param name="count">count of nodes tagged</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public async Task<string> CreateTagAsync(string tagName, int count, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                //Throw exception if cancellation is requested.
                cancellationToken.ThrowIfCancellationRequested();
                
                // Check if tag is null
                if(tagName != null)
                {
                    // Check if the count is negative (not possible)
                    if (count >= 0)
                    {
                        //Create tag in bank
                        string result = await _sqlDAO.CreateTagAsync(tagName, count, cancellationToken).ConfigureAwait(false);
                        return result;
                    }
                    else
                    {
                        // Invalid count (negative)
                        return await _messageBank.GetMessage(IMessageBank.Responses.tagCountInvalid).ConfigureAwait(false);
                    }
                }
                else
                {
                    // Tag name is null
                    return await _messageBank.GetMessage(IMessageBank.Responses.tagNameInvalid).ConfigureAwait(false);
                }
                
            }
            catch (OperationCanceledException)
            {
                //  No rollback necessary
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }

        /// <summary>
        ///     Retrieves list of all tags in tag bank.
        /// </summary>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>List of tags and status code</returns>
        public async Task<Tuple<List<ITag>, string>> GetTagsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {

            try
            {
                //Throw exception if cancellation is requested.
                cancellationToken.ThrowIfCancellationRequested();
                Tuple<List<ITag>, string> result = await _sqlDAO.GetTagsAsync(cancellationToken).ConfigureAwait(false);
                return result;
            }
            catch (OperationCanceledException)
            {
                return Tuple.Create(new List<ITag>(), await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                return Tuple.Create(new List<ITag>(), await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message);
            }
        }

        /// <summary>
        ///      Removes tag from tag bank
        /// </summary>
        /// <param name="tagName">Tag to remove</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>String status code</returns>
        public async Task<string> RemoveTagAsync(string tagName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                //Throw exception if cancellation is requested.
                cancellationToken.ThrowIfCancellationRequested();

                // Check if the tag name is null
                if(tagName != null)
                {
                    string result = await _sqlDAO.DeleteTagAsync(tagName, cancellationToken).ConfigureAwait(false);
                    return result;
                }
                else
                {
                    // tag name is null
                    return await _messageBank.GetMessage(IMessageBank.Responses.tagNameInvalid).ConfigureAwait(false);
                }      
            }
            catch (OperationCanceledException)
            {
                // No rollback necessary
                return await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message;
            }
        }
    }
}

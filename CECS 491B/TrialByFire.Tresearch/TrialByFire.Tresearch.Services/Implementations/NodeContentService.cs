using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    /// <summary>
    ///     NodeContentService: Class that is part of the Service abstraction layer that performs services related to UpdateNodeContent
    /// </summary>
    public class NodeContentService : INodeContentService
    {
        private ISqlDAO _sqlDAO;
        private IMessageBank _messageBank;
        /// <summary>
        ///     public NodeContentService():
        ///         Constructor for NodeContentService class
        /// </summary>
        /// <param name="sqlDAO">SQL Data Access Object to interact with the database</param>
        /// <param name="messageBank">Object that contains error and success messages</param>
        public NodeContentService(ISqlDAO sqlDAO, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _messageBank = messageBank;
        }

        /// <summary>
        ///     public UpdateNodeContentAsync():
        ///         Calls DAO object and interprets the response from it
        /// </summary>
        /// <param name="nodeContentInput">Custom input object that contains relevant information for methods related to UpdateNodeContent</param>
        /// <returns>Response that contains the result of the database operation</returns>
        public async Task<IResponse<string>> UpdateNodeContentAsync(INodeContentInput nodeContentInput)
        {
            // Check if input null
            if (nodeContentInput == null)
            {
                return new NodeContentResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.noSearchInput).ConfigureAwait(false), null, 400, false);
            }
            nodeContentInput.CancellationToken.ThrowIfCancellationRequested();
            try
            {
                var result = await _sqlDAO.UpdateNodeContentAsync(nodeContentInput).ConfigureAwait(false);
                switch (result.Data)
                {
                    // Success
                    case "1":
                        result.Data = await _messageBank.GetMessage(IMessageBank.Responses.updateNodeContentSuccess).ConfigureAwait(false);
                        return result;
                    // Rollback
                    case "2":
                        return new NodeContentResponse<string>(await _messageBank.GetMessage(
                            IMessageBank.Responses.updateNodeContentRollback).ConfigureAwait(false), null, 500, false);
                    // Some other error
                    default:
                        return new NodeContentResponse<string>(await _messageBank.GetMessage(
                            IMessageBank.Responses.unhandledException).ConfigureAwait(false), null, 500, false);
                }
            }catch (Exception ex)
            {
                return new NodeContentResponse<string>(await _messageBank.GetMessage(
                    IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 500, false);
            }
        }
    }
}

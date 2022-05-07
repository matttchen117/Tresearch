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
    public class NodeContentService : INodeContentService
    {
        private ISqlDAO _sqlDAO;
        private IMessageBank _messageBank;
        public NodeContentService(ISqlDAO sqlDAO, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _messageBank = messageBank;
        }

        public async Task<IResponse<string>> UpdateNodeContentAsync(INodeContentInput nodeContentInput)
        {
            if (nodeContentInput == null)
            {
                return new NodeContentResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.noSearchInput).ConfigureAwait(false), null, 400, false);
            }
            try
            {
                var result = await _sqlDAO.UpdateNodeContentAsync(nodeContentInput).ConfigureAwait(false);
                switch (result.Data)
                {
                    case "1":
                        result.Data = await _messageBank.GetMessage(IMessageBank.Responses.updateNodeContentSuccess).ConfigureAwait(false);
                        return result;
                    case "2":
                        return new NodeContentResponse<string>(await _messageBank.GetMessage(
                            IMessageBank.Responses.updateNodeContentRollback).ConfigureAwait(false), null, 500, false);
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

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
    public class PrivateAndPublicService : IPrivateAndPublicService
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IMessageBank _messageBank { get; }

        public PrivateAndPublicService(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
        }

        public async Task<IResponse<string>> PrivateNodeAsync(List<long> nodes, CancellationToken cancellationToken = default(CancellationToken))
        {
            if(nodes.Count != 0 || nodes != null)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    IResponse<string> response;


                    //IResponse<string> response = await _sqlDAO.PrivateNodeAsync(nodes, cancellationtoken).ConfigureAwait(false);
                    response = await _sqlDAO.PrivateNodeAsync(nodes, cancellationToken).ConfigureAwait(false);
                    //return response;

                    if(response.Equals(null) || response.IsSuccess == false)
                    {
                        return response;
                    }

                    //error check here
                    return response;
                }
                catch (OperationCanceledException)
                {
                    //return code for operationCancelled is 500
                    return new PrivateResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false), null, 500, false);

                }
                catch (Exception ex)
                {
                    //return code for unhandledException is 500
                    return new PrivateResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 500, false);

                }


            }

            return new PrivateResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.privateNodeFailure).ConfigureAwait(false), null, 400, false);
        }


        
        public async Task<IResponse<string>> PublicNodeAsync(List<long> nodes, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (nodes.Count != 0 || nodes != null)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    IResponse<string> response;


                    //IResponse<string> response = await _sqlDAO.PrivateNodeAsync(nodes, cancellationtoken).ConfigureAwait(false);
                    response = await _sqlDAO.PublicNodeAsync(nodes, cancellationToken).ConfigureAwait(false);
                    //return response;



                    //error check here
                    return response;
                }
                catch (OperationCanceledException)
                {
                    //return code for operationCancelled is 500
                    return new PublicResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).ConfigureAwait(false), null, 500, false);

                }
                catch (Exception ex)
                {
                    //return code for unhandledException is 500
                    return new PublicResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 500, false);

                }


            }

            return new PublicResponse<string>(await _messageBank.GetMessage(IMessageBank.Responses.privateNodeFailure).ConfigureAwait(false), null, 400, false);
        }
        

    }
}

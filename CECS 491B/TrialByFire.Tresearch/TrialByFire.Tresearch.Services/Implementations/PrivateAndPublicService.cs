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

                    //IResponse<string> response = await _sqlDAO.PrivateNodeAsync(nodes, cancellationtoken).ConfigureAwait(false);

                    //return response;
                }
                catch (OperationCanceledException ece)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    //return ("500: Server: " + ex.Message);
                }


            }
        }

    }
}

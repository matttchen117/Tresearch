using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class EditParentManager : IEditParentManager
    {
        private IMessageBank _messageBank;
        private IEditParentService _editParentService;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(
            TimeSpan.FromSeconds(10));

        public EditParentManager(IMessageBank messageBank, IEditParentService editParentService)
        {
            _messageBank = messageBank;
            _editParentService = editParentService;
        }

        public async Task<IResponse<string>> EditParentNodeAsync(string userhash, long nodeID, string nodeIDs, CancellationToken cancellationToken = default)
        {
            //if (userhash == (Thread.CurrentPrincipal.Identity as RoleIdentity).UserHash)
            if(userhash != "null")
            {
                try
                {
                    IResponse<string> response = await _editParentService.EditParentNodeAsync(nodeID, nodeIDs, cancellationToken);

                    if (cancellationToken.IsCancellationRequested)
                    {
                        MethodBase? m = MethodBase.GetCurrentMethod();
                        if (m != null)
                        {
                            response.ErrorMessage = await _messageBank.GetMessage(IMessageBank.Responses.operationTimeExceeded).ConfigureAwait(false) + m.Name;
                        }
                    }
                    return response;
                }
                catch (Exception ex)
                {
                    return new EditParentResponse<string>(
                        await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message, null, 400, false);
                }
            }
            else
            {
                return new EditParentResponse<string>(
                    await _messageBank.GetMessage(IMessageBank.Responses.notAuthorized).ConfigureAwait(false), null, 400, false);     
             }
        }
    }
}

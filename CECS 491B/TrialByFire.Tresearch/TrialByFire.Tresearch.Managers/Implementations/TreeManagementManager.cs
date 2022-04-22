using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Exceptions;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class TreeManagementManager : ITreeManagementManager
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private ITreeManagementService _treeManagementService { get; }
        private IMessageBank _messageBank { get; }

        public TreeManagementManager(ISqlDAO sqlDAO, ILogService logService, ITreeManagementService treeManagementService, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _treeManagementService = treeManagementService;
            _messageBank = messageBank;
        }
        public async Task<Tuple<Tree, string>> GetNodesAsync(string userHash, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<INode> nullList = null;
            Tree nullTree = null;
            List<string> roles = new List<string>() {"admin", "user", "guest"};
            try
            {
                //if (roles.Contains((Thread.CurrentPrincipal.Identity as RoleIdentity).AuthorizationLevel))
                //{
                    Tuple<Tree, string> results = await _treeManagementService.GetNodesAsync(userHash, cancellationToken);
                    return results;
                //}
                /*else
                {
                    return (Tuple.Create(nullTree, await _messageBank.GetMessage(IMessageBank.Responses.notAuthorized)));
                }*/
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch(Exception ex)
            {
                return (Tuple.Create(nullTree, ("500: Server: " + ex.Message)));
            }
        }
    }
}

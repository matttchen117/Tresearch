using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class LogoutManager : ILogoutManager
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }

        private IMessageBank _messageBank { get; }
        private IRolePrincipal _rolePrincipal { get; }

        private ILogoutService _logoutService { get; }

        public LogoutManager(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank, 
            IRolePrincipal rolePrincipal, ILogoutService logoutService)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
            _rolePrincipal = rolePrincipal;
            _logoutService = logoutService;
        }

        public string Logout()
        {
            if(!_rolePrincipal.RoleIdentity.Username.Equals("guest") && 
                !_rolePrincipal.RoleIdentity.AuthorizationLevel.Equals("guest"))
            {
                return _logoutService.Logout();
            }
            return _messageBank.ErrorMessages["notAuthenticated"];
        }
    }
}

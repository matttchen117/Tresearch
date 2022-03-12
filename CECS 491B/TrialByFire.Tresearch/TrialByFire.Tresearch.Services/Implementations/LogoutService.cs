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
    public class LogoutService : ILogoutService
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }

        private IMessageBank _messageBank { get; }
        private IRolePrincipal _rolePrincipal { get; }

        public LogoutService(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank, IRolePrincipal rolePrincipal)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
            _rolePrincipal = rolePrincipal;
        }

        public string Logout()
        {
            _rolePrincipal.RoleIdentity.Username = "guest";
            _rolePrincipal.RoleIdentity.AuthorizationLevel = "guest";
            if(_rolePrincipal.RoleIdentity.Username.Equals("guest") && 
                _rolePrincipal.RoleIdentity.AuthorizationLevel.Equals("guest"))
            {
                return _messageBank.SuccessMessages["generic"];
            }
            return _messageBank.ErrorMessages["logoutFail"];
        }
    }
}

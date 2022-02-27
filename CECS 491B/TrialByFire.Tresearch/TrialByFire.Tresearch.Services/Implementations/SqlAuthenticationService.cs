using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class SqlAuthenticationService : IAuthenticationService
    {
        public ISqlDAO _sqlDAO { get; set;  }
        public ILogService _logService { get; set; }

        public string _payload { get; set; }

        public SqlAuthenticationService(ISqlDAO _sqlDAO, ILogService _logService)
        {
            this._sqlDAO = _sqlDAO;
            this._logService = _logService;
        }


        public List<string> Authenticate(IOTPClaim _otpClaim)
        {
            _payload = _sqlDAO.Authenticate(_otpClaim);
        }

        public List<string> CreateJwtToken(string _payload)
        {
            // hash header and payload
            

            //store header, payload, and signature in jwt

            // encrypt the jwt 

            // return the result and jwt if success
        }
    }
}

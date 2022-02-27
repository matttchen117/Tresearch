using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Services.Implentations
{
    public class InMemoryAuthenticationService
    {
        private readonly ISqlDAO _sqlDAO;
        private readonly ILogService _logService;

        public string _payload { get; set; }

        public InMemoryAuthenticationService()
        {
            _sqlDAO = new InMemorySqlDAO();
            _logService = new InMemoryLogService();
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

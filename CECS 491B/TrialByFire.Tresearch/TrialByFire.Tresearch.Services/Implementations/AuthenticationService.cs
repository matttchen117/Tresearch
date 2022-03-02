using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        public ISqlDAO _sqlDAO { get; set;  }
        public ILogService _logService { get; set; }
        public string _payload { get; set; }

        public AuthenticationService(ISqlDAO _sqlDAO, ILogService _logService)
        {
            this._sqlDAO = _sqlDAO;
            this._logService = _logService;
        }


        public List<string> Authenticate(IOTPClaim _otpClaim)
        {
            _payload = _sqlDAO.Authenticate(_otpClaim);
        }

        // use microsoft built in jWT
        // use default key, randomizer, replace every 3 months
        // look into AES type 
        
        public List<string> CreateJwtToken(string _payload)
        {
            List<string> results = new List<string>();
            
            // break payload into parts
            Dictionary<string, string> claimValuePairs = new Dictionary<string, string>();
            string[] claimValue = _payload.Split(",");
            foreach(string cV in claimValue)
            {
                string[] pair = cV.Split(":");
                claimValuePairs.Add(pair[0], pair[1]);
            }

            // create identity to place into JWT
            IRoleIdentity roleIdentity = new RoleIdentity(true, claimValuePairs["Username"], claimValuePairs["Role"]);
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(roleIdentity);

            //create jwt and set values
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyValue = "default";
            var key = Encoding.ASCII.GetBytes(keyValue);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(7),
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            results.Add("success");
            results.Add(tokenHandler.WriteToken(token));
            return results;
        }
    }
}

﻿using System;
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
using System.Security.Principal;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private string _payLoad { get; }

        public AuthenticationService(ISqlDAO sqlDAO, ILogService logService)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _payLoad = "";
        }

        public List<string> Authenticate(IOTPClaim _otpClaim)
        {
            List<string> results = _sqlDAO.Authenticate(_otpClaim);
            return CreateJwtToken(results[1]);
        }

        // use microsoft built in jWT
        // use default key, randomizer, replace every 3 months
        // look into AES type 
        
        private List<string> CreateJwtToken(string _payload)
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
            IRoleIdentity roleIdentity = new RoleIdentity(true, claimValuePairs["username"], claimValuePairs["role"]);
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

        public string VerifyAuthenticated(IPrincipal rolePrincipal)
        {
            return _sqlDAO.VerifyAuthenticated(rolePrincipal);
        }

        public string VerifyNotAuthenticated(IPrincipal rolePrincipal)
        {
            return _sqlDAO.VerifyNotAuthenticated(rolePrincipal);
        }
    }
}

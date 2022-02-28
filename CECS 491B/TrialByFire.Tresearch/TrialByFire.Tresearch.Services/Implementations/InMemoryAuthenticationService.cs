using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implentations
{
    public class InMemoryAuthenticationService : IAuthenticationService
    {
        private readonly ISqlDAO _sqlDAO;
        private readonly ILogService _logService;
        private string _payload { get; set; }

        public InMemoryAuthenticationService()
        {
            // Do like this, when doing DI for tests, it will be for 
            _sqlDAO = new InMemorySqlDAO();
            _logService = new InMemoryLogService(_sqlDAO);
        }


        public List<string> Authenticate(IOTPClaim otpClaim)
        {
            _payload = _sqlDAO.Authenticate(otpClaim);
            List<string> results = CreateJwtToken(_payload);
            return results;
        }

        public List<string> CreateJwtToken(string _payload)
        {
            List<string> results = new List<string>();

            string _jwtToken;
            string header = "{\"alg\": \"HS256\",\"typ\": \"JWT\"}";
            //string _payload = "{\"Username\": \"Bob\",\"Role\": \"Admin\",\"iat\": \"now\"}";

            // hash header and payload
            string key = "";
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                //store header, payload, and signature in jwt
                // encrypt the jwt
                var signature = hmac.ComputeHash(Encoding.UTF8.GetBytes(header + '.' + _payload));
                _jwtToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(header)) + '.' +
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(_payload)) + '.' +
                    Convert.ToBase64String(signature);
            }

            // encrypt the jwt
            string encrypted;
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(_jwtToken);
                        }
                        encrypted = Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }

            // return the result and encrypted jwt if success
            results.Add("success");
            results.Add(encrypted);
            return results;
        }
    }
}

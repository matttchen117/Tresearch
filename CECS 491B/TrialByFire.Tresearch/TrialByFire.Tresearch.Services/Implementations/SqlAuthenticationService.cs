using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
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

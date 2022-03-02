using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class OTPClaim : IOTPClaim
    {
        public string _username { get; }

        public string _otp { get; }

        public DateTime _created { get; }

        public OTPClaim(string username, string otp, DateTime created)
        {
            _username = username;
            _otp = otp;
            _created = created;
        }

        public OTPClaim(IAccount account)
        {
            _username = account.username;
            _otp = GenerateRandomOTP();
            _created = DateTime.Now;
        }

        public string GenerateRandomOTP()
        {
            string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Random random = new Random();
            int length = random.Next(8, 17);
            string otp = "";
            for (int i = 0; i < length; i++)
            {
                otp += validCharacters[random.Next(0, validCharacters.Length)];
            }
            return otp;
        }
    }
}

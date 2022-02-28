using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class AccountService : IAccountService
    {
        public ISqlDAO _sqlDAO { get; set; }
        public ILogService _logService { get; set; }
        public string CreatePreRegisteredAccount(IAccount account)
        {
            try
            {

            }
            catch
            {

            }

            return "Success - Account Created";
        }

        public string CreateConfirmation(string email, string username)
        {
            try
            {

            }
            catch
            {

            }
            return "Success - Confirmation Sent";
        }

        public string ConfirmAccount(string email)
        {
            try
            {

            }
            catch
            {

            }
            return "Success - Account Confirmed";
        }

    }
}

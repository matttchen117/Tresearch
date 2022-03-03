using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class ValidationService : IValidationService
    {
        public ValidationService()
        {
        }
        public string ValidateInput(Dictionary<string, string> input)
        {
            string result = "";
            if(input.ContainsKey("username"))
            {

            }
            if(input.ContainsKey("passphrase"))
            {

            }
            if(input.ContainsKey("otp"))
            {

            }
            if(input.ContainsKey("email"))
            {

            }
            return result;
        }
    }
}

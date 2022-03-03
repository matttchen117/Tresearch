using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface IValidationService
    {
        public string ValidateInput(Dictionary<string, string> input);
    }
}

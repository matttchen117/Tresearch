using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IOutput
    {
        public string ErrorMessage { get; set; }
        public string? Data { get; set; }
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
    }
}

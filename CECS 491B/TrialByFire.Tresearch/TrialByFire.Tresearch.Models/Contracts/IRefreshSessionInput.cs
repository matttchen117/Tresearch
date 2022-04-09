using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IRefreshSessionInput : ICreateJwtInput
    {
        public string? Token { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IUserHashObject
    {
        public string? UserID { get; set; }

        public string? UserRole { get; set; }

        public string UserHash { get; set; }
    }
}

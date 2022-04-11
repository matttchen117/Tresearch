using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class UserHashObject : IUserHashObject
    {
        public string? UserID { get; set; }
        public string? UserRole { get; set; }
        public string UserHash { get; set; }

        public UserHashObject(string userID, string userRole, string userHash)
        {
            UserID = userID;
            UserRole = userRole;
            UserHash = userHash;
        }
    }
}

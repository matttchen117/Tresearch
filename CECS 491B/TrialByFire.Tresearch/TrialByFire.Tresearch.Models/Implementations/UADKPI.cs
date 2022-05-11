using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class UADKPI : IKPI
    {
        public string result { get; set; }
        List<int> dailyLogins { get; set; }
        List<int> dailyRegistrations { get; set; }
        List<int> dailyNodesCreated { get; set; }

        public UADKPI()
        {
            result = "";
            dailyLogins = new List<int>();
            dailyRegistrations = new List<int>();
            dailyNodesCreated = new List<int>();
        }

        public UADKPI(string result, List<int> dLogins, List<int> dRegistrations, List<int> dNCreated)
        {
            this.result = result;
            dailyLogins = dLogins;
            dailyRegistrations = dRegistrations;
            dailyNodesCreated = dNCreated;
        }


    }
}

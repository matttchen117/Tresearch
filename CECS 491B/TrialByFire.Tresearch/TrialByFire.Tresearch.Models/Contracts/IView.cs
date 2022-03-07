using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IView
    {
        public DateTime date { get; set; }
        public string viewName { get; set; }
        public int visits { get; set; }
        public double averageDuration { get; set; }
    }
}

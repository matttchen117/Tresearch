using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IView
    {
        public DateTime timestamp { get; }
        public string viewName { get; }
        public double averageDuration { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class View
    {
        public DateTime date { get; }
        public string viewName { get; }
        public double averageDuration { get; }

        public View(DateTime date, string viewName, int averageDuration)
        {
            this.date = date;
            this.viewName = viewName;
            this.averageDuration = averageDuration;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class View : IView
    {
        public DateTime date { get; set; }
        public string viewName { get; set; }
        public int visits { get; set; }
        public double averageDuration { get; set; }

        public View()
        {
            date = DateTime.Now;
            viewName = "";
            visits = -1;
            averageDuration = -1;
        }
        public View(DateTime date, string viewName, int visits, double averageDuration)
        {
            this.date = date;
            this.viewName = viewName;
            this.visits = visits;
            this.averageDuration = averageDuration;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Managers.Contracts
{
    public interface ILogManager
    {
        public enum Levels
        {
            Info,
            Debug,
            Warning,
            Error,
        }

        public enum Categories
        {
            View,
            Business,
            Server,
            Data,
            Datastore,
        }

        public Task<string> StoreAnalyticLogAsync(DateTime timestamp, Levels level, Categories category, 
            string description);
        public Task<string> StoreArchiveLogAsync(DateTime timestamp, Levels level, Categories category, 
            string description);
    }
}

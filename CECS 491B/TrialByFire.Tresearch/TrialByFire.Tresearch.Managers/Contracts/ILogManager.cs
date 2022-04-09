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

        public Task<string> StoreAnalyticLogAsync(DateTime timestamp, Levels level, string username,
            string authorizationLevel, Categories category, string description,
            CancellationToken cancellationToken = default);
        public Task<string> StoreArchiveLogAsync(DateTime timestamp, Levels level, string username,
            string authorizationLevel, Categories category, string description,
            CancellationToken cancellationToken = default);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface ILog
    {
        public DateTime Timestamp { get; }
        public string Level { get; }
        public enum Levels
        {
            Info,
            Debug,
            Warning,
            Error,
        }
        public string Username { get; }
        public string Category { get; }
        public enum Categories
        {
            View,
            Business,
            Server,
            Data,
            Datastore,
        }
        public string Description { get; }
        public string Hash { get; }
    }
}

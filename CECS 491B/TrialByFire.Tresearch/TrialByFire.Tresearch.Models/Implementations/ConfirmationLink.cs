using System;

namespace TrialByFire.Tresearch.Models
{
    public class ConfirmationLinks
    {
        public string username { get; set; }

        public Guid uniqueIdentifier { get; set; }

        public DateTime timestamp { get; set; }

        public ConfirmationLinks(string username, Guid uniqueIdentifier, DateTime timestamp)
        {
            this.username = username;
            this.uniqueIdentifier = uniqueIdentifier;
            this.timestamp = timestamp;
        }
    }
}
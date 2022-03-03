using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class ConfirmationLink : IConfirmationLink
    {
        public string username { get; set; }

        public Guid uniqueIdentifier { get; set; }

        public DateTime timestamp { get; set; }

        public ConfirmationLink(string username, Guid uniqueIdentifier, DateTime timestamp)
        {
            this.username = username;
            this.uniqueIdentifier = uniqueIdentifier;
            this.timestamp = timestamp;
        }
    }
}
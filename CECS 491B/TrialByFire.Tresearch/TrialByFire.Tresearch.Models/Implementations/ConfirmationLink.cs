using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class ConfirmationLink : IConfirmationLink
    {
        public string Username { get; set; }

        public Guid UniqueIdentifier { get; set; }

        public DateTime Datetime { get; set; }

        public ConfirmationLink()
        {

        }
        public ConfirmationLink(string username, Guid uniqueIdentifier, DateTime timestamp)
        {
            this.Username = username;
            this.UniqueIdentifier = uniqueIdentifier;
            this.Datetime = timestamp;
        }
    }
}
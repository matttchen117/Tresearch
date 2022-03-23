using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class RecoveryLink : IRecoveryLink
    {
        public string Username { get; set; }

        public Guid GUIDLink { get; set; }

        public DateTime TimeCreated { get; set; }

        public string AuthorizationLevel { get; set; }

        public RecoveryLink(string username, Guid uniqueIdentifier, DateTime datetime, string authorizationLevel)
        {
            Username = username;
            GUIDLink = uniqueIdentifier;
            TimeCreated = datetime;
            AuthorizationLevel = authorizationLevel;
        }

        public bool isValid()
        {
            DateTime now = System.DateTime.Now;
            return (TimeCreated > now.AddHours(-24) && TimeCreated <=now);
        }
    }
}

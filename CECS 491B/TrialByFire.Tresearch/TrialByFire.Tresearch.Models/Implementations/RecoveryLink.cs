using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class RecoveryLink : IRecoveryLink
    {
        public string Username { get; set; }

        public string AuthorizationLevel { get; set; }
        public DateTime TimeCreated { get; set; }
        public Guid GUIDLink { get; set; }

        public RecoveryLink(string username, string authorizationLevel, DateTime datetime, Guid uniqueIdentifier)
        {
            Username = username;
            GUIDLink = uniqueIdentifier;
            TimeCreated = datetime;
            AuthorizationLevel = authorizationLevel;
        }

        public bool isValid()
        {
            DateTime now = System.DateTime.Now;
            return (TimeCreated <=now.AddMinutes(1) && TimeCreated > now.AddDays(-1));
        }
    }
}

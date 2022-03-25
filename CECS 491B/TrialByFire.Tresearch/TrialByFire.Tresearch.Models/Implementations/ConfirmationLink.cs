using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class ConfirmationLink : IConfirmationLink
    {
        public string Username { get; set; }
        public string AuthorizationLevel { get; set; }
        public Guid GUIDLink { get; set; }
        public DateTime TimeCreated { get; set; }
        public ConfirmationLink(string username, string authorizationLevel, Guid uniqueIdentifier, DateTime timeCreated)
        {
            Username = username;
            AuthorizationLevel = authorizationLevel;
            GUIDLink = uniqueIdentifier;
            TimeCreated = timeCreated;
        }
    }
}
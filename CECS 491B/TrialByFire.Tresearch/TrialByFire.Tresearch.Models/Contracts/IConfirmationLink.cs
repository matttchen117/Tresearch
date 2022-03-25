using System;

namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IConfirmationLink
    {
        public string Username { get; set; }
        public string AuthorizationLevel { get; set; }
        public Guid GUIDLink { get; set; }
        public DateTime TimeCreated { get; set; }
    }
}
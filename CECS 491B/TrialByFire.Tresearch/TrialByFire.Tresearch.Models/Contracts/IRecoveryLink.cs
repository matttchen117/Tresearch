namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface IRecoveryLink
    {
        public string? Username { get; set; }

        public Guid? GUIDLink { get; set; }

        public DateTime? TimeCreated { get; set; }

        public string? AuthorizationLevel { get; set; }
        public bool isValid();
    }
}

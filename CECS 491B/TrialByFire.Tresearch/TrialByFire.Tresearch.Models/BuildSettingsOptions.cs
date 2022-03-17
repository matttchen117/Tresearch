namespace TrialByFire.Tresearch.Models
{
    public class BuildSettingsOptions
    {
        public string Environment { get; set; } = String.Empty;
        public string SqlConnectionString { get; set; } = String.Empty;
        public string SendGridAPIKey { get; set; } = String.Empty;
    }
}

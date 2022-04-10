namespace TrialByFire.Tresearch.Models.Contracts
{
    public interface ITag
    {
        /// <summary>
        ///     String holding name of tag
        /// </summary>
        string tagName { get; set; }
        /// <summary>
        /// Long holding number of nodes that contain this tag
        /// </summary>
        long tagCount { get; set; }
    }
}
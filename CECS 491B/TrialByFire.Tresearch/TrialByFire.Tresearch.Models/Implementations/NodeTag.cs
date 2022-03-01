namespace TrialByFire.Tresearch.Models
{
    public class NodeTag
    {
        public long nodeID { get; set; }
        public string tagName { get; set; }

        public Tag (long nodeID, string tagName)
        {
            this.nodeID = nodeID;
            this.tagName = tagName;
        }
    }
}
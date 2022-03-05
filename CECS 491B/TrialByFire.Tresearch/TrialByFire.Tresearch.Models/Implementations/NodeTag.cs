namespace TrialByFire.Tresearch.Models.Implementations
{
    public class NodeTag
    {
        public long nodeID { get; set; }
        public string tagName { get; set; }

        public NodeTag(long nodeID, string tagName)
        {
            this.nodeID = nodeID;
            this.tagName = tagName;
        }
    }
}
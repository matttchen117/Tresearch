namespace TrialByFire.Tresearch.Models.Implementations
{
    public class Node
    {
        public long nodeID { get; set; }

        public long parentNodeID { get; set; }

        public string nodeTitle { get; set; }

        public string summary { get; set; }

        public string mode { get; set; }

        public string accountOwner { get; set; }

        public Node(long nodeID, long parentNodeID, string nodeTitle, string summary, string mode, string accountOwner)
        {
            this.nodeID = nodeID;
            this.parentNodeID = parentNodeID;
            this.nodeTitle = nodeTitle;
            this.summary = summary;
            this.mode = mode;
            this.accountOwner = accountOwner;
        }
    }
}
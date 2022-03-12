using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
	public class NodeKPI : INodeKPI
	{
		public string result { get; set; }
		public List<INodesCreated> nodesCreated { get; set; }
		public NodeKPI()
		{
			this.result = "";
			this.nodesCreated = new List<INodesCreated>();
		}
		public NodeKPI(string result, List<INodesCreated> nodesCreated)
		{
			this.result = result;
			this.nodesCreated = nodesCreated;
		}
	}
}


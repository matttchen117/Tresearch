using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class NodeKPI : INodeKPI
	{
		public List<NodesCreated> nodesCreated { get; set; }
		public NodeKPI(List<NodesCreated> nodesCreated)
		{
			this.nodesCreated = nodesCreated;
		}
	}
}


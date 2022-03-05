using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
	public class NodeKPI : INodeKPI
	{
		List<NodesCreated> nodesCreated { get; }
		public NodeKPI(List<Node> nodesCreated)
		{
			this.nodesCreated = nodesCreated;
		}
	}
}


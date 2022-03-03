using System;
namespace TrialByFire.Tresearch.Models.Contracts
{
	public interface INodeKPI : IKPI
	{
		List<int> nodeCount { get; }
	}
}


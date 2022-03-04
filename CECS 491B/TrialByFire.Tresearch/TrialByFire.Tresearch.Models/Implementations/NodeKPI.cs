﻿using System;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Implementations
{
	public class NodeKPI : INodeKPI
	{
		List<int> nodeCount { get; }
		public NodeKPI(List<int> nodeCount)
		{
			this.nodeCount = nodeCount;
		}
	}
}


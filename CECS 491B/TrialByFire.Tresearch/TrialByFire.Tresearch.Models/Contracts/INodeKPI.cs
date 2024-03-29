﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Models.Contracts
{
	public interface INodeKPI : IKPI
	{
		string result { get; set; }
		List<INodesCreated> nodesCreated { get; set; }

	}
}


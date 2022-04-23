using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface ICopyAndPasteService
    {
        public Task<List<INode>> CopyNodeAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken));

        public Task<string> PasteNodeAsync(List<long> nodeIDs, CancellationToken cancellationToken = default(CancellationToken));
    }
}

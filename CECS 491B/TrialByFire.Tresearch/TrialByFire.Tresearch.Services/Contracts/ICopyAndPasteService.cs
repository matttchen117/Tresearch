using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface ICopyAndPasteService
    {
        //public Task<Tuple<List<INode>, string>> CopyNodeAsync(List<INode> nodesCopy, CancellationToken cancellationToken = default(CancellationToken));

        //public Task<Tuple<List<INode>, string>> CopyNodeAsync(List<long> nodesCopy, CancellationToken cancellationToken = default(CancellationToken));

        public Task<IResponse<IEnumerable<Node>>> CopyNodeAsync(List<long> nodesCopy, CancellationToken cancellationToken = default(CancellationToken));


        public Task<string> PasteNodeAsync(INode nodeToPasteTo, List<INode> nodes, CancellationToken cancellationToken = default(CancellationToken));
    }
}

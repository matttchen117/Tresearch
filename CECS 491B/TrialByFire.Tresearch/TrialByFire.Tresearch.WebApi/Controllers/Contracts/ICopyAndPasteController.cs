using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    /// <summary>
    /// Interface to access get and post requests for copy and paste feature
    /// </summary>
    public interface ICopyAndPasteController
    {

        public Task<ActionResult<IEnumerable<Node>>> CopyNodeAsync(List<long> nodesCopy);

        public Task<ActionResult<string>> PasteNodeAsync(long nodeIDToPasteTo, List<INode> nodes);

    }
}

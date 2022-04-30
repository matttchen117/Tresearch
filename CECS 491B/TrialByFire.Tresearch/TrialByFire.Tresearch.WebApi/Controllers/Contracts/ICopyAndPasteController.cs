using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface ICopyAndPasteController
    {
        //public Task<IActionResult> CopyNodeAsync(List<INode> nodesCopy);
        public Task<ActionResult<IEnumerable<Node>>> CopyNodeAsync(List<long> nodesCopy);

        //public Task<IActionResult> PasteNodeAsync(INode nodeToPasteTo, List<INode> nodes);

    }
}

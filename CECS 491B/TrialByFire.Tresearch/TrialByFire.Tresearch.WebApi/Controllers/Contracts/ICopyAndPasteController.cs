using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface ICopyAndPasteController
    {
        public Task<IActionResult> CopyNodeAsync(List<long> nodeIDs);

        public Task<IActionResult> PasteNodeAsync(INode nodeToPasteTo, List<INode> nodes);

    }
}

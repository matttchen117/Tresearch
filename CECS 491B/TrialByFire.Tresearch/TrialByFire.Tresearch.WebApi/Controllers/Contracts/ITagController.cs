using Microsoft.AspNetCore.Mvc;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface ITagController
    {
        public Task<IActionResult> GetTagsAsync();
    }
}

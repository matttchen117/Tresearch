using Microsoft.AspNetCore.Mvc;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface ITagController
    {
        public Task<IActionResult> GetTagsAsync();
        public Task<IActionResult> CreateTagAsync(string tagName);
        public Task<IActionResult> DeleteTagAsync(string tagName);
        public Task<IActionResult> AddTagToNodesAsync(List<long> nodeIDs, string tagName);
        public Task<IActionResult> RemoveTagFromNodesAsync(List<long> nodeIDs, string tagName);
        public Task<IActionResult> GetNodeTagsAsync(List<long> nodeIDs);
    }
}

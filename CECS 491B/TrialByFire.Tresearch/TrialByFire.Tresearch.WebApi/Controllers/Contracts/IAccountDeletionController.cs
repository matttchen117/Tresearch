using Microsoft.AspNetCore.Mvc;
using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface IAccountDeletionController
    {
        public Task<List<string>> DeleteAccountAsync();

    }
}

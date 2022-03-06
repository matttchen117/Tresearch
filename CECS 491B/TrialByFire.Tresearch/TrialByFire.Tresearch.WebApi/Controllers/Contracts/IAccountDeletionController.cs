using TrialByFire.Tresearch.Models.Contracts;

namespace TrialByFire.Tresearch.WebApi.Controllers.Contracts
{
    public interface IAccountDeletionController
    {
        public string DeleteAccount(IRolePrincipal rolePrincipal);

    }
}

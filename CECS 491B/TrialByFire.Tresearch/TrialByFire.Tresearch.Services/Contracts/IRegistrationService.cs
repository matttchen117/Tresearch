using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;



namespace TrialByFire.Tresearch.Services.Contracts
{
    public interface IRegistrationService
    {
        public Task<string> CreateAccountAsync(IAccount account, CancellationToken cancellation = default(CancellationToken));

        public List<string> CreateConfirmation(string email, string baseUrl);

        public List<string> ConfirmAccount(IAccount account);

        public IConfirmationLink GetConfirmationLink(string url);

        public List<string> RemoveConfirmationLink(IConfirmationLink _confirmationLink);
        public IAccount GetUserFromConfirmationLink(IConfirmationLink link);
    }
}

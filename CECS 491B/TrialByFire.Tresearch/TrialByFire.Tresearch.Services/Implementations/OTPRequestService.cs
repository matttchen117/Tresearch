using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Exceptions;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    // Summary:
    //     A service class for requesting OTPs.
    public class OTPRequestService : IOTPRequestService
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IMessageBank _messageBank { get; }
        public OTPRequestService(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
        }

        //
        // Summary:
        //     Verifies the Account and calls the DAO to store an OTPClaim for the corresponding Account
        //
        // Parameters:
        //   account:
        //     The Account to verify and store the OTPClaim for.
        //   otpClaim:
        //     The OTPClaim to store.
        //
        // Returns:
        //     The result of the verification/storing process.
        public async Task<string> RequestOTPAsync(IAccount account, IOTPClaim otpClaim, 
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                int result = await _sqlDAO.StoreOTPAsync(account, otpClaim, cancellationToken).ConfigureAwait(false);
                switch (result)
                {
                    case 0:
                        return await _messageBank.GetMessage(IMessageBank.Responses.accountNotFound).ConfigureAwait(false);
                    case 1:
                        return await _messageBank.GetMessage(IMessageBank.Responses.storeOTPSuccess).ConfigureAwait(false);
                    case 2:
                        return await _messageBank.GetMessage(IMessageBank.Responses.badNameOrPass).ConfigureAwait(false);
                    case 3:
                        return await _messageBank.GetMessage(IMessageBank.Responses.otpClaimNotFound).ConfigureAwait(false);
                    case 4:
                        return await _messageBank.GetMessage(IMessageBank.Responses.duplicateOTPClaimData).ConfigureAwait(false);
                    default:
                        throw new NotImplementedException();
                };
            }
            catch (OTPClaimCreationFailedException occfe)
            {
                return "400: Server: " + occfe.Message;
                //return occfe.Message;
            }
            catch (InvalidOperationException ioe)
            {
                return await _messageBank.GetMessage(IMessageBank.Responses.notFoundOrEnabled).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return "500: Database: " + ex.Message;
            }
        }
    }
}
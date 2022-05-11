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
    /// <summary>
    ///     OTPRequestService: Class that is part of the Service abstraction layer that performs services related to OTPRequest
    /// </summary>
    public class OTPRequestService : IOTPRequestService
    {
        private ISqlDAO _sqlDAO { get; }
        private IMessageBank _messageBank { get; }

        /// <summary>
        ///     public OTPRequestService():
        ///         Constructor for OTPRequestService class
        /// </summary>
        /// <param name="sqlDAO">SQL Data Access Object to interact with the database</param>
        /// <param name="messageBank">Object that contains error and success messages</param>
        public OTPRequestService(ISqlDAO sqlDAO, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _messageBank = messageBank;
        }

        /// <summary>
        ///     RequestOTPAsync:
        ///         Calls DAO object and interprets the response from it
        /// </summary>
        /// <param name="account">The account to associate the OTP with</param>
        /// <param name="otpClaim">The otp to store</param>
        /// <param name="cancellationToken">The cancellation token of the operation</param>
        /// <returns>The result of the operation</returns>
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
                    case 5:
                        return await _messageBank.GetMessage(IMessageBank.Responses.storeOTPRollback)
                            .ConfigureAwait(false);
                    default:
                        throw new NotImplementedException();
                };
            }
            catch (OTPClaimCreationFailedException occfe)
            {
                return "400: Server: " + occfe.Message;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TrialByFire.Tresearch.Models.Implementations;
using System.Security.Principal;
using TrialByFire.Tresearch.Exceptions;
using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.Models;

namespace TrialByFire.Tresearch.Services.Implementations
{
    /// <summary>
    ///     AuthenticationService: Class that is part of the Service abstraction layer that performs services related to Authentication
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private ISqlDAO _sqlDAO { get; }
        private BuildSettingsOptions _options { get; }
        private IMessageBank _messageBank { get; }
        private string _payLoad { get; }

        public AuthenticationService(ISqlDAO sqlDAO, 
            IOptionsSnapshot<BuildSettingsOptions> options, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _options = options.Value;
            _messageBank = messageBank;
            _payLoad = "";
        }


        /// <summary>
        ///     public AuthenticateAsync():
        ///         Calls DAO object and interprets the response from it for Authentication
        /// </summary>
        /// <param name="authenticationInput">Custom input object that contains relevant information for methods related to Authentication</param>
        /// <param name="cancellationToken">The cancellation token of the operation</param>
        /// <returns>The results of the operation</returns>
        public async Task<List<string>> AuthenticateAsync(IAuthenticationInput authenticationInput, 
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<string> results = new List<string>();
            try
            {
                authenticationInput.UserHash = await _sqlDAO.GetUserHashAsync(authenticationInput.UserAccount)
                    .ConfigureAwait(false);

                authenticationInput.UserAccount.Token = await CreateJwtTokenAsync(authenticationInput)
                    .ConfigureAwait(false);

                int result = await _sqlDAO.AuthenticateAsync(authenticationInput, cancellationToken)
                    .ConfigureAwait(false);

                switch (result)
                {
                    case 0:
                        results.Add(await _messageBank.GetMessage(IMessageBank.Responses.otpClaimNotFound).ConfigureAwait(false));
                        break;
                    case 1:
                        results.Add(await _messageBank.GetMessage(IMessageBank.Responses.authenticationSuccess).ConfigureAwait(false));
                        results.Add(authenticationInput.UserAccount.Token);
                        break;
                    case 2:
                        results.Add(await _messageBank.GetMessage(IMessageBank.Responses.otpExpired)
                            .ConfigureAwait(false));
                        break;
                    case 3:
                        results.Add(await _messageBank.GetMessage(IMessageBank.Responses.badNameOrOTP)
                            .ConfigureAwait(false));
                        break;
                    case 4:
                        results.Add(await _messageBank.GetMessage(IMessageBank.Responses.tooManyFails)
                            .ConfigureAwait(false));
                        break;
                    case 5:
                        results.Add(await _messageBank.GetMessage(IMessageBank.Responses.duplicateOTPClaimData)
                            .ConfigureAwait(false));
                        break;
                    case 6:
                        results.Add(await _messageBank.GetMessage(IMessageBank.Responses.duplicateAccountData)
                            .ConfigureAwait(false));
                        break;
                    case 7:
                        results.Add(await _messageBank.GetMessage(IMessageBank.Responses.authenticationRollback)
                            .ConfigureAwait(false));
                        break;
                    default:
                        throw new NotImplementedException();
                };
                return results;
            }
            catch (OTPClaimCreationFailedException occfe)
            {
                results.Add(("400: Server: " + occfe.Message));
                return results;
            }
            catch (InvalidOperationException ioe)
            {
                results.Add(await _messageBank.GetMessage(IMessageBank.Responses.notFoundOrEnabled).ConfigureAwait(false));
                return results;
            }
            catch (Exception ex)
            {
                results.Add("500: Database: " + ex.Message);
                return results;
            }
        }

        /// <summary>
        ///     CreateJwtTokenAsync:
        ///         Creates a JWT token based on the input
        /// </summary>
        /// <param name="authenticationInput">Custom input object that contains relevant information for methods related to Authentication</param>
        /// <param name="cancellationToken">The cancellation token of the operation</param>
        /// <returns>The created JWT</returns>
        private async Task<string> CreateJwtTokenAsync(IAuthenticationInput authenticationInput, CancellationToken cancellationToken 
            = default)
        {
            try
            {
                // break payload into parts
                Dictionary<string, string> claimValuePairs = new Dictionary<string, string>();
                claimValuePairs.Add(_options.RoleIdentityIdentifier1, authenticationInput.UserAccount.Username);
                claimValuePairs.Add(_options.RoleIdentityIdentifier2, authenticationInput.UserAccount.AuthorizationLevel);
                claimValuePairs.Add(_options.RoleIdentityIdentifier3, authenticationInput.UserHash);
                // create identity to place into JWT
                IRoleIdentity roleIdentity = new RoleIdentity(true, claimValuePairs[_options.RoleIdentityIdentifier1], 
                    claimValuePairs[_options.RoleIdentityIdentifier2], claimValuePairs[_options.RoleIdentityIdentifier3]);
                //create jwt and set values
                var tokenHandler = new JwtSecurityTokenHandler();
                var keyValue = _options.JWTTokenKey;
                var key = Encoding.UTF8.GetBytes(keyValue);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { 
                        new Claim(_options.RoleIdentityIdentifier1, 
                            claimValuePairs[_options.RoleIdentityIdentifier1]), 
                        new Claim(_options.RoleIdentityIdentifier2, 
                            claimValuePairs[_options.RoleIdentityIdentifier2]),
                        new Claim(_options.RoleIdentityIdentifier3,
                            claimValuePairs[_options.RoleIdentityIdentifier3])}),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    IssuedAt = DateTime.UtcNow,
                    Issuer = _options.JwtIssuer,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (RoleIdentityCreationFailedException ricf)
            {
                return ricf.Message;
            }
            catch (ArgumentNullException ane)
            {
                return "401: Server: " + ane.Message;
            }
        }

        /// <summary>
        ///     RefreshSessionAsync:
        ///         Creates a new JWT for the user
        /// </summary>
        /// <param name="authenticationInput">Custom input object that contains relevant information for methods related to Authentication</param>
        /// <param name="cancellationToken">The cancellation token of the operation</param>
        /// <returns>The results of the operation</returns>
        public async Task<List<string>> RefreshSessionAsync(IAuthenticationInput authenticationInput, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<string> results = new List<string>();
            try
            {
                string jwt = await CreateJwtTokenAsync(authenticationInput, cancellationToken).ConfigureAwait(false);
                results.Add(await _messageBank.GetMessage(IMessageBank.Responses.refreshSessionSuccess).ConfigureAwait(false));
                results.Add(jwt);
            }
            catch(Exception ex)
            {
                results.Add(await _messageBank.GetMessage(IMessageBank.Responses.unhandledException).ConfigureAwait(false) + ex.Message);
            }
            return results;
        }
    }
}
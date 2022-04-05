﻿using System;
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
    // Summary:
    //     A service class for Authenticating the User.
    public class AuthenticationService : IAuthenticationService
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private BuildSettingsOptions _options { get; }
        private IMessageBank _messageBank { get; }
        private string _payLoad { get; }

        public AuthenticationService(ISqlDAO sqlDAO, ILogService logService, 
            IOptionsSnapshot<BuildSettingsOptions> options, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _options = options.Value;
            _messageBank = messageBank;
            _payLoad = "";
        }


        //
        // Summary:
        //     Authenticates the User and returns a JWT token for them on success
        //
        // Parameters:
        //   otpClaim:
        //     The OTPClaim representing the credentials of the User attempting to Authenticate.
        //
        // Returns:
        //     The result of the Authentication process and a JWT token on success.
        public async Task<List<string>> AuthenticateAsync(IOTPClaim otpClaim, 
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<string> results = new List<string>();
            try
            {
                string jwtToken = await CreateJwtToken(otpClaim).ConfigureAwait(false);
                int result = await _sqlDAO.AuthenticateAsync(otpClaim, jwtToken, cancellationToken).ConfigureAwait(false);
                switch (result)
                {
                    case 0:
                        results.Add(await _messageBank.GetMessage(IMessageBank.Responses.otpClaimNotFound).ConfigureAwait(false));
                        break;
                    case 1:
                        results.Add(await _messageBank.GetMessage(IMessageBank.Responses.authenticationSuccess).ConfigureAwait(false));
                        results.Add(jwtToken);
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

        // use microsoft built in jWT
        // use default key, randomizer, replace every 3 months
        // look into AES type 

        //
        // Summary:
        //     Creates a JWT token based
        //
        // Parameters:
        //   payload:
        //     The payload to be put into the JWT
        //
        // Returns:
        //     The result of the JWT creation process and the JWT token on success.
        private async Task<string> CreateJwtToken(IOTPClaim otpClaim, CancellationToken cancellation 
            = default)
        {
            // break payload into parts
            Dictionary<string, string> claimValuePairs = new Dictionary<string, string>();
            claimValuePairs.Add(_options.RoleIdentityIdentifier1, otpClaim.Username);
            claimValuePairs.Add(_options.RoleIdentityIdentifier2, otpClaim.AuthorizationLevel);

            // create identity to place into JWT
            try
            {
                IRoleIdentity roleIdentity = new RoleIdentity(true, claimValuePairs[_options.RoleIdentityIdentifier1], 
                    claimValuePairs[_options.RoleIdentityIdentifier2]);
                //create jwt and set values
                var tokenHandler = new JwtSecurityTokenHandler();
                var keyValue = _options.JWTTokenKey;
                var key = Encoding.UTF8.GetBytes(keyValue);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim(_options.RoleIdentityIdentifier1, 
                    claimValuePairs[_options.RoleIdentityIdentifier1]), 
                        new Claim(_options.RoleIdentityIdentifier2, 
                        claimValuePairs[_options.RoleIdentityIdentifier2]) }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    IssuedAt = DateTime.UtcNow,
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
    }
}
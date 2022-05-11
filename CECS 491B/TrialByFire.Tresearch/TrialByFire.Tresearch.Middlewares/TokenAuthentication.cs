using Microsoft.AspNetCore.Http;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Managers.Contracts;

namespace TrialByFire.Tresearch.Middlewares
{
    /// <summary>
    ///     TokenAuthentication:
    ///         Custom middleware class for JWT token authentication
    /// </summary>
    public class TokenAuthentication
    {
        private RequestDelegate _next { get; }
        private IOptionsMonitor<BuildSettingsOptions> _options { get; }
        /// <summary>
        ///     public TokenAuthentication():
        ///         Constructor for TokenAuthentication class
        /// </summary>
        /// <param name="next">The next delagate in the process</param>
        /// <param name="options">Snapshot object that represents the setings/configurations of the application</param>
        public TokenAuthentication(RequestDelegate next, IOptionsMonitor<BuildSettingsOptions> options)
        {
            _next = next;
            _options = options;
        }

        /// <summary>
        ///     Invoke:
        ///         Operation for TokenAuthentication that checks incoming JWT, validates it, and refreshes it
        /// </summary>
        /// <param name="httpContext">The incoming HTTP request context</param>
        /// <param name="logManager">Manager object for Manager abstraction layer to handle business rules related to Logging</param>
        /// <param name="messageBank">Object that contains error and success messages</param>
        /// <param name="authenticationManager">Manager object for Manager abstraction layer to handle business rules related to Authentication</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext, ILogManager logManager, 
            IMessageBank messageBank, IAuthenticationManager authenticationManager)
        {
            try
            {
                if (httpContext.Request.Headers.ContainsKey(_options.CurrentValue.JWTHeaderName))
                {
                    if (!httpContext.Request.Headers[_options.CurrentValue.JWTHeaderName].Equals("null"))
                    {
                        // Validate JWT
                        string jwt = httpContext.Request.Headers[_options.CurrentValue.JWTHeaderName];
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var keyValue = _options.CurrentValue.JWTTokenKey;
                        var key = Encoding.UTF8.GetBytes(keyValue);
                        tokenHandler.ValidateToken(jwt, new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = true,
                            ValidIssuer = _options.CurrentValue.JwtIssuer,
                            ValidateAudience = false,
                            ValidAlgorithms = new[] { _options.CurrentValue.JwtHashAlgorithm },
                            ClockSkew = TimeSpan.Zero
                        }, out SecurityToken validatedToken);
                        var jwtToken = (JwtSecurityToken)validatedToken;
                        
                        // Set Principal 
                        IRoleIdentity roleIdentity = new RoleIdentity(true,
                            jwtToken.Claims.First(x => x.Type ==
                                _options.CurrentValue.RoleIdentityIdentifier1).Value,
                            jwtToken.Claims.First(x => x.Type ==
                                _options.CurrentValue.RoleIdentityIdentifier2).Value,
                            jwtToken.Claims.First(x => x.Type ==
                                _options.CurrentValue.RoleIdentityIdentifier3).Value);
                        IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
                        Thread.CurrentPrincipal = rolePrincipal;


                        // call authentication manager for refresh
                        List<string> results = await authenticationManager.RefreshSessionAsync().ConfigureAwait(false);
                        string[] split;
                        string result = results[0];
                        // Refresh only on success
                        if (result.Equals(await messageBank.GetMessage(IMessageBank.Responses.refreshSessionSuccess).
                                ConfigureAwait(false)))
                        {
                            httpContext.Response.Headers.Add(_options.CurrentValue.AccessControlHeaderName, _options.CurrentValue.JWTHeaderName);
                            httpContext.Response.Headers.Add(_options.CurrentValue.JWTHeaderName, results[1]);
                            split = result.Split(": ");
                            await logManager.StoreAnalyticLogAsync(DateTime.Now.ToUniversalTime(),
                                level: ILogManager.Levels.Info, category: ILogManager.Categories.Server, split[2]);
                        }
                        else
                        {
                            split = result.Split(": ");
                            if (Enum.TryParse(split[1], out ILogManager.Categories category))
                            {
                                await logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(),
                                    level: ILogManager.Levels.Error, category, split[2]);
                            }
                            else
                            {
                                await logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(),
                                    level: ILogManager.Levels.Error, category: ILogManager.Categories.Server,
                                    split[2] + ": Bad category passed back.");
                            }
                        }
                    }
                    else
                    {
                        IRoleIdentity roleIdentity = new RoleIdentity(true, _options.CurrentValue.GuestName,
                            _options.CurrentValue.GuestRole, _options.CurrentValue.GuestHash);
                        IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
                        Thread.CurrentPrincipal = rolePrincipal;
                    }
                }
                else
                {
                    IRoleIdentity roleIdentity = new RoleIdentity(true, _options.CurrentValue.GuestName,
                        _options.CurrentValue.GuestRole, _options.CurrentValue.GuestHash);
                    IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
                    Thread.CurrentPrincipal = rolePrincipal;
                }
            }
            catch (Exception ex)
            {
                IRoleIdentity roleIdentity = new RoleIdentity(true, "", "", _options.CurrentValue.GuestHash);
                IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
                Thread.CurrentPrincipal = rolePrincipal;
                await logManager.StoreArchiveLogAsync(DateTime.UtcNow, level: ILogManager.Levels.Error, 
                    category: ILogManager.Categories.Server, 
                    await messageBank.GetMessage(IMessageBank.Responses.jwtValidationFail)
                    .ConfigureAwait(false) + ex.Message);
            }finally
            {
                await _next(httpContext);
            }
        }
    }
}

//a - object for all cross cutting concerns
//b - set current thread to be the principal Thread.CurrentPrincipal, do this way
// need to set user (ClaimPrincipal here too, put data from RolePrincipal into
// new ClaimPrincipal object)
// httpContext.User = ClaimPrincipal(RolePrincipal)

// Visual Studio Magazine
// Can check archives for blog on how to do a

// Otherwise look at UseAuthentication module/source code
// Refer to see what need to do to create correct dependencies

//IServiceProvider serviceProvider = 
//httpContext.RequestServices.CreateScope(); // gives scope and access
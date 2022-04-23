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
    public class TokenAuthentication
    {
        private RequestDelegate _next { get; }
        private IOptionsMonitor<BuildSettingsOptions> _options { get; }
        public TokenAuthentication(RequestDelegate next, IOptionsMonitor<BuildSettingsOptions> options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext httpContext, ILogManager logManager, 
            IMessageBank messageBank, IAuthenticationManager authenticationManager)
        {
            try
            {
                // This should check if httpContext.User is not null
                // The Thread.CurrentPrincipal could be running on a different thread from the logout

            // This is not working, is always not null, but has no values

            // Authorization is usually used default header - gets transferred all the time
            // ALL hardcoding should be in config
            // for custom headers, follow format X-{HeaderName}

                if (httpContext.Request.Headers.ContainsKey(_options.CurrentValue.JWTHeaderName))
                {
                    if (!httpContext.Request.Headers[_options.CurrentValue.JWTHeaderName].Equals("null"))
                    {
                        // if can modify permissions, always need to check db
                        // for access, would need to verify db
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
                        //singleton
                        IRoleIdentity roleIdentity = new RoleIdentity(true,
                            jwtToken.Claims.First(x => x.Type ==
                                _options.CurrentValue.RoleIdentityIdentifier1).Value,
                            jwtToken.Claims.First(x => x.Type ==
                                _options.CurrentValue.RoleIdentityIdentifier2).Value,
                            jwtToken.Claims.First(x => x.Type ==
                                _options.CurrentValue.RoleIdentityIdentifier3).Value);
                        IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);

                        // possibly issue with this?
                        //httpContext.User = new ClaimsPrincipal(rolePrincipal);
                        Thread.CurrentPrincipal = rolePrincipal;


                        // call authentcation manager for refresh

                        List<string> results = await authenticationManager.RefreshSessionAsync().ConfigureAwait(false);
                        string[] split;
                        string result = results[0];
                        if (result.Equals(await messageBank.GetMessage(IMessageBank.Responses.refreshSessionSuccess).
                                ConfigureAwait(false)))
                        {
                            httpContext.Response.Headers.Add(_options.CurrentValue.AccessControlHeaderName, _options.CurrentValue.JWTHeaderName);
                            httpContext.Response.Headers.Add(_options.CurrentValue.JWTHeaderName, results[1]);
                            split = result.Split(": ");
                            logManager.StoreAnalyticLogAsync(DateTime.Now.ToUniversalTime(),
                                level: ILogManager.Levels.Info, category: ILogManager.Categories.Server, split[2]);
                        }
                        else
                        {
                            split = result.Split(": ");
                            if (Enum.TryParse(split[1], out ILogManager.Categories category))
                            {
                                logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(),
                                    level: ILogManager.Levels.Error, category, split[2]);
                            }
                            else
                            {
                                logManager.StoreArchiveLogAsync(DateTime.Now.ToUniversalTime(),
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
                IRoleIdentity roleIdentity = new RoleIdentity(true, "","", _options.CurrentValue.GuestHash);
                IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);
                Thread.CurrentPrincipal = rolePrincipal;
                logManager.StoreArchiveLogAsync(DateTime.UtcNow, level: ILogManager.Levels.Error, 
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
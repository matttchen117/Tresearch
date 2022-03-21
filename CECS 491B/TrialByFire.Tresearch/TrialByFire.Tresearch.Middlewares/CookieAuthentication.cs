using Microsoft.AspNetCore.Http;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace TrialByFire.Tresearch.Middlewares
{
    public class CookieAuthentication
    {
        private RequestDelegate _next { get; }
        public CookieAuthentication(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            /*try
            {*/

                // This should check if httpContext.User is not null
                // The Thread.CurrentPrincipal could be running on a different thread from the logout

                // This is not working, is always not null, but has no values
                if (httpContext.User != null)
                {
                    if(httpContext.Request.Cookies["TresearchAuthenticationCookie"] != null)
                    {
                        string jwt = httpContext.Request.Cookies["TresearchAuthenticationCookie"];
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var keyValue = "akxhBSian218c9pJA98912n4010409AMKLUHqjn2njwaj";
                        var key = Encoding.UTF8.GetBytes(keyValue);
                        tokenHandler.ValidateToken(jwt, new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ClockSkew = TimeSpan.Zero
                        }, out SecurityToken validatedToken);
                        var jwtToken = (JwtSecurityToken)validatedToken;
                        //singleton
                        IRoleIdentity roleIdentity = new RoleIdentity(true, jwtToken.Claims.First(x => x.Type == "username").Value,
                             jwtToken.Claims.First(x => x.Type == "authorizationLevel").Value);
                        IRolePrincipal rolePrincipal = new RolePrincipal(roleIdentity);

                        // possibly issue with this?
                        httpContext.User = new ClaimsPrincipal(rolePrincipal);
                        Thread.CurrentPrincipal = rolePrincipal;

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

                    }
                }
            /*}
            catch (Exception ex)
            {
                await _next(httpContext);
            }*/

            await _next(httpContext);
        }
    }
}
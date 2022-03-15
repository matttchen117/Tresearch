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

        public async Task InvokeAsync(HttpContext httpContext, IRolePrincipal rolePrincipal)
        {
            if(httpContext.Request.Cookies["TresearchAuthenticationCookie"] != null)
            {
                try
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
                    rolePrincipal.RoleIdentity = new RoleIdentity(true, jwtToken.Claims.First(x => x.Type == "username").Value,
                         jwtToken.Claims.First(x => x.Type == "authorizationLevel").Value);
                    //IServiceProvider serviceProvider = 
                    //httpContext.RequestServices = new ServiceProvider();
                }catch(Exception ex)
                {

                }
            }

            await _next(httpContext);
        }
    }
}
<<<<<<< HEAD
﻿using TrialByFire.Tresearch.Models.Contracts;
=======
﻿using Microsoft.AspNetCore.Http;
using TrialByFire.Tresearch.Models.Contracts;
>>>>>>> TestPammyMerge
using TrialByFire.Tresearch.Models.Implementations;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
<<<<<<< HEAD
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
=======
>>>>>>> TestPammyMerge

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
<<<<<<< HEAD
            if (httpContext.Request.Cookies.ContainsKey("AuthN"))
=======
            if(httpContext.Request.Cookies.ContainsKey("AuthN"))
>>>>>>> TestPammyMerge
            {
                string jwt = httpContext.Request.Cookies["AuthN"];
                var tokenHandler = new JwtSecurityTokenHandler();
                var keyValue = "akxhBSian218c9pJA98912n4010409AMKLUHqjn2njwaj";
                var key = Encoding.ASCII.GetBytes(keyValue);
                tokenHandler.ValidateToken(jwt, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
<<<<<<< HEAD
                    ValidAlgorithms = new[] { "hmacsha256 " },
=======
                    ValidAlgorithms = new[] { "hmacsha256 "},
>>>>>>> TestPammyMerge
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                rolePrincipal.RoleIdentity = new RoleIdentity(true, jwtToken.Claims.First(x => x.Type == "username").Value,
                     jwtToken.Claims.First(x => x.Type == "authorizationLevel").Value);
            }

            await _next(httpContext);
        }
    }
}
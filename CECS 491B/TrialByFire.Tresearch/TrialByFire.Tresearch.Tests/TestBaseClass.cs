using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Microsoft.AspNetCore.Builder;

namespace TrialByFire.Tresearch.Tests
{
    public class TestBaseClass
    {
        public IServiceCollection TestServices;
        public WebApplicationBuilder TestBuilder;
        public WebApplication TestApp;

        public TestBaseClass(string[] args)
        {
            TestBuilder = WebApplication.CreateBuilder();

            // Add Configuration File to DI
            TestBuilder.Services.Configure<BuildSettingsOptions>(
                TestBuilder.Configuration.GetSection(nameof(BuildSettingsOptions)));
            // Add services to the container.
            TestBuilder.Services.AddScoped<IMessageBank, MessageBank>();
            TestBuilder.Services.AddScoped<ISqlDAO, SqlDAO>();
            // Service
            TestBuilder.Services.AddScoped<IValidationService, ValidationService>();
            TestBuilder.Services.AddScoped<ILogService, LogService>();
            TestBuilder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            TestBuilder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
            TestServices = TestBuilder.Services;
        }
    }
}



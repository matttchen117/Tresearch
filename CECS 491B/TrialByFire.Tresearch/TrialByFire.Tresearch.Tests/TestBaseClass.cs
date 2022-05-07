using Microsoft.Extensions.DependencyInjection;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;

namespace TrialByFire.Tresearch.Tests
{
    public class TestBaseClass
    {
        public IServiceCollection TestServices;
        public IServiceProvider TestProvider;

        public TestBaseClass(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            TestServices = new ServiceCollection();
            TestServices.Configure<BuildSettingsOptions>(
                config.GetSection(nameof(BuildSettingsOptions)));
            // Add services to the container.
            TestServices.AddScoped<IMessageBank, MessageBank>();
            TestServices.AddScoped<ISqlDAO, SqlDAO>();
            // Service
            TestServices.AddScoped<IValidationService, ValidationService>();
            TestServices.AddScoped<ILogService, LogService>();
            TestServices.AddScoped<ILogManager, LogManager>();
            TestServices.AddScoped<IAuthenticationService, AuthenticationService>();
            TestServices.AddScoped<IAuthorizationService, AuthorizationService>();
            TestServices.AddScoped<IAccountVerificationService, AccountVerificationService>();
        }


    }
}

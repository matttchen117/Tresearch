using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();

builder.Services.AddTransient<IMessageBank, MessageBank>();
builder.Services.AddTransient<ISqlDAO, SqlDAO>();
builder.Services.AddTransient<ILogService, SqlLogService>();
builder.Services.AddTransient<IAccountDeletionService, AccountDeletionService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<IAuthorizationService, AuthorizationService>();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddTransient<IOTPRequestService, OTPRequestService>();
builder.Services.AddTransient<IRegistrationService, RegistrationService>();
builder.Services.AddTransient<IUADService, UADService>();
builder.Services.AddTransient<IValidationService, ValidationService>();
builder.Services.AddTransient<IAccountDeletionManager, AccountDeletionManager>();
builder.Services.AddTransient<IAuthenticationManager, AuthenticationManager>();
builder.Services.AddTransient<IOTPRequestManager, OTPRequestManager>();
builder.Services.AddTransient<IRegistrationManager, RegistrationManager>();
builder.Services.AddTransient<IRoleIdentity>(service => new RoleIdentity(true, "guest", "guest"));
builder.Services.AddTransient<IRolePrincipal, RolePrincipal>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCookieAuthentication();

app.UseAuthorization();

app.Run();

public static class AuthExtensions
{
    // Refer UseRouting, just passing Host
    public static IApplicationBuilder UseCookieAuthentication(this IApplicationBuilder host)
    {
        throw new NotImplementedException();
    }
}
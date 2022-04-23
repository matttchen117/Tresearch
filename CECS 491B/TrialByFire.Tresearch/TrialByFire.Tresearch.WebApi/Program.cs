using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
using TrialByFire.Tresearch.Middlewares;
using TrialByFire.Tresearch.Models;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add Configuration File to DI
builder.Services.Configure<BuildSettingsOptions>(
    builder.Configuration.GetSection(nameof(BuildSettingsOptions)));
// Add services to the container.
builder.Services.AddScoped<IMessageBank, MessageBank>();
builder.Services.AddScoped<ISqlDAO, SqlDAO>();
// Service
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IAccountVerificationService, AccountVerificationService>();
builder.Services.AddScoped<IAccountDeletionService, AccountDeletionService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IOTPRequestService, OTPRequestService>();
builder.Services.AddScoped<IRecoveryService, RecoveryService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IUADService, UADService>();
builder.Services.AddScoped<IValidationService, ValidationService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ITreeManagementService, TreeManagementService>();
builder.Services.AddScoped<IRateService, RateService>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();

// Managers
builder.Services.AddScoped<ILogManager, LogManager>();
builder.Services.AddScoped<IAccountDeletionManager, AccountDeletionManager>();
builder.Services.AddScoped<IAuthenticationManager, AuthenticationManager>();
builder.Services.AddScoped<IOTPRequestManager, OTPRequestManager>();
builder.Services.AddScoped<IRecoveryManager, RecoveryManager>();
builder.Services.AddScoped<IRegistrationManager, RegistrationManager>();
builder.Services.AddScoped<ILogoutManager, LogoutManager>();
builder.Services.AddScoped<ILogManager, LogManager>();
builder.Services.AddScoped<ITagManager, TagManager>();
builder.Services.AddScoped<IUADManager, UADManager>();  
builder.Services.AddScoped<ITreeManagementManager, TreeManagementManager>();
builder.Services.AddScoped<IUADManager, UADManager>();
builder.Services.AddScoped<IRateManager, RateManager>();
builder.Services.AddScoped<IUserManagementManager, UserManagementManager>(); 

// Unnecessary, only here temporarily for successful build

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Invoked during build, not waht we want, want it to be invoked for AuthN process
// Need to DI inject into Middleware - look into source code for how to do
// builder.Services.AddScoped<IRolePrincipal, RolePrincipal>((services) => {new RolePrincipal()});


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("https://trialbyfiretresearch.azurewebsites.net",
                                                "http://localhost:3000",
                                                "https://localhost:3000")
                                              //.WithHeaders("TresearchAuthenticationCookie")
                                              .AllowAnyHeader()
                                              .AllowAnyMethod()
                                              .AllowCredentials();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHsts();

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseTokenAuthentication();

/*app.Use((context, next) =>
{

});*/

app.MapControllers();

app.Run();

public static class AuthExtensions
{
    public static IApplicationBuilder UseTokenAuthentication(this IApplicationBuilder host)
    {
        return host.UseMiddleware<TokenAuthentication>();
    }

}

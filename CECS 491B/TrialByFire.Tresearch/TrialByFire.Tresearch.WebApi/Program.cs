using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.DAL.Implementations;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Managers.Implementations;
<<<<<<< HEAD
using TrialByFire.Tresearch.Middlewares;
using TrialByFire.Tresearch.Models;
=======
//using TrialByFire.Tresearch.Middlewares;
>>>>>>> Working
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;
using TrialByFire.Tresearch.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add Configuration File to DI
builder.Services.Configure<BuildSettingsOptions>(
    builder.Configuration.GetSection(nameof(BuildSettingsOptions)));
// Add services to the container.
<<<<<<< HEAD
builder.Services.AddScoped<IMessageBank, MessageBank>();
builder.Services.AddScoped<ISqlDAO, SqlDAO>();
// Service
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IAccountVerificationService, AccountVerificationService>();
builder.Services.AddScoped<IAccountDeletionService, AccountDeletionService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<ICreateNodeService, CreateNodeService>();
builder.Services.AddScoped<IDeleteNodeService, DeleteNodeService>();
builder.Services.AddScoped<IEditParentService, EditParentService>();
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
builder.Services.AddScoped<ICopyAndPasteService, CopyAndPasteService>();
builder.Services.AddScoped<IPrivateAndPublicService, PrivateAndPublicService>();


builder.Services.AddScoped<INodeSearchService, NodeSearchService>();
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
builder.Services.AddScoped<ICreateNodeManager, CreateNodeManager>();
builder.Services.AddScoped<IDeleteNodeManager, DeleteNodeManager>();
builder.Services.AddScoped<IEditParentManager, EditParentManager>();
builder.Services.AddScoped<ITreeManagementManager, TreeManagementManager>();
builder.Services.AddScoped<IUADManager, UADManager>();
builder.Services.AddScoped<IRateManager, RateManager>();
builder.Services.AddScoped<IUserManagementManager, UserManagementManager>();
builder.Services.AddScoped<ICopyAndPasteManager, CopyAndPasteManager>();
builder.Services.AddScoped<IPrivateAndPublicManager, PrivateAndPublicManager>();



builder.Services.AddScoped<IUADManager, UADManager>();
builder.Services.AddScoped<INodeSearchManager, NodeSearchManager>();
// Unnecessary, only here temporarily for successful build

=======
builder.Services.AddTransient<IMessageBank, MessageBank>();
builder.Services.AddTransient<ISqlDAO, SqlDAO>();
builder.Services.AddTransient<ILogService, SqlLogService>();
//builder.Services.AddTransient<IAccountDeletionService, AccountDeletionService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<IAuthorizationService, AuthorizationService>();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddTransient<IOTPRequestService, OTPRequestService>();
builder.Services.AddTransient<IRegistrationService, RegistrationService>();
builder.Services.AddTransient<IUADService, UADService>();
builder.Services.AddTransient<IValidationService, ValidationService>();
//builder.Services.AddTransient<IAccountDeletionManager, AccountDeletionManager>();
builder.Services.AddTransient<IAuthenticationManager, AuthenticationManager>();
builder.Services.AddTransient<IOTPRequestManager, OTPRequestManager>();
builder.Services.AddTransient<IRegistrationManager, RegistrationManager>();
builder.Services.AddScoped<IRolePrincipal, RolePrincipal>();
>>>>>>> Working
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

<<<<<<< HEAD


// Invoked during build, not waht we want, want it to be invoked for AuthN process
// Need to DI inject into Middleware - look into source code for how to do
// builder.Services.AddScoped<IRolePrincipal, RolePrincipal>((services) => {new RolePrincipal()});


=======
>>>>>>> Working
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
<<<<<<< HEAD
                          builder.WithOrigins("https://trialbyfiretresearch.azurewebsites.net",
                                                "http://localhost:3000",
                                                "https://localhost:3000")
                                              //.WithHeaders("TresearchAuthenticationCookie")
                                              .AllowAnyHeader()
                                              .AllowAnyMethod()
                                              .AllowCredentials();
=======
                          builder.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
>>>>>>> Working
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

<<<<<<< HEAD
//app.UseHsts();

=======
>>>>>>> Working
app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

<<<<<<< HEAD
app.UseTokenAuthentication();

/*app.Use((context, next) =>
{

});*/
=======
app.UseAuthorization();
>>>>>>> Working

app.MapControllers();

app.Run();

public static class AuthExtensions
{
<<<<<<< HEAD
    public static IApplicationBuilder UseTokenAuthentication(this IApplicationBuilder host)
    {
        return host.UseMiddleware<TokenAuthentication>();
=======
    public static IApplicationBuilder UseCookieAuthentication(this IApplicationBuilder host)
    {
        using(var serviceScope = host.ApplicationServices.CreateScope())
        {
            var services = serviceScope.ServiceProvider;
            var rolePrincipalService = services.GetRequiredService<IRolePrincipal>();
            rolePrincipalService.RoleIdentity = new RoleIdentity(true, "a", "b");
        }
        throw new NotImplementedException();
        //return host.UseMiddleware<CookieAuthentication>();
>>>>>>> Working
    }

}

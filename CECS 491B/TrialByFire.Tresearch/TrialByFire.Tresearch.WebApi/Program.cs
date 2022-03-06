var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthorization();

app.MapControllers();

app.Run();

public static class AuthExtensions
{
    // Refer UseRouting, just passing Host
    public static IApplicationBuilder UseCookieAuthentication(this IApplicationBuilder host)
    {
        SHA256 sHA256 = new SHA256Managed();
        Aes aes = new AesManaged();

        return host;
    }

}
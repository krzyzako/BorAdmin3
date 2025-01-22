
using System.Text;
using Bormech.Client.Liblary.Services.Contracts;
using Bormech.Client;
using Bormech.Client.Liblary.Services.Implementations;
using Bormech.Mqtt;
using Bormech.Server.Liblary.Data;
using Bormech.Server.Liblary.Helpers;
using Bormech.Server.Liblary.Reporitories.Contracts;
using Bormech.Server.Liblary.Reporitories.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MudBlazor.Services;
using Bormech.Plc;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<JwtSection>(builder.Configuration.GetSection("JwtSection"));
var jwtSection = builder.Configuration.GetSection("JwtSection").Get<JwtSection>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSection!.Issuer,
        ValidAudience = jwtSection!.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection!.Key!))
    };
});


builder.Services.AddScoped<IUserAccount, UserAccountRepository>();
builder.Services.AddSwaggerGen();
// Add services to the container.
builder.Services.AddMudServices();
builder.Services.AddMyjkaServices(builder.Configuration);
builder.Services.AddMqttServices(builder.Configuration);
builder.Services.AddRazorComponents();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});
var app = builder.Build();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
});
app.UseBlazorFrameworkFiles();
// app.MapRazorComponents<App>();
    // .AddInteractiveWebAssemblyRenderMode();
    // .AddAdditionalAssemblies(typeof(Bormech.Client._Imports).Assembly);
// // Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI();
}

Console.WriteLine("vr2");
// app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseStatusCodePages();
app.MapHub<PlcHub>("/plc");
app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();
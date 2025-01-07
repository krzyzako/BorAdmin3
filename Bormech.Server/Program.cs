
using Bormech.Client.Liblary.Services.Contracts;
using Bormech.Client;
using Bormech.Client.Liblary.Services.Implementations;
using Bormech.Server.Liblary.Data;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSwaggerGen();
// Add services to the container.
builder.Services.AddMudServices();

builder.Services.AddRazorComponents();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
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

app.UseHttpsRedirection();


app.UseAuthorization();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapControllers();
// app.MapBlazorHub();
app.MapFallbackToFile("index.html");
app.Run();
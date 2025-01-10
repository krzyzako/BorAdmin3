using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bormech.Plc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMyjkaServices(this IServiceCollection services, IConfiguration configuration)
        {   
            var logger = services.BuildServiceProvider().GetRequiredService<ILogger<PlcService>>();
            // Dodaj tutaj Twoje usługi Myjka
            Console.WriteLine("AddMyjkaServices");
            services.AddSingleton<OutGoPlc>();
            services.AddSingleton<PlcService>(provider => new PlcService(  provider.GetRequiredService<OutGoPlc>(),logger, provider.GetRequiredService<IHubContext<PlcHub>>())); //myjak charnowo
            return services;
        }
    }
}
using checkPIVABatch.DTOs;
using checkPIVABatch.Interfaces;
using checkPIVABatch.Models;
using checkPIVABatch.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace checkPIVABatch
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Application coniguration
            HostApplicationBuilder hostBuilder = Host.CreateApplicationBuilder(args);

            hostBuilder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            hostBuilder.Logging.AddConsole();

            // Add services to the container
            hostBuilder.Services.AddScoped<IRepositoryService, RepositoryService>();
            hostBuilder.Services.AddScoped<IVatService, VatService>();
            hostBuilder.Services.AddHttpClient();

            // Add EF server configuration and add DB context to Services
            var serverVersion = new MySqlServerVersion(hostBuilder.Configuration.GetSection("ConnectionStrings:ServerVersion").Value);
            hostBuilder.Services.AddDbContext<CheckIVABatchDBContext>(options =>
                options.UseMySql(hostBuilder.Configuration.GetSection("ConnectionStrings:DefaultConnectionMySQL").Value, serverVersion));

            IHost host = hostBuilder.Build();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("CHECK PARTITA IVA");
                Console.WriteLine("Menu' funzionalità");
                Console.WriteLine("1) Stampa la lista delle richieste effettuate in precedenza e salvate");
                Console.WriteLine("2) Effettua una nuova interrogazione");
                Console.WriteLine("3) Esci");
                Console.WriteLine();
                Console.Write("Inserisci il numero corrispondente all'azione che vuoi eseguire:");

                var commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case "1":
                        using (var scope = host.Services.CreateScope())
                        {
                            var repositoryService = scope.ServiceProvider.GetRequiredService<IRepositoryService>();

                        }
                        break;

                    case "2":
                        using (var scope = host.Services.CreateScope())
                        {
                            var vatService = scope.ServiceProvider.GetRequiredService<IVatService>();
                            await vatService.CheckVATNumber(new CheckVATNumberPostDTO()
                            {
                                CountryCode = "",
                                VatNumber = "IT00808220131",
                            });
                        }
                        break;

                    case "3":
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Scelta non consentita, per favore riprova");
                        break;
                }
            }
        }
    }
}

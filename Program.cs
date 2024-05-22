using checkPIVABatch.DTOs;
using checkPIVABatch.Interfaces;
using checkPIVABatch.Models;
using checkPIVABatch.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
                            var result = repositoryService.GetAllTaxInterrogationHistory();
                            if (result.Success == false)
                            {
                                Console.WriteLine(result.Message);
                                break;
                            }

                            if (result.Data.Count() == 0)
                            {
                                Console.WriteLine("There are no stored tax interrogation");
                                break;
                            }

                            Console.WriteLine("Richieste salvate:");
                            Console.WriteLine("Id | CountryCode | VatNumber | RequestDate | Valid | RequestIdentifier | Name | Address");
                            foreach (var item in result.Data)
                            {
                                Console.WriteLine(item.ToString());
                            }
                        }
                        break;

                    case "2":
                        Console.Write("Inserisci il codice nazione (ISO2) di domicilio della P.IVA:");
                        var countryCode = Console.ReadLine();
                        Console.Write("Inserisci il numero di partita IVA:");
                        var vatNumber = Console.ReadLine();

                        // Check if VAT number is already stored
                        Result<TaxInterrogationHistory> checkResult;
                        using (var scope = host.Services.CreateScope())
                        {
                            var repositoryService = scope.ServiceProvider.GetRequiredService<IRepositoryService>();
                            checkResult = repositoryService.GetTaxInterrogationHistoryByVatNumber(countryCode, vatNumber);
                        }

                        bool isVatNumberAlreadyStored = false;
                        TaxInterrogationHistory vatNumberAlreadyStoredEntity = new TaxInterrogationHistory();
                        if (checkResult.Success == true)
                        {
                            Console.WriteLine($"La partita iva con codice nazione {countryCode} e partitaIVA {vatNumber} e' gia' presente in database");
                            Console.Write($"Premere 1 per visualizzarla o 2 per eseguire comunque la ricerca: ");
                            var choice = Console.ReadLine();

                            if (choice == "1")
                            {
                                Console.WriteLine("Id | CountryCode | VatNumber | RequestDate | Valid | RequestIdentifier | Name | Address");
                                Console.WriteLine(checkResult.Data.ToString());
                                break;
                            }

                            isVatNumberAlreadyStored = true;
                            vatNumberAlreadyStoredEntity = checkResult.Data;
                        }

                        Result<CheckVATNumberResponseDTO> vatRequestResult;
                        using (var scope = host.Services.CreateScope())
                        {
                            var vatService = scope.ServiceProvider.GetRequiredService<IVatService>();
                            vatRequestResult = await vatService.CheckVATNumber(new CheckVATNumberPostDTO()
                            {
                                CountryCode = countryCode,
                                VatNumber = vatNumber,
                            });
                        }

                        if (vatRequestResult.Success == false)
                        {
                            Console.WriteLine(vatRequestResult.Message);
                            break;
                        }

                        Console.WriteLine("Richiesta effettuata con successo");

                        if (vatRequestResult.Data.Valid == false)
                        {
                            Console.WriteLine("P. IVA non valida");
                            break;
                        }

                        Console.WriteLine("CountryCode | VatNumber | RequestDate | Valid | RequestIdentifier | Name | Address");
                        Console.WriteLine(vatRequestResult.Data.ToString());

                        if (isVatNumberAlreadyStored)
                        {
                            Console.Write("Desideri sovreascrivere la ricerca in memoria con la seguente ricerca (SI/NO)? ");
                            var overwriteRequest = Console.ReadLine();

                            if (overwriteRequest.Equals("SI", StringComparison.InvariantCultureIgnoreCase))
                            {
                                Result<TaxInterrogationHistory> updateResult;
                                using (var scope = host.Services.CreateScope())
                                {
                                    var vatService = scope.ServiceProvider.GetRequiredService<IRepositoryService>();
                                    updateResult = vatService.UpdateTaxInterrogationHistory(vatNumberAlreadyStoredEntity.Id, new TaxInterrogationHistory()
                                    {
                                        Address = vatRequestResult.Data.Address,
                                        CountryCode = vatRequestResult.Data.CountryCode,
                                        Name = vatRequestResult.Data.Name,
                                        RequestDate = vatRequestResult.Data.RequestDate,
                                        RequestIdentifier = vatRequestResult.Data.RequestIdentifier,
                                        Valid = vatRequestResult.Data.Valid,
                                        VatNumber = vatRequestResult.Data.VatNumber,
                                    });
                                }

                                if (updateResult.Success == false)
                                {
                                    Console.WriteLine(updateResult.Message);
                                    break;
                                }

                                Console.WriteLine($"Richiesta sovrascritta con successo");
                            }
                            break;
                        }

                        Console.WriteLine();
                        Console.Write("Desideri salvare la seguente ricerca (SI/NO)? ");

                        var saveRequest = Console.ReadLine();
                        if (saveRequest.Equals("SI", StringComparison.InvariantCultureIgnoreCase))
                        {
                            Result<TaxInterrogationHistory> addResult;
                            using (var scope = host.Services.CreateScope())
                            {
                                var vatService = scope.ServiceProvider.GetRequiredService<IRepositoryService>();
                                addResult = vatService.AddTaxInterrogationHistory(new TaxInterrogationHistory()
                                {
                                    Address = vatRequestResult.Data.Address,
                                    CountryCode = vatRequestResult.Data.CountryCode,
                                    Name = vatRequestResult.Data.Name,
                                    RequestDate = vatRequestResult.Data.RequestDate,
                                    RequestIdentifier = vatRequestResult.Data.RequestIdentifier,
                                    Valid = vatRequestResult.Data.Valid,
                                    VatNumber = vatRequestResult.Data.VatNumber
                                });
                            }

                            if (addResult.Success == false)
                            {
                                Console.WriteLine(addResult.Message);
                                break;
                            }

                            Console.WriteLine($"Richiesta salvata con successo con Id: {addResult.Data.Id}");
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

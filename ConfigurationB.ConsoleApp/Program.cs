using ConfigurationB.Management.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ConfigurationB.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string w1 = System.AppDomain.CurrentDomain.FriendlyName;
            //string asd = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //string qwe = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            string basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));
            //string wanted_path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            //string w1 = Directory.GetCurrentDirectory();

            var builder = new ConfigurationBuilder()
         .SetBasePath(basePath)
         .AddJsonFile("appsettings.json", true, true);
            var configuration = builder.Build();

            ConfigurationReaderService configurationReaderService = new ConfigurationReaderService(configuration.GetConnectionString("ConfigureConnection"), 1000);

            while (true)
            {

                var siteName = await configurationReaderService.GetValueAsync<string>("SiteName");
                var isBasketEnabled = await configurationReaderService.GetValueAsync<bool>("IsBasketEnabled");
                var maxItemCount = await configurationReaderService.GetValueAsync<int>("MaxItemCount");
                var consoleApp = configurationReaderService.GetValue<string>("ConsoleApp");
                var consoleAppAsync = await configurationReaderService.GetValueAsync<string>("ConsoleAppAsync");
                var test = await configurationReaderService.GetValueAsync<int>("tst");

                Console.WriteLine(siteName);
                Console.WriteLine(isBasketEnabled);
                Console.WriteLine(maxItemCount);
                Console.WriteLine(consoleApp);
                Console.WriteLine(consoleAppAsync);
                Console.WriteLine(test);
                Thread.Sleep(5000);
            }

        }
    }
}

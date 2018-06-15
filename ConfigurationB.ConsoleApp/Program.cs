using ConfigurationB.Management.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;

namespace ConfigurationB.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
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

            ConfigurationReaderService configurationReaderService = new ConfigurationReaderService("SERVICE-A", configuration.GetConnectionString("ConfigureConnection"), 1000);

            while (true)
            {

                var siteName = configurationReaderService.GetValue<string>("SiteName");
                var isBasketEnabled = configurationReaderService.GetValue<bool>("IsBasketEnabled");
                var maxItemCount = configurationReaderService.GetValue<int>("MaxItemCount");
                var test = configurationReaderService.GetValue<int>("asdz");

                Console.WriteLine(siteName);
                Console.WriteLine(isBasketEnabled);
                Console.WriteLine(maxItemCount);
                Console.WriteLine(test);
                //Thread.Sleep(1000);
            }

        }
    }
}

using ConfigurationB.Management.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ConfigurationB.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string w1 = Directory.GetCurrentDirectory();

            var builder = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            ConfigurationReaderService configurationReaderService = new ConfigurationReaderService("SERVICE-A", configuration.GetConnectionString("ConfigureConnection"), 5000);
        }
    }
}

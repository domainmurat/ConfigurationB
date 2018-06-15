﻿using ConfigurationB.Management.Repositories;
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

                var appValue = configurationReaderService.GetValue<string>("SiteName");

                Console.WriteLine(configurationReaderService.ConfigurationItems.Count.ToString());
                Thread.Sleep(1000);
            }

        }
    }
}

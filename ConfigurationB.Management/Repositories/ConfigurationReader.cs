using ConfigurationB.Context.Management;
using ConfigurationB.Management.Common;
using ConfigurationB.Management.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConfigurationB.Management.Repositories
{
    public class ConfigurationReaderService : IConfigurationReaderServices
    {
        private string ApplicationName { get; set; }
        private int RefreshTimerIntervalInMs { get; set; }
        public IList<ConfigurationItem> ConfigurationItems;
        private readonly IAsyncRepository<ConfigurationItem> _configurationItemRepository;

        public ConfigurationReaderService(string applicationName, string connectionString, int refreshTimerIntervalInMs)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ConfigurationDbContext>().UseSqlServer(connectionString);

            _configurationItemRepository = new EFRepository<ConfigurationItem>(
                new ConfigurationDbContext(optionsBuilder.Options));

            this.ApplicationName = applicationName;
            this.RefreshTimerIntervalInMs = refreshTimerIntervalInMs;
            ConfigurationItems = new List<ConfigurationItem>();

            Run();
        }

        private void Run()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            ReadGivenAppSettingVariables(cancellationTokenSource.Token);
        }

        private async Task ReadGivenAppSettingVariables(CancellationToken cancellationToken)
        {
            while (true)
            {
                this.ConfigurationItems = await _configurationItemRepository.ListAsync(x => x.ApplicationName == this.ApplicationName
                                                      && x.IsActive == true);

                //this.ConfigurationItems = configurationItems.MapTo<ConfigurationItem, ConfigurationItem>(cfg =>
                //{
                //    cfg.CreateMap<ConfigurationItem, ConfigurationItem>();
                //});

                await Task.Delay(this.RefreshTimerIntervalInMs, cancellationToken);
            }
        }

        public T GetValue<T>(string key)
        {
            var configurationItem = ConfigurationItems.Where(x => x.Name == key).FirstOrDefault();

            return configurationItem != null && !configurationItem.Name.IsNullOrEmpty() ? configurationItem.Value.ChangeType<T>() : default(T);
        }
    }
}


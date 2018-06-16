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
        private IList<ConfigurationItem> ConfigurationItems;
        private readonly IAsyncRepository<ConfigurationItem> _configurationItemRepository;

        //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-2.1#timed-background-tasks
        public ConfigurationReaderService(string connectionString, int refreshTimerIntervalInMs)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ConfigurationDbContext>().UseSqlServer(connectionString);

            _configurationItemRepository = new EFRepository<ConfigurationItem>(
                new ConfigurationDbContext(optionsBuilder.Options));

            //this.ApplicationName = applicationName;
            //The following code prevents access to the values of another application
            this.ApplicationName = System.AppDomain.CurrentDomain.FriendlyName;
            this.RefreshTimerIntervalInMs = refreshTimerIntervalInMs;
            ConfigurationItems = new List<ConfigurationItem>();

            Start();
        }

        private void Start()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            ReadGivenAppSettingVariables(cancellationTokenSource.Token);
        }
        /// <summary>
        /// This method works in interval time with the value assigned to the RefreshTimerIntervalInMs
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task ReadGivenAppSettingVariables(CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    var configurationItems = await _configurationItemRepository.ListAsync(x => x.ApplicationName == this.ApplicationName
                                                          && x.IsActive == true);

                    //if configurationItems is null or empty this.ConfigurationItems keeps last values and app works with last values
                    if (configurationItems.Any())
                    {
                        //we are fetch all data from db and set to this.ConfigurationItems  this provides us to keep last updated values
                        this.ConfigurationItems = configurationItems;

                        //Automapper temporarily taken to the comments line
                        //this.ConfigurationItems = configurationItems.MapTo<ConfigurationItem, ConfigurationItem>(cfg =>
                        //{
                        //    cfg.CreateMap<ConfigurationItem, ConfigurationItem>();
                        //});
                    }

                    await Task.Delay(this.RefreshTimerIntervalInMs, cancellationToken);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public T GetValue<T>(string key)
        {
            return GetValueCommon<T>(key);
        }

        public async Task<T> GetValueAsync<T>(string key)
        {
            return await Task.Run(() => GetValueCommon<T>(key));
        }

        private T GetValueCommon<T>(string key)
        {
            var configurationItem = ConfigurationItems.Where(x => x.Name == key).FirstOrDefault();

            return configurationItem != null && !configurationItem.Name.IsNullOrEmpty() ? configurationItem.Value.ChangeType<T>() : default(T);
        }
    }
}


# ConfigurationB

Bu projede tüm bussiness logic ConfigurationB.Management projesine yüklenilmiştir. ConfigurationB.Management yapısı herhangi bir Dependency Injectiona bağlanabilecek şekilde yazılmıştır. İçerisinde EntityFrameworkCore ile MsSql e CRUD işlemlerinin yapıldığı Repository Pattern ve belirli aralıklarla storage da tutulan verileri okuyan yapı vardır. İşlemlerin çoğu async/await şeklinde çalışmaktadır.

* ConfigurationB.Management ı Aspnet mvc core projesi olan ConfigurationB.MVC e referans vererek gerekli olan CRUD işlemleri buradan sağlanmaktadır. Aspnet core da default olarak gelen Dependency Injection yapısı kullanıldı.

```javascript
public class HomeController : Controller
    {
        private readonly IAsyncRepository<ConfigurationItem> _configurationItemRepository;
        public HomeController(IAsyncRepository<ConfigurationItem> configurationItemRepository)
        {
            _configurationItemRepository = configurationItemRepository;
        }

        public async Task<IActionResult> Index()
        {
            List<ConfigurationItem> configurationItems = await _configurationItemRepository.ListAllAsync();
            return View(configurationItems);
        }
```

* ConfigurationB.Management ı ConfigurationB.ConsoleApp a referans vererk ise storage daki düzenli son veri okumayı sağlıyoruz. Bu yapı bir background work şeklinde çalışmaktadır.
Aşağıda gördüğümüz şekilde ConfigurationReaderService instance alıyoruz sonrasında instance ile ConfigurationReaderService içerisindeki ReadGivenAppSettingVariables methodu çalışır. Bu method refreshTimerIntervalInMs a verilen değer ile düzenli olarak çalışıyor ve ConfigurationItems listesini güncelliyor. GetValue methodu ilede gidip bu listin içinden istenilen veriyi alıyoruz. 
Başka bir applicationın verileri alınmasın diye uygulama adını ConfigurationReaderService constructorının içerisinde ```     this.ApplicationName = System.AppDomain.CurrentDomain.FriendlyName; ``` ile veriyoruz.

```javascript
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
```

```

* Storage olarak MsSSql tutulduğundan uygulama başlatılmadan önce projenin çalıştırıalcağı pcde MsSql ve dotnet core olmalı ve aşağıdaki "dotnet ef database update" komutu database oluşması için CMD de çalıştırılmalıdır.

-- navigate to webfolder or console apllication then create migration or directly update database
dotnet ef migrations add InitialModel --context configurationdbcontext -p ../ConfigurationB.Management/ConfigurationB.Management.csproj -s ConfigurationB.MVC.csproj -o Migrations

dotnet ef database update -c configurationdbcontext -p ../ConfigurationB.Management/ConfigurationB.Management.csproj -s ConfigurationB.MVC.csproj
```

Aşağıdaki method ile belirlenmiş aralıkalrda storage dan verilerin son hali alınarak direkman set edildiğinden en son hali hep güncel kalıyor eğer bir hata oluşmadıysa hata var ise en son hali kalıyor. O yüzden şimdilik yeni veri eklenmişmi veya verilerde güncellenme olmuşmu diye teker teker bakılmadı.

```javascript
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
                        this.ConfigurationItems = configurationItems.MapTo<ConfigurationItem, ConfigurationItem>(cfg =>
                        {
                            cfg.CreateMap<ConfigurationItem, ConfigurationItem>();
                        });
                    }

                    await Task.Delay(this.RefreshTimerIntervalInMs, cancellationToken);
                }
            }
            catch (Exception ex)
            {
            }
        }
```

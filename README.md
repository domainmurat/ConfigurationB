# ConfigurationB

Bu projede tüm bussiness logic ConfigurationB.Management projesine yüklenilmiştir. ConfigurationB.Management yapısı herhangi bir Dependency Injectiona bağlanabilecek şekilde yazılmıştır. İçerisinde EntityFrameworkCore işe MsSql e CRUD işlemlerinin yapıldığı Repository Pattern ve belirli aralıklarla storage da tutulan verileri okuyan yapı vardır. İşlemlerin çoğu async/await şeklinde çalışmaktadır.

```
-- navigate to webfolder or console apllication then create migration or directly update database
dotnet ef migrations add InitialModel --context configurationdbcontext -p ../ConfigurationB.Management/ConfigurationB.Management.csproj -s ConfigurationB.MVC.csproj -o Migrations

dotnet ef database update -c configurationdbcontext -p ../ConfigurationB.Management/ConfigurationB.Management.csproj -s ConfigurationB.MVC.csproj
```

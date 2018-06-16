# ConfigurationB



```
-- navigate to webfolder or console apllication then create migration or directly update database
dotnet ef migrations add InitialModel --context configurationdbcontext -p ../ConfigurationB.Management/ConfigurationB.Management.csproj -s ConfigurationB.MVC.csproj -o Migrations

dotnet ef database update -c configurationdbcontext -p ../ConfigurationB.Management/ConfigurationB.Management.csproj -s ConfigurationB.MVC.csproj
```

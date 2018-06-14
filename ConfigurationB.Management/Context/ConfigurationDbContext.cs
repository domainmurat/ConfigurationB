using ConfigurationB.Management.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ConfigurationB.Context.Management
{
    public class ConfigurationDbContext : DbContext
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var builder = new ConfigurationBuilder()
        //    .SetBasePath(Directory.GetCurrentDirectory())
        //    .AddJsonFile("appsettings.json");

        //    var configuration = builder.Build();

        //    optionsBuilder.UseSqlServer(configuration.GetConnectionString("ConfigureConnection"));
        //}

        public ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options) : base(options)
        {

        }
        public DbSet<ConfigurationItem> ConfigurationItem { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ConfigurationItem>(ConfigureConfigurationItem);
        }

        private void ConfigureConfigurationItem(EntityTypeBuilder<ConfigurationItem> builder)
        {
            builder.ToTable("ConfigurationItem");

            builder.HasKey(ci => ci.Id);
        }
    }
}

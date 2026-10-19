using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace RaceDay_API.Data
{
    public class RaceDayDbContextFactory : IDesignTimeDbContextFactory<RaceDayDbContext>
    {
        public RaceDayDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<RaceDayDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new RaceDayDbContext(optionsBuilder.Options);
        }
    }
}
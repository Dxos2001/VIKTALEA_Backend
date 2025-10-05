using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using VIKTALEA_Backend.Models;

namespace Viktalea.Backend.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var env = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .AddUserSecrets<AppDbContextFactory>(optional: true)
                .AddEnvironmentVariables()
                .Build();

            var cs = config.GetConnectionString("OracleDbConnection") ?? Environment.GetEnvironmentVariable("ConnectionStrings_OracleDbConnection") ?? throw new InvalidOperationException("Falta ConnectionStrings:OracleDbConnection");
            if (string.IsNullOrWhiteSpace(cs))
                throw new InvalidOperationException("Falta ConnectionStrings:OracleDbConnection");

            var opts = new DbContextOptionsBuilder<AppDbContext>()
                .UseOracle(cs) // provider Oracle
                .Options;

            return new AppDbContext(opts);
        }
    }
}

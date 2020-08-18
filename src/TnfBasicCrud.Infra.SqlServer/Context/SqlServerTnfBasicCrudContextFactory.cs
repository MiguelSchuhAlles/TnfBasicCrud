using TnfBasicCrud.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using Tnf.Runtime.Session;

namespace TnfBasicCrud.Infra.SqlServer.Context
{
    public class SqlServerTnfBasicCrudContextFactory : IDesignTimeDbContextFactory<SqlServerTnfBasicCrudContext>
    {
        public SqlServerTnfBasicCrudContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<TnfBasicCrudContext>();

            var configuration = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile($"appsettings.Development.json", false)
                                    .Build();

            var databaseConfiguration = new DatabaseConfiguration(configuration);

            builder.UseSqlServer(databaseConfiguration.ConnectionString);

            return new SqlServerTnfBasicCrudContext(builder.Options, NullTnfSession.Instance);
        }
    }
}

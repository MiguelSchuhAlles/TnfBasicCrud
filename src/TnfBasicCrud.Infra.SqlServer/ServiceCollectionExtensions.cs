using TnfBasicCrud.Domain;
using TnfBasicCrud.Infra.Context;
using TnfBasicCrud.Infra.SqlServer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TnfBasicCrud.Infra;

namespace BasicCrud.Infra.SqlServer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlServerDependency(this IServiceCollection services)
        {
            services
                .AddInfraDependency()
                .AddTnfDbContext<TnfBasicCrudContext, SqlServerTnfBasicCrudContext>((config) =>
                {
                    if (Constants.IsDevelopment())
                    {
                        config.DbContextOptions.EnableSensitiveDataLogging();
                        config.UseLoggerFactory();
                    }

                    if (config.ExistingConnection != null)
                        config.DbContextOptions.UseSqlServer(config.ExistingConnection);
                    else
                        config.DbContextOptions.UseSqlServer(config.ConnectionString);
                });

            return services;
        }
    }
}

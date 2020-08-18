using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BasicCrud.Infra.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using TnfBasicCrud.Application;
using TnfBasicCrud.Common;
using TnfBasicCrud.Domain;
using TnfBasicCrud.Domain.Entities;
using TnfBasicCrud.Infra;

namespace TnfBasicCrud.API
{
    public class Startup
    {
        DatabaseConfiguration DatabaseConfiguration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            DatabaseConfiguration = new DatabaseConfiguration(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Chaveamento de qual banco a aplicação irá usar
            services
                .AddCorsAll("AllowAll")
                .AddApplicationServiceDependency();  // dependencia da camada BasicCrud.Application

            services.AddTnfAspNetCore(builder =>
            {
                builder.UseDomainLocalization();

                // Configuração global de como irá funcionar o Get utilizando o repositorio do Tnf
                // O exemplo abaixo registra esse comportamento através de uma convenção:
                // toda classe que implementar essas interfaces irão ter essa configuração definida
                // quando for consultado um método que receba a interface IRequestDto do Tnf
                builder.Repository(repositoryConfig =>
                {
                    repositoryConfig.Entity<IEntity>(entity =>
                        entity.RequestDto<IDefaultRequestDto>((e, d) => e.Id == d.Id));
                });

                // Configura a connection string da aplicação
                builder.DefaultConnectionString(DatabaseConfiguration.ConnectionString);

                // Altera o default isolation level para Unspecified (SqlLite não trabalha com isolationLevel)
                //options.UnitOfWorkOptions().IsolationLevel = IsolationLevel.Unspecified;

                // Altera o default isolation level para ReadCommitted (ReadUnCommited not supported by Devart)
                //options.UnitOfWorkOptions().IsolationLevel = IsolationLevel.ReadCommitted;
            });

            //services.AddControllers();

            services.AddSingleton(DatabaseConfiguration);

            if (DatabaseConfiguration.DatabaseType == DatabaseType.SqlServer)
                services.AddSqlServerDependency();
            else
                throw new NotSupportedException("No database configuration found");

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TNF Basic CRUD API", Version = "v1" });
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "TnfBasicCrud.API.xml"));
            });

            services.AddResponseCompression();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowAll");

            app.UseTnfAspNetCore();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseResponseCompression();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TNF Basic CRUD API v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

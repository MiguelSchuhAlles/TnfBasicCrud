using TnfBasicCrud.Application.Services.Interfaces;
using TnfBasicCrud.API.Controllers;
using TnfBasicCrud.API.Tests.Mocks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace TnfBasicCrud.API.Tests
{
    public class StartupControllerTest
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Configura o setup de teste para AspNetCore
            services.AddTnfAspNetCoreSetupTest();

            // Registro dos serviços de Mock
            services.AddTransient<ICustomerAppService, CustomerAppServiceMock>();
            services.AddTransient<IProductAppService, ProductAppServiceMock>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            // Configura o uso do teste
            app.UseTnfAspNetCoreSetupTest();

            app.UseEndpoints(endpoint =>
            {
                endpoint.MapDefaultControllerRoute();
            });
        }
    }
}

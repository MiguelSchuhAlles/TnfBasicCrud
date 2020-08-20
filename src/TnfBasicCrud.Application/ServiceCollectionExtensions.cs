using TnfBasicCrud.Application.Services;
using TnfBasicCrud.Application.Services.Interfaces;
using TnfBasicCrud.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace TnfBasicCrud.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServiceDependency(this IServiceCollection services)
        {
            // Dependencia do projeto TnfBasicCrud.Domain
            services.AddDomainDependency();

            // Para habilitar as convenções do Tnf para Injeção de dependência (ITransientDependency, IScopedDependency, ISingletonDependency)
            // descomente a linha abaixo:
            // services.AddTnfDefaultConventionalRegistrations();

            // Registro dos serviços
            services.AddTransient<ICustomerAppService, CustomerAppService>();
            services.AddTransient<IProductAppService, ProductAppService>();
            services.AddTransient<IPurchaseAppService, PurchaseAppService>();

            return services;
        }
    }
}

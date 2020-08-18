using AutoMapper;
using TnfBasicCrud.Domain.Entities;
using TnfBasicCrud.Common.Customer;
using TnfBasicCrud.Common.Product;
using TnfBasicCrud.Common.Purchase;

namespace TnfBasicCrud.Infra.MapperProfiles
{
    public class TnfBasicCrudProfile : Profile
    {
        public TnfBasicCrudProfile()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<Product, ProductDto>();
            CreateMap<Purchase, PurchaseDto>();
        }
    }
}

using TnfBasicCrud.Common;
using TnfBasicCrud.Common.Product;
using System;
using System.Threading.Tasks;
using Tnf.Application.Services;
using Tnf.Dto;

namespace TnfBasicCrud.Application.Services.Interfaces
{
    // Para que essa interface seja registrada por convenção ela precisa herdar de alguma dessas interfaces: ITransientDependency, IScopedDependency, ISingletonDependency
    public interface IProductAppService : IApplicationService
    {
        Task<ProductDto> CreateAsync(ProductDto product);
        Task<ProductDto> UpdateAsync(Guid id, ProductDto product);
        Task DeleteAsync(Guid id);
        Task<ProductDto> GetAsync(DefaultRequestDto id);
        Task<IListDto<ProductDto>> GetAllAsync(ProductRequestAllDto request);
    }
}

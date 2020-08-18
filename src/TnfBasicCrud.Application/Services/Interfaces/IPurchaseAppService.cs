using TnfBasicCrud.Common;
using TnfBasicCrud.Common.Purchase;
using System;
using System.Threading.Tasks;
using Tnf.Application.Services;
using Tnf.Dto;

namespace TnfBasicCrud.Application.Services.Interfaces
{
    // Para que essa interface seja registrada por convenção ela precisa herdar de alguma dessas interfaces: ITransientDependency, IScopedDependency, ISingletonDependency
    public interface IPurchaseAppService : IApplicationService
    {
        Task<PurchaseDto> CreateAsync(PurchaseDto purchase);
        Task<PurchaseDto> UpdateAsync(Guid id, PurchaseDto purchase);
        Task DeleteAsync(Guid id);
        Task<PurchaseDto> GetAsync(DefaultRequestDto id);
        Task<IListDto<PurchaseDto>> GetAllAsync(PurchaseRequestAllDto request);
    }
}

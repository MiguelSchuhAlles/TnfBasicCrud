using TnfBasicCrud.Application.Services.Interfaces;
using TnfBasicCrud.Domain.Entities;
using TnfBasicCrud.Common;
using TnfBasicCrud.Common.Purchase;
using System;
using System.Threading.Tasks;
using Tnf.Application.Services;
using Tnf.Dto;
using Tnf.Notifications;
using Tnf.Repositories.Uow;
using Tnf.Domain.Services;

namespace TnfBasicCrud.Application.Services
{
    public class PurchaseAppService : ApplicationService, IPurchaseAppService
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IDomainService<Purchase> _service;

        public PurchaseAppService(
            IUnitOfWorkManager unitOfWorkManager,
            IDomainService<Purchase> service,
            INotificationHandler notificationHandler)
            : base(notificationHandler)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _service = service;
        }

        public async Task<PurchaseDto> CreateAsync(PurchaseDto dto)
        {
            if (!ValidateDto<PurchaseDto>(dto))
                return null;

            var builder = Purchase.Create(Notification)
                .WithDescription(dto.Description)
                .WithCustomerId(dto.CustomerId)
                .WithProductId(dto.ProductId)
                .WithDate(dto.Date)
                .WithQuantity(dto.Quantity);

            Purchase entity = null;

            using (var uow = _unitOfWorkManager.Begin())
            {
                entity = await _service.InsertAndSaveChangesAsync(builder);

                await uow.CompleteAsync().ForAwait();
            }

            if (Notification.HasNotification())
                return null;

            return entity.MapTo<PurchaseDto>();
        }

        public async Task<PurchaseDto> UpdateAsync(Guid id, PurchaseDto dto)
        {
            if (!ValidateDtoAndId(dto, id))
                return null;

            var builder = Purchase.Create(Notification)
                .WithId(id)
                .WithDescription(dto.Description)
                .WithCustomerId(dto.CustomerId)
                .WithProductId(dto.ProductId)
                .WithDate(dto.Date)
                .WithQuantity(dto.Quantity);

            using (var uow = _unitOfWorkManager.Begin())
            {
                await _service.UpdateAsync(builder);

                await uow.CompleteAsync().ForAwait();
            }

            dto.Id = id;
            return dto;
        }

        public async Task DeleteAsync(Guid id)
        {
            if (!ValidateId(id))
                return;

            using (var uow = _unitOfWorkManager.Begin())
            {
                await _service.DeleteAsync(p => p.Id == id);

                await uow.CompleteAsync().ForAwait();
            }
        }

        public async Task<PurchaseDto> GetAsync(DefaultRequestDto id)
        {
            if (!ValidateRequestDto(id) || !ValidateId<Guid>(id.Id))
                return null;

            var entity = await _service.GetAsync(id);

            return entity.MapTo<PurchaseDto>();
        }

        public async Task<IListDto<PurchaseDto>> GetAllAsync(PurchaseRequestAllDto request)
            => await _service.GetAllAsync<PurchaseDto>(request
                , p => request.Description.IsNullOrEmpty() || p.Description.Contains(request.Description));
    }
}

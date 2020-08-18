using TnfBasicCrud.Application.Services.Interfaces;
using TnfBasicCrud.Domain.Entities;
using TnfBasicCrud.Common;
using TnfBasicCrud.Common.Product;
using System;
using System.Threading.Tasks;
using Tnf.Application.Services;
using Tnf.Dto;
using Tnf.Notifications;
using Tnf.Repositories.Uow;
using Tnf.Domain.Services;

namespace TnfBasicCrud.Application.Services
{
    public class ProductAppService : ApplicationService, IProductAppService
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IDomainService<Product> _service;

        public ProductAppService(
            IUnitOfWorkManager unitOfWorkManager,
            IDomainService<Product> service,
            INotificationHandler notificationHandler)
            : base(notificationHandler)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _service = service;
        }

        public async Task<ProductDto> CreateAsync(ProductDto dto)
        {
            if (!ValidateDto<ProductDto>(dto))
                return null;

            var builder = Product.Create(Notification)
                .WithDescription(dto.Description)
                .WithValue(dto.Value);

            Product entity = null;

            using (var uow = _unitOfWorkManager.Begin())
            {
                entity = await _service.InsertAndSaveChangesAsync(builder);

                await uow.CompleteAsync().ForAwait();
            }

            if (Notification.HasNotification())
                return null;

            return entity.MapTo<ProductDto>();
        }

        public async Task<ProductDto> UpdateAsync(Guid id, ProductDto dto)
        {
            if (!ValidateDtoAndId(dto, id))
                return null;

            var builder = Product.Create(Notification)
                .WithId(id)
                .WithDescription(dto.Description)
                .WithValue(dto.Value);

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

        public async Task<ProductDto> GetAsync(DefaultRequestDto id)
        {
            if (!ValidateRequestDto(id) || !ValidateId<Guid>(id.Id))
                return null;

            var entity = await _service.GetAsync(id);

            return entity.MapTo<ProductDto>();
        }

        public async Task<IListDto<ProductDto>> GetAllAsync(ProductRequestAllDto request)
            => await _service.GetAllAsync<ProductDto>(request
                , p => request.Description.IsNullOrEmpty() || p.Description.Contains(request.Description));
    }
}

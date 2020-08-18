using TnfBasicCrud.Application.Services.Interfaces;
using TnfBasicCrud.Common;
using TnfBasicCrud.Common.Purchase;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Tnf.AspNetCore.Mvc.Response;
using Tnf.Dto;

namespace TnfBasicCrud.API.Controllers
{
    /// <summary>
    /// Purchase API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : TnfController
    {
        private readonly IPurchaseAppService _appService;
        private const string _name = "Purchase";

        public PurchasesController(IPurchaseAppService appService)
        {
            _appService = appService;
        }

        /// <summary>
        /// Get all purchases
        /// </summary>
        /// <param name="requestDto">Request params</param>
        /// <returns>List of purchases</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IListDto<PurchaseDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> GetAll([FromQuery] PurchaseRequestAllDto requestDto)
        {
            var response = await _appService.GetAllAsync(requestDto);

            return CreateResponseOnGetAll(response, _name);
        }

        /// <summary>
        /// Get purchase
        /// </summary>
        /// <param name="id">Purchase id</param>
        /// <param name="requestDto">Request params</param>
        /// <returns>Purchase requested</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PurchaseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Get(Guid id, [FromQuery] RequestDto requestDto)
        {
            var request = new DefaultRequestDto(id, requestDto);

            var response = await _appService.GetAsync(request);

            return CreateResponseOnGet(response, _name);
        }

        /// <summary>
        /// Create a new purchase
        /// </summary>
        /// <param name="purchaseDto">Purchase to create</param>
        /// <returns>Purchase created</returns>
        [HttpPost]
        [ProducesResponseType(typeof(PurchaseDto), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Post([FromBody] PurchaseDto purchaseDto)
        {
            purchaseDto = await _appService.CreateAsync(purchaseDto);

            return CreateResponseOnPost(purchaseDto, _name);
        }

        /// <summary>
        /// Update a purchase
        /// </summary>
        /// <param name="id">Purchase id</param>
        /// <param name="purchaseDto">Purchase content to update</param>
        /// <returns>Updated purchase</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PurchaseDto), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Put(Guid id, [FromBody] PurchaseDto purchaseDto)
        {
            purchaseDto = await _appService.UpdateAsync(id, purchaseDto);

            return CreateResponseOnPut(purchaseDto, _name);
        }

        /// <summary>
        /// Delete a purchase
        /// </summary>
        /// <param name="id">Purchase id</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _appService.DeleteAsync(id);

            return CreateResponseOnDelete(_name);
        }
    }
}

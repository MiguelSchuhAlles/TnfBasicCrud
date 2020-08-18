using System;
using Tnf.Dto;

namespace TnfBasicCrud.Common.Purchase
{
    public class PurchaseDto : BaseDto
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public Guid ProductId { get; set; }

        public Guid CustomerId { get; set; }

        public DateTime Date { get; set; }
    }
}

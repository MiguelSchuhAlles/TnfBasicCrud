using System;
using Tnf.Dto;

namespace TnfBasicCrud.Common.Customer
{
    public class CustomerDto : BaseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}

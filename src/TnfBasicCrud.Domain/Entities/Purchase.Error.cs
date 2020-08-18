using System;
using System.Collections.Generic;
using System.Text;

namespace TnfBasicCrud.Domain.Entities
{
    public partial class Purchase
    {
        public enum Error
        {
            PurchaseShouldHaveCustomer,
            PurchaseShouldHaveProduct,
            PurchaseShouldHaveQuantity
        }
    }
}

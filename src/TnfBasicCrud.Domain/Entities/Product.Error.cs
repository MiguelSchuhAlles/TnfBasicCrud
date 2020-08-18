using System;
using System.Collections.Generic;
using System.Text;

namespace TnfBasicCrud.Domain.Entities
{
    public partial class Product
    {
        public enum Error
        {
            ProductShouldHaveDescription,
            ProductShouldHaveValue
        }
    }
}

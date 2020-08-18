using System;
using System.Linq.Expressions;
using Tnf.Specifications;

namespace TnfBasicCrud.Domain.Entities.Specifications
{
    public class PurchaseShouldHaveCustomer : Specification<Purchase>
    {
        public override string LocalizationSource { get; protected set; } = Constants.LocalizationSourceName;
        public override Enum LocalizationKey { get; protected set; } = Purchase.Error.PurchaseShouldHaveCustomer;

        public override Expression<Func<Purchase, bool>> ToExpression()
        {
            return p => p.CustomerId != null && p.CustomerId != Guid.Empty;
        }
    }
}

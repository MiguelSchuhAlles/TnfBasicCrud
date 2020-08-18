using System;
using Tnf.Builder;
using Tnf.Notifications;
using TnfBasicCrud.Domain.Entities.Specifications;

namespace TnfBasicCrud.Domain.Entities
{
    public partial class Purchase
    {
        public class Builder : Builder<Purchase>
        {
            public Builder(INotificationHandler notificationHandler)
                : base(notificationHandler)
            {
            }

            public Builder(INotificationHandler notificationHandler, Purchase instance)
                : base(notificationHandler, instance)
            {
            }

            public Builder WithId(Guid id)
            {
                Instance.Id = id;
                return this;
            }

            public Builder WithDescription(string description)
            {
                Instance.Description = description;
                return this;
            }

            public Builder WithQuantity(int quantity)
            {
                Instance.Quantity = quantity;
                return this;
            }

            public Builder WithProductId(Guid productId)
            {
                Instance.ProductId = productId;
                return this;
            }

            public Builder WithCustomerId(Guid customerId)
            {
                Instance.CustomerId = customerId;
                return this;
            }

            public Builder WithDate(DateTime date)
            {
                Instance.Date = date;
                return this;
            }

            protected override void Specifications()
            {
                AddSpecification<PurchaseShouldHaveCustomer>();
                AddSpecification<PurchaseShouldHaveProduct>();
                AddSpecification<PurchaseShouldHaveQuantity>();
            }
        }
    }
}

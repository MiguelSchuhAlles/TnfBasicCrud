using System;
using Tnf.Notifications;

namespace TnfBasicCrud.Domain.Entities
{

    public partial class Purchase : IEntity
    {
        public Guid Id { get; set; }

        public string Description { get; internal set; }

        public int Quantity { get; internal set; }

        public Guid ProductId { get; set; }

        public Guid CustomerId { get; set; }

        public DateTime Date { get; set; }

        public Product Product { get; set; }

        public Customer Customer { get; set; }

        public static Builder Create(INotificationHandler handler)
            => new Builder(handler);

        public static Builder Create(INotificationHandler handler, Purchase instance)
            => new Builder(handler, instance);
    }
}

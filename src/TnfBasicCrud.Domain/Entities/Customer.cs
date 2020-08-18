using System;
using System.Collections.Generic;
using Tnf.Notifications;

namespace TnfBasicCrud.Domain.Entities
{
    public partial class Customer : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; internal set; }

        public ICollection<Purchase> Purchases { get; set; }

        public static Builder Create(INotificationHandler handler)
            => new Builder(handler);

        public static Builder Create(INotificationHandler handler, Customer instance)
            => new Builder(handler, instance);
    }
}

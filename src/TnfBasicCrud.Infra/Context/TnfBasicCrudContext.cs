using Microsoft.EntityFrameworkCore;
using System;
using Tnf.EntityFrameworkCore;
using Tnf.Runtime.Session;
using TnfBasicCrud.Domain.Entities;

namespace TnfBasicCrud.Infra.Context
{
    public abstract class TnfBasicCrudContext : TnfDbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }


        // Importante o construtor do contexto receber as opções com o tipo generico definido: DbContextOptions<TDbContext>
        public TnfBasicCrudContext(DbContextOptions<TnfBasicCrudContext> options, ITnfSession session)
            : base(options, session)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureCustomer(modelBuilder);
            ConfigureProduct(modelBuilder);
            ConfigurePurchase(modelBuilder);
        }

        public void ConfigureCustomer(ModelBuilder builder)
        {
            builder.Entity<Customer>(e =>
            {
                e.ToTable("Customers");
                e.HasKey(k => k.Id);
                e.Property(p => p.Name).IsRequired();
            });
        }

        public void ConfigureProduct(ModelBuilder builder)
        {
            builder.Entity<Product>(e =>
            {
                e.ToTable("Products");
                e.HasKey(k => k.Id);
                e.Property(p => p.Description).IsRequired();
                e.Property(p => p.Value).IsRequired();
            });
        }

        private void ConfigurePurchase(ModelBuilder builder)
        {
            builder.Entity<Purchase>(e =>
            {
                e.ToTable("Purchases");
                e.HasKey(k => k.Id);
                e.HasOne(p => p.Customer).WithMany(c => c.Purchases);
                e.HasOne(p => p.Product).WithMany(p => p.Purchases);
                e.Property(p => p.Quantity).IsRequired();
            });
        }
    }
}
